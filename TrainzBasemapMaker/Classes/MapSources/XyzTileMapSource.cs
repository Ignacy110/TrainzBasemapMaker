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

        public override async Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3)
        {
            // Convert center coordinates (EPSG:2180 or EPSG:3857) to Lat/Lon
            var (centerLat, centerLon) = GeoHelperEPSG2180.MetersToLatLon(xCenter, yCenter);

            // Compute 500m x 500m geographic footprint
            double halfSize = TileSize / 2.0;
            double latOffset = halfSize / 111320.0;
            double lonOffset = halfSize / (111320.0 * Math.Cos(centerLat * Math.PI / 180.0));

            double minLat = centerLat - latOffset;
            double maxLat = centerLat + latOffset;
            double minLon = centerLon - lonOffset;
            double maxLon = centerLon + lonOffset;

            const int zoom = 18;

            var (tlX, tlY) = LatLonToTile(maxLat, minLon, zoom);
            var (trX, trY) = LatLonToTile(maxLat, maxLon, zoom);
            var (blX, blY) = LatLonToTile(minLat, minLon, zoom);
            var (brX, brY) = LatLonToTile(minLat, maxLon, zoom);

            int minTileX = (int)Math.Floor(Math.Min(tlX, blX));
            int maxTileX = (int)Math.Floor(Math.Max(trX, brX));
            int minTileY = (int)Math.Floor(Math.Min(tlY, trY));
            int maxTileY = (int)Math.Floor(Math.Max(blY, brY));

            int tilesNumX = maxTileX - minTileX + 1;
            int tilesNumY = maxTileY - minTileY + 1;

            var downloadTasks = new List<Task<(int tx, int ty, byte[] baseBytes, byte[]? overlayBytes)>>();

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
                        byte[] baseBytes = await FetchTileBytesWithRetryAsync(baseUrlFormatted, maxRetries, delaySeconds);
                        byte[]? overlayBytes = null;
                        if (!string.IsNullOrEmpty(overlayUrlFormatted))
                        {
                            overlayBytes = await FetchTileBytesWithRetryAsync(overlayUrlFormatted, maxRetries, delaySeconds);
                        }
                        return (currentTx, currentTy, baseBytes, overlayBytes);
                    }));
                }
            }

            var results = await Task.WhenAll(downloadTasks);

            int stitchedWidth = tilesNumX * 256;
            int stitchedHeight = tilesNumY * 256;

            using Bitmap stitchedBitmap = new Bitmap(stitchedWidth, stitchedHeight);
            using (Graphics gStitch = Graphics.FromImage(stitchedBitmap))
            {
                gStitch.Clear(Color.White);
                foreach (var result in results)
                {
                    int posX = (result.tx - minTileX) * 256;
                    int posY = (result.ty - minTileY) * 256;

                    if (result.baseBytes != null && result.baseBytes.Length > 0)
                    {
                        using MemoryStream msBase = new MemoryStream(result.baseBytes);
                        using Image baseImg = Image.FromStream(msBase);
                        gStitch.DrawImage(baseImg, posX, posY, 256, 256);
                    }

                    if (result.overlayBytes != null && result.overlayBytes.Length > 0)
                    {
                        using MemoryStream msOverlay = new MemoryStream(result.overlayBytes);
                        using Image overlayImg = Image.FromStream(msOverlay);
                        gStitch.DrawImage(overlayImg, posX, posY, 256, 256);
                    }
                }
            }

            // Map stitched coordinates of EPSG:2180 box corners
            PointF srcTL = new PointF((float)((tlX - minTileX) * 256.0), (float)((tlY - minTileY) * 256.0));
            PointF srcTR = new PointF((float)((trX - minTileX) * 256.0), (float)((trY - minTileY) * 256.0));
            PointF srcBL = new PointF((float)((blX - minTileX) * 256.0), (float)((blY - minTileY) * 256.0));

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

        private static async Task<byte[]> FetchTileBytesWithRetryAsync(string url, int maxRetries, int delaySeconds)
        {
            int maxAttempts = Math.Max(1, maxRetries);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (Exception ex)
                {
                    if (attempt == maxAttempts)
                    {
                        throw new Exception($"Pobieranie kafelka XYZ nie powiodło się po {maxAttempts} próbach ({url}): {ex.Message}", ex);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            throw new Exception("Pobieranie kafelka XYZ nie powiodło się.");
        }
    }
}
