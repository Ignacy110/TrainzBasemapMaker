// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TrainzBasemapMaker.Classes
{
    /// <summary>
    /// Map provider implementation for Slippy Map / XYZ tile services with optional overlay compositing.
    /// </summary>
    internal class XyzTileMapSource : MapSourceBase
    {
        public string BaseUrl { get; }
        public string OverlayUrl { get; }
        public override bool AllowsHighResolution => true;

        public XyzTileMapSource(string name, string baseUrl, string overlayUrl = "")
            : base(name, false)
        {
            BaseUrl = baseUrl;
            OverlayUrl = overlayUrl;
        }

        public override async Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3, CancellationToken cancellationToken = default)
        {
            const int zoom = 18;
            double n = Math.Pow(2, zoom);

            double tlX, tlY, trX, trY, blX, blY, brX, brY;

            if (GeoHelperEPSG2180.IsWithin2180Bounds(xCenter, yCenter))
            {
                // EPSG:2180 (Poland CS92)
                double halfSize = TileSize / 2.0;
                var (tlLat, tlLon) = GeoHelperEPSG2180.Meters2180ToLatLon(xCenter - halfSize, yCenter + halfSize);
                var (trLat, trLon) = GeoHelperEPSG2180.Meters2180ToLatLon(xCenter + halfSize, yCenter + halfSize);
                var (blLat, blLon) = GeoHelperEPSG2180.Meters2180ToLatLon(xCenter - halfSize, yCenter - halfSize);
                var (brLat, brLon) = GeoHelperEPSG2180.Meters2180ToLatLon(xCenter + halfSize, yCenter - halfSize);

                (tlX, tlY) = LatLonToTile(tlLat, tlLon, zoom);
                (trX, trY) = LatLonToTile(trLat, trLon, zoom);
                (blX, blY) = LatLonToTile(blLat, blLon, zoom);
                (brX, brY) = LatLonToTile(brLat, brLon, zoom);
            }
            else
            {
                // EPSG:3857 (Web Mercator / Pseudo-Mercator)
                var (centerLat, _) = GeoHelperEPSG3857.Meters3857ToLatLon(xCenter, yCenter);
                double cosLat = Math.Max(0.01, Math.Cos(centerLat * Math.PI / 180.0));
                double halfSpan = (TileSize / 2.0) / cosLat;

                double minX = xCenter - halfSpan;
                double maxX = xCenter + halfSpan;
                double minY = yCenter - halfSpan;
                double maxY = yCenter + halfSpan;

                double originShift = GeoHelperEPSG3857.OriginShift;

                tlX = (minX + originShift) / (2.0 * originShift) * n;
                trX = (maxX + originShift) / (2.0 * originShift) * n;
                blX = tlX;
                brX = trX;

                tlY = (originShift - maxY) / (2.0 * originShift) * n;
                trY = tlY;
                blY = (originShift - minY) / (2.0 * originShift) * n;
                brY = blY;
            }

            int minTileX = (int)Math.Floor(Math.Min(Math.Min(tlX, trX), Math.Min(blX, brX)));
            int maxTileX = (int)Math.Floor(Math.Max(Math.Max(tlX, trX), Math.Max(blX, brX)));
            int minTileY = (int)Math.Floor(Math.Min(Math.Min(tlY, trY), Math.Min(blY, brY)));
            int maxTileY = (int)Math.Floor(Math.Max(Math.Max(tlY, trY), Math.Max(blY, brY)));

            int tilesNumX = maxTileX - minTileX + 1;
            int tilesNumY = maxTileY - minTileY + 1;

            var downloadTasks = new List<Task<(int tx, int ty, byte[]? baseBytes, byte[]? overlayBytes)>>();

            for (int ty = minTileY; ty <= maxTileY; ty++)
            {
                for (int tx = minTileX; tx <= maxTileX; tx++)
                {
                    int currentTx = tx;
                    int currentTy = ty;
                    string sub = "abc"[Math.Abs(currentTx + currentTy) % 3].ToString();

                    string baseUrlFormatted = BaseUrl
                        .Replace("{s}", sub)
                        .Replace("{z}", zoom.ToString())
                        .Replace("{x}", currentTx.ToString())
                        .Replace("{y}", currentTy.ToString());

                    string overlayUrlFormatted = string.IsNullOrEmpty(OverlayUrl)
                        ? ""
                        : OverlayUrl
                            .Replace("{s}", sub)
                            .Replace("{z}", zoom.ToString())
                            .Replace("{x}", currentTx.ToString())
                            .Replace("{y}", currentTy.ToString());

                    downloadTasks.Add(Task.Run(async () =>
                    {
                        byte[]? baseBytes = await FetchTileBytesWithRetryAsync(baseUrlFormatted, maxRetries, delaySeconds, cancellationToken);
                        byte[]? overlayBytes = null;
                        if (!string.IsNullOrEmpty(overlayUrlFormatted))
                        {
                            overlayBytes = await FetchTileBytesWithRetryAsync(overlayUrlFormatted, maxRetries, delaySeconds, cancellationToken);
                        }
                        return (currentTx, currentTy, baseBytes, overlayBytes);
                    }, cancellationToken));
                }
            }

            var results = await Task.WhenAll(downloadTasks);

            if (results.All(r => r.baseBytes == null || r.baseBytes.Length == 0))
            {
                throw new HttpRequestException("Serwer kafelków XYZ nie zwrócił żadnych kafelków dla wybranego obszaru.");
            }

            int stitchedWidth = tilesNumX * Constants.XyzTilePixelSize;
            int stitchedHeight = tilesNumY * Constants.XyzTilePixelSize;

            using Bitmap stitchedBitmap = new Bitmap(stitchedWidth, stitchedHeight);
            using (Graphics gStitch = Graphics.FromImage(stitchedBitmap))
            {
                gStitch.Clear(Color.White);
                foreach (var result in results)
                {
                    int posX = (result.tx - minTileX) * Constants.XyzTilePixelSize;
                    int posY = (result.ty - minTileY) * Constants.XyzTilePixelSize;

                    if (result.baseBytes != null && result.baseBytes.Length > 0)
                    {
                        using MemoryStream msBase = new MemoryStream(result.baseBytes);
                        using Image baseImg = Image.FromStream(msBase);
                        gStitch.DrawImage(baseImg, posX, posY, Constants.XyzTilePixelSize, Constants.XyzTilePixelSize);
                    }

                    if (result.overlayBytes != null && result.overlayBytes.Length > 0)
                    {
                        using MemoryStream msOverlay = new MemoryStream(result.overlayBytes);
                        using Image overlayImg = Image.FromStream(msOverlay);
                        gStitch.DrawImage(overlayImg, posX, posY, Constants.XyzTilePixelSize, Constants.XyzTilePixelSize);
                    }
                }
            }

            // Map stitched coordinates of EPSG:2180 box corners
            PointF srcTL = new PointF((float)((tlX - minTileX) * Constants.XyzTilePixelSize), (float)((tlY - minTileY) * Constants.XyzTilePixelSize));
            PointF srcTR = new PointF((float)((trX - minTileX) * Constants.XyzTilePixelSize), (float)((trY - minTileY) * Constants.XyzTilePixelSize));
            PointF srcBL = new PointF((float)((blX - minTileX) * Constants.XyzTilePixelSize), (float)((blY - minTileY) * Constants.XyzTilePixelSize));

            using Bitmap outputBitmap = new Bitmap(resolution, resolution);
            using (Graphics gOut = Graphics.FromImage(outputBitmap))
            {
                gOut.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gOut.SmoothingMode = SmoothingMode.HighQuality;
                gOut.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF destRect = new RectangleF(0, 0, resolution, resolution);
                PointF[] srcTriangle = new PointF[] { srcTL, srcTR, srcBL };

                using Matrix matrix = new Matrix(destRect, srcTriangle);
                matrix.Invert();
                gOut.Transform = matrix;
                gOut.DrawImage(stitchedBitmap, 0, 0);
            }

            using MemoryStream outMs = new MemoryStream();
            outputBitmap.Save(outMs, ImageFormat.Jpeg);
            return outMs.ToArray();
        }

        private static (double tileX, double tileY) LatLonToTile(double lat, double lon, int zoom)
        {
            double n = Math.Pow(2, zoom);
            double tileX = (lon + 180.0) / 360.0 * n;
            double latRad = lat * Math.PI / 180.0;
            double tileY = (1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n;
            return (tileX, tileY);
        }
    }
}
