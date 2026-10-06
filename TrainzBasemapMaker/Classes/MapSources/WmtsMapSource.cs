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
    /// Map provider implementation for OGC Web Map Tile Service (WMTS) in EPSG:2180 coordinate system.
    /// </summary>
    internal class WmtsMapSource : MapSourceBase
    {
        public string BaseUrl { get; }
        public string Layer { get; }
        public string Format { get; }
        public IReadOnlyList<WmtsMatrixLevel> MatrixLevels { get; }
        public override bool AllowsHighResolution { get; }

        // Top-left origin coordinates for EPSG:2180 TileMatrixSet in Polish Geoportal
        private const double OriginX = 100000.0;
        private const double OriginY = 850000.0;
        private const int WmtsTileSize = Constants.WmtsTilePixelSize;

        public static readonly IReadOnlyList<WmtsMatrixLevel> StandardOrtoLevels = new[]
        {
            new WmtsMatrixLevel("EPSG:2180:16", 236.23559151785716),  // ~0.066 m/px
            new WmtsMatrixLevel("EPSG:2180:15", 472.4711830357143),   // ~0.132 m/px
            new WmtsMatrixLevel("EPSG:2180:14", 944.9423660714286),   // ~0.265 m/px
            new WmtsMatrixLevel("EPSG:2180:13", 1889.8847321428573),  // ~0.529 m/px
            new WmtsMatrixLevel("EPSG:2180:12", 4724.711830357143),   // ~1.323 m/px
            new WmtsMatrixLevel("EPSG:2180:11", 9449.423660714287)    // ~2.646 m/px
        };

        public static readonly IReadOnlyList<WmtsMatrixLevel> TopoAndShadedLevels = new[]
        {
            new WmtsMatrixLevel("EPSG:2180:12", 944.9423660714286),   // ~0.265 m/px
            new WmtsMatrixLevel("EPSG:2180:11", 1889.8847321428573),  // ~0.529 m/px
            new WmtsMatrixLevel("EPSG:2180:10", 4724.711830357143),   // ~1.323 m/px
            new WmtsMatrixLevel("EPSG:2180:9",  9449.423660714287)    // ~2.646 m/px
        };

        public string? FallbackWmsUrl { get; }
        public string? FallbackWmsLayer { get; }

        public WmtsMapSource(string name, string baseUrl, string layer, IEnumerable<WmtsMatrixLevel> levels, bool supportsTime = false, string format = "image/jpeg", bool allowsHighResolution = false, string? fallbackWmsUrl = null, string? fallbackWmsLayer = null)
            : base(name, supportsTime)
        {
            BaseUrl = baseUrl;
            Layer = layer;
            Format = format;
            MatrixLevels = levels.OrderBy(l => l.PixelSize).ToList();
            AllowsHighResolution = allowsHighResolution || MatrixLevels.Any(l => l.PixelSize <= 0.15);
            FallbackWmsUrl = fallbackWmsUrl;
            FallbackWmsLayer = fallbackWmsLayer;
        }

        public override async Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3, CancellationToken cancellationToken = default)
        {
            if (!GeoHelperEPSG2180.IsWithin2180Bounds(xCenter, yCenter))
            {
                var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(xCenter, yCenter);
                if (GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon))
                {
                    var (tx, ty) = GeoHelperEPSG2180.LatLonToMeters2180(lat, lon);
                    xCenter = tx;
                    yCenter = ty;
                }
                else
                {
                    throw new InvalidOperationException("Wybrany obszar znajduje się poza granicami Polski. Usługi Geoportalu obejmują wyłącznie terytorium Polski. Aby pobrać podkład dla tego obszaru, wybierz OpenStreetMap lub OpenRailwayMap.");
                }
            }

            WmtsMatrixLevel selectedLevel = GetOptimalLevel(resolution);

            double pixelSize = selectedLevel.PixelSize;
            double tileSpan = WmtsTileSize * pixelSize;

            // Bounding box in EPSG:2180 (500m x 500m centered at xCenter, yCenter)
            double minX = xCenter - TileSize / 2.0;
            double maxX = xCenter + TileSize / 2.0;
            double minY = yCenter - TileSize / 2.0;
            double maxY = yCenter + TileSize / 2.0;

            int minCol = (int)Math.Floor((minX - OriginX) / tileSpan);
            int maxCol = (int)Math.Floor((maxX - OriginX) / tileSpan);
            int minRow = (int)Math.Floor((OriginY - maxY) / tileSpan);
            int maxRow = (int)Math.Floor((OriginY - minY) / tileSpan);

            int tilesNumX = maxCol - minCol + 1;
            int tilesNumY = maxRow - minRow + 1;

            var downloadTasks = new List<Task<(int col, int row, byte[]? bytes)>>();

            for (int r = minRow; r <= maxRow; r++)
            {
                for (int c = minCol; c <= maxCol; c++)
                {
                    int currentC = c;
                    int currentR = r;

                    downloadTasks.Add(Task.Run(async () =>
                    {
                        byte[]? tileBytes = await FetchTileWithFallbackAsync(currentC, currentR, selectedLevel, tileSpan, maxRetries, delaySeconds, cancellationToken).ConfigureAwait(false);
                        return (currentC, currentR, tileBytes);
                    }, cancellationToken));
                }
            }

            // ConfigureAwait(false): the CPU-heavy stitching/scaling below must not run on the UI thread.
            var results = await Task.WhenAll(downloadTasks).ConfigureAwait(false);

            if (results.All(r => r.bytes == null || r.bytes.Length == 0))
            {
                throw new HttpRequestException("Serwer WMTS nie zwrócił żadnych kafelków dla wybranego obszaru.");
            }

            int stitchedWidth = tilesNumX * WmtsTileSize;
            int stitchedHeight = tilesNumY * WmtsTileSize;

            using Bitmap stitchedBitmap = ImageHelpers.CreateRgbBitmap(stitchedWidth, stitchedHeight);
            using (Graphics gStitch = Graphics.FromImage(stitchedBitmap))
            {
                gStitch.Clear(Color.FromArgb(92, 108, 68)); // Muted natural terrain green instead of white
                foreach (var result in results)
                {
                    if (result.bytes != null && result.bytes.Length > 0)
                    {
                        using MemoryStream ms = new MemoryStream(result.bytes);
                        using Image tileImg = Image.FromStream(ms);
                        int posX = (result.col - minCol) * WmtsTileSize;
                        int posY = (result.row - minRow) * WmtsTileSize;
                        gStitch.DrawImage(tileImg, posX, posY, WmtsTileSize, WmtsTileSize);
                    }
                }
            }

            // Calculate precise rectangular sub-pixel crop bounds (axis-aligned in EPSG:2180)
            float cropX = (float)((minX - (OriginX + minCol * tileSpan)) / pixelSize);
            float cropY = (float)(((OriginY - minRow * tileSpan) - maxY) / pixelSize);
            float cropWidth = (float)((maxX - minX) / pixelSize);
            float cropHeight = (float)((maxY - minY) / pixelSize);

            using Bitmap outputBitmap = ImageHelpers.CreateRgbBitmap(resolution, resolution);
            using (Graphics gOut = Graphics.FromImage(outputBitmap))
            {
                gOut.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gOut.SmoothingMode = SmoothingMode.HighQuality;
                gOut.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF destRect = new RectangleF(0, 0, resolution, resolution);
                RectangleF srcRect = new RectangleF(cropX, cropY, cropWidth, cropHeight);

                gOut.DrawImage(stitchedBitmap, destRect, srcRect, GraphicsUnit.Pixel);
            }

            return ImageHelpers.EncodeJpeg(outputBitmap);
        }

        private WmtsMatrixLevel GetOptimalLevel(int resolution)
        {
            // Target ground resolution (in meters per pixel) for the 500m area exported at requested resolution
            double targetPixelSize = TileSize / (double)resolution;

            // Pick the most efficient level that satisfies the target ground resolution (with a 20% tolerance margin)
            return MatrixLevels
                .Where(l => l.PixelSize <= targetPixelSize * 1.2)
                .OrderByDescending(l => l.PixelSize)
                .FirstOrDefault()
                ?? MatrixLevels.OrderBy(l => l.PixelSize).First();
        }

        private async Task<byte[]?> FetchTileWithFallbackAsync(
            int col, int row, WmtsMatrixLevel selectedLevel, double tileSpan, int maxRetries, int delaySeconds, CancellationToken cancellationToken)
        {
            string tileUrl = $"{BaseUrl}?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0" +
                             $"&LAYER={Uri.EscapeDataString(Layer)}" +
                             "&STYLE=default" +
                             $"&FORMAT={Format}" +
                             "&TILEMATRIXSET=EPSG:2180" +
                             $"&TILEMATRIX={selectedLevel.Identifier}" +
                             $"&TILEROW={row}&TILECOL={col}";

            byte[]? tileBytes = await FetchTileBytesWithRetryAsync(tileUrl, maxRetries, delaySeconds, cancellationToken);
            if (tileBytes != null && tileBytes.Length > 0)
            {
                return tileBytes;
            }

            double tileMinX = OriginX + col * tileSpan;
            double tileMaxX = tileMinX + tileSpan;
            double tileMaxY = OriginY - row * tileSpan;
            double tileMinY = tileMaxY - tileSpan;

            // Fallback 1: WMS dynamic rendering (if fallback URL is configured)
            if (!string.IsNullOrEmpty(FallbackWmsUrl) && !string.IsNullOrEmpty(FallbackWmsLayer))
            {
                string sep = FallbackWmsUrl.Contains("?") ? (FallbackWmsUrl.EndsWith("?") || FallbackWmsUrl.EndsWith("&") ? "" : "&") : "?";
                string wmsUrl = $"{FallbackWmsUrl}{sep}SERVICE=WMS&REQUEST=GetMap&VERSION=1.3.0" +
                                $"&LAYERS={Uri.EscapeDataString(FallbackWmsLayer)}&STYLES=" +
                                $"&CRS=EPSG:2180&BBOX={tileMinY.ToString(System.Globalization.CultureInfo.InvariantCulture)},{tileMinX.ToString(System.Globalization.CultureInfo.InvariantCulture)},{tileMaxY.ToString(System.Globalization.CultureInfo.InvariantCulture)},{tileMaxX.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                                $"&WIDTH={WmtsTileSize}&HEIGHT={WmtsTileSize}&FORMAT={Format}";

                byte[]? wmsBytes = await FetchTileBytesWithRetryAsync(wmsUrl, 2, 1, cancellationToken);
                if (wmsBytes != null && wmsBytes.Length > 0)
                {
                    return wmsBytes;
                }
            }

            // Fallback 2: Coarser WMTS levels in MatrixLevels
            int levelIdx = -1;
            for (int li = 0; li < MatrixLevels.Count; li++)
            {
                if (MatrixLevels[li].Identifier == selectedLevel.Identifier) { levelIdx = li; break; }
            }
            if (levelIdx >= 0)
            {
                double tileCenterX = (tileMinX + tileMaxX) / 2.0;
                double tileCenterY = (tileMinY + tileMaxY) / 2.0;

                for (int nextIdx = levelIdx + 1; nextIdx < MatrixLevels.Count; nextIdx++)
                {
                    var fbLevel = MatrixLevels[nextIdx];
                    double fbTileSpan = WmtsTileSize * fbLevel.PixelSize;
                    int fbCol = (int)Math.Floor((tileCenterX - OriginX) / fbTileSpan);
                    int fbRow = (int)Math.Floor((OriginY - tileCenterY) / fbTileSpan);

                    string fbUrl = $"{BaseUrl}?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0" +
                                   $"&LAYER={Uri.EscapeDataString(Layer)}" +
                                   "&STYLE=default" +
                                   $"&FORMAT={Format}" +
                                   "&TILEMATRIXSET=EPSG:2180" +
                                   $"&TILEMATRIX={fbLevel.Identifier}" +
                                   $"&TILEROW={fbRow}&TILECOL={fbCol}";

                    byte[]? fbBytes = await FetchTileBytesWithRetryAsync(fbUrl, 2, 1, cancellationToken);
                    if (fbBytes != null && fbBytes.Length > 0)
                    {
                        try
                        {
                            using MemoryStream fbMs = new MemoryStream(fbBytes);
                            using Image fbImg = Image.FromStream(fbMs);

                            double fbMinX = OriginX + fbCol * fbTileSpan;
                            double fbMaxY = OriginY - fbRow * fbTileSpan;

                            float cropX = (float)((tileMinX - fbMinX) / fbLevel.PixelSize);
                            float cropY = (float)((fbMaxY - tileMaxY) / fbLevel.PixelSize);
                            float cropW = (float)(tileSpan / fbLevel.PixelSize);
                            float cropH = (float)(tileSpan / fbLevel.PixelSize);

                            using Bitmap subTileBmp = ImageHelpers.CreateRgbBitmap(WmtsTileSize, WmtsTileSize);
                            using (Graphics gSub = Graphics.FromImage(subTileBmp))
                            {
                                gSub.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                gSub.SmoothingMode = SmoothingMode.HighQuality;
                                gSub.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                gSub.DrawImage(fbImg, new RectangleF(0, 0, WmtsTileSize, WmtsTileSize), new RectangleF(cropX, cropY, cropW, cropH), GraphicsUnit.Pixel);
                            }

                            return ImageHelpers.EncodeJpeg(subTileBmp);
                        }
                        catch
                        {
                            // Try next coarser level
                        }
                    }
                }
            }

            return null;
        }
    }
}
