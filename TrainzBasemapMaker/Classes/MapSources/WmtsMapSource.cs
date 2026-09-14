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
    /// Represents a specific TileMatrix level definition in a WMTS TileMatrixSet.
    /// </summary>
    internal class WmtsMatrixLevel
    {
        public string Identifier { get; }
        public double ScaleDenominator { get; }
        public double PixelSize => ScaleDenominator * 0.00028;

        public WmtsMatrixLevel(string identifier, double scaleDenominator)
        {
            Identifier = identifier;
            ScaleDenominator = scaleDenominator;
        }

        public override string ToString() => $"{Identifier} (Scale: {ScaleDenominator:F2}, {PixelSize:F4}m/px)";
    }

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
        private const int WmtsTileSize = 512;

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

        public WmtsMapSource(string name, string baseUrl, string layer, IEnumerable<WmtsMatrixLevel> levels, bool supportsTime = false, string format = "image/jpeg", bool allowsHighResolution = false)
            : base(name, supportsTime)
        {
            BaseUrl = baseUrl;
            Layer = layer;
            Format = format;
            MatrixLevels = levels.OrderBy(l => l.PixelSize).ToList();
            AllowsHighResolution = allowsHighResolution || MatrixLevels.Any(l => l.PixelSize <= 0.15);
        }

        public override async Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3)
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
                    string tileUrl = $"{BaseUrl}?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0" +
                                     $"&LAYER={Uri.EscapeDataString(Layer)}" +
                                     "&STYLE=default" +
                                     $"&FORMAT={Format}" +
                                     "&TILEMATRIXSET=EPSG:2180" +
                                     $"&TILEMATRIX={selectedLevel.Identifier}" +
                                     $"&TILEROW={currentR}&TILECOL={currentC}";

                    downloadTasks.Add(Task.Run(async () =>
                    {
                        byte[]? tileBytes = await FetchTileBytesWithRetryAsync(tileUrl, maxRetries, delaySeconds);
                        return (currentC, currentR, tileBytes);
                    }));
                }
            }

            var results = await Task.WhenAll(downloadTasks);

            if (results.All(r => r.bytes == null || r.bytes.Length == 0))
            {
                throw new HttpRequestException("Serwer WMTS nie zwrócił żadnych kafelków dla wybranego obszaru.");
            }

            int stitchedWidth = tilesNumX * WmtsTileSize;
            int stitchedHeight = tilesNumY * WmtsTileSize;

            using Bitmap stitchedBitmap = new Bitmap(stitchedWidth, stitchedHeight);
            using (Graphics gStitch = Graphics.FromImage(stitchedBitmap))
            {
                gStitch.Clear(Color.White);
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

            using Bitmap outputBitmap = new Bitmap(resolution, resolution);
            using (Graphics gOut = Graphics.FromImage(outputBitmap))
            {
                gOut.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gOut.SmoothingMode = SmoothingMode.HighQuality;
                gOut.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF destRect = new RectangleF(0, 0, resolution, resolution);
                RectangleF srcRect = new RectangleF(cropX, cropY, cropWidth, cropHeight);

                gOut.DrawImage(stitchedBitmap, destRect, srcRect, GraphicsUnit.Pixel);
            }

            using MemoryStream outMs = new MemoryStream();
            outputBitmap.Save(outMs, ImageFormat.Jpeg);
            return outMs.ToArray();
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

        private static async Task<byte[]?> FetchTileBytesWithRetryAsync(string url, int maxRetries, int delaySeconds)
        {
            int maxAttempts = Math.Max(1, maxRetries);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                }
                catch
                {
                    // Retry on transient network errors
                }

                if (attempt < maxAttempts)
                {
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            return null;
        }
    }
}
