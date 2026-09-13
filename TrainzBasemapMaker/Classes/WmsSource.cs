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
using System.Globalization;

namespace TrainzBasemapMaker.Classes
{
    internal class WmsSource
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Layer { get; set; }
        public bool SupportsTime { get; set; }
        public string Format { get; set; }
        public bool IsXyzTileSource { get; set; }
        public string OverlayUrl { get; set; }

        private static readonly HttpClient _httpClient = new HttpClient();

        public const long TileSize = 500;

        static WmsSource()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "TrainzBasemapMaker/1.0 (https://github.com/Ignacy110/TrainzBasemapMaker)");
            }
        }

        public WmsSource(string name, string url, string layer, bool supportsTime, string format = "image/jpeg", bool isXyzTileSource = false, string overlayUrl = "")
        {
            Name = name;
            BaseUrl = url;
            Layer = layer;
            SupportsTime = supportsTime;
            Format = format;
            IsXyzTileSource = isXyzTileSource;
            OverlayUrl = overlayUrl;
        }

        public override string ToString() => Name;

        public static List<WmsSource> availableMaps = new List<WmsSource>
        {
            new WmsSource("Ortofotomapa",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/StandardResolutionTime?",
                "Raster", true),
            new WmsSource("Ortofotomapa wysoka rozdzielczość",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/HighResolutionTime?",
                "Image", true),
            new WmsSource("Cieniowanie",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WMS/ShadedRelief?",
                "Raster", false),
            new WmsSource("OpenRailwayMap",
                "https://tile.openstreetmap.org/{z}/{x}/{y}.png",
                "", false, "image/png", true,
                "https://{s}.tiles.openrailwaymap.org/standard/{z}/{x}/{y}.png")
        };

        public async Task<byte[]> GetMapImageAsync(string year, double xLeft, double yTop, int resolution, int maxRetries = 3, int delaySeconds = 3)
        {
            if (IsXyzTileSource)
            {
                return await GetXyzTileImageAsync(xLeft, yTop, resolution, maxRetries, delaySeconds);
            }

            double xRight = xLeft + TileSize;
            double yBottom = yTop - TileSize;

            string url = BuildWmsUrl(year, xLeft, yBottom, xRight, yTop, resolution);

            int maxAttempts = Math.Max(1, maxRetries);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    byte[] bytes = await response.Content.ReadAsByteArrayAsync();

                    // WMS servers sometimes return HTTP 200 OK with an XML error message instead of an image
                    if (IsWmsXmlException(bytes))
                    {
                        throw new HttpRequestException("Serwer WMS zwrócił komunikat błędu XML zamiast obrazu.");
                    }

                    return bytes;
                }
                catch (Exception ex)
                {
                    if (attempt == maxAttempts)
                    {
                        throw new Exception($"Pobieranie podkładu nie powiodło się po {maxAttempts} próbach: {ex.Message}", ex);
                    }

                    // Delay for specified time (e.g., 3 seconds) before next attempt
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            throw new Exception("Pobieranie podkładu nie powiodło się.");
        }

        private async Task<byte[]> GetXyzTileImageAsync(double xCenter, double yCenter, int resolution, int maxRetries, int delaySeconds)
        {
            // Bounding box in EPSG:2180 (500m x 500m centered at xCenter, yCenter)
            double minX = xCenter - TileSize / 2.0;
            double maxX = xCenter + TileSize / 2.0;
            double minY = yCenter - TileSize / 2.0;
            double maxY = yCenter + TileSize / 2.0;

            // Convert 4 corners to Lat/Lon
            var (tlLat, tlLon) = GeoHelperEPSG2180.Meters2180ToLatLon(minX, maxY);
            var (trLat, trLon) = GeoHelperEPSG2180.Meters2180ToLatLon(maxX, maxY);
            var (blLat, blLon) = GeoHelperEPSG2180.Meters2180ToLatLon(minX, minY);
            var (brLat, brLon) = GeoHelperEPSG2180.Meters2180ToLatLon(maxX, minY);

            const int zoom = 18;

            var (tlX, tlY) = LatLonToTile(tlLat, tlLon, zoom);
            var (trX, trY) = LatLonToTile(trLat, trLon, zoom);
            var (blX, blY) = LatLonToTile(blLat, blLon, zoom);
            var (brX, brY) = LatLonToTile(brLat, brLon, zoom);

            int minTileX = (int)Math.Floor(Math.Min(Math.Min(tlX, trX), Math.Min(blX, brX)));
            int maxTileX = (int)Math.Floor(Math.Max(Math.Max(tlX, trX), Math.Max(blX, brX)));
            int minTileY = (int)Math.Floor(Math.Min(Math.Min(tlY, trY), Math.Min(blY, brY)));
            int maxTileY = (int)Math.Floor(Math.Max(Math.Max(tlY, trY), Math.Max(blY, brY)));

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
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
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

        private static bool IsWmsXmlException(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 10) return false;

            // Inspect file header for XML declaration or ServiceExceptionReport
            string prefix = System.Text.Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 200));
            return prefix.Contains("<ServiceException") || prefix.Contains("<?xml");
        }

        private string BuildWmsUrl(string year, double xLeft, double yBottom, double xRight, double yTop, int resolution)
        {
            xLeft = xLeft - TileSize / 2;
            yBottom = yBottom + TileSize / 2;
            xRight = xRight - TileSize / 2;
            yTop = yTop + TileSize / 2;

            var culture = CultureInfo.InvariantCulture;

            string url = $"{BaseUrl}" +
                         "SERVICE=WMS&REQUEST=GetMap&VERSION=1.1.1" +
                         $"&LAYERS={Layer}" +
                         "&SRS=EPSG:2180" +
                         $"&BBOX={xLeft.ToString(culture)},{yBottom.ToString(culture)},{xRight.ToString(culture)},{yTop.ToString(culture)}" +
                         $"&WIDTH={resolution}&HEIGHT={resolution}" +
                         $"&FORMAT={Format}" +
                         "&STYLES=";

            if (SupportsTime && !string.IsNullOrEmpty(year))
            {
                url += $"&TIME={year}";
            }

            return url;
        }
    }
}
