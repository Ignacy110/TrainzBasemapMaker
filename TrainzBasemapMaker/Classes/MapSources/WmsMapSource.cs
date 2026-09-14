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

using System.Globalization;

namespace TrainzBasemapMaker.Classes
{
    /// <summary>
    /// Map provider implementation for OGC Web Map Service (WMS) endpoints.
    /// </summary>
    internal class WmsMapSource : MapSourceBase
    {
        public string BaseUrl { get; }
        public string Layer { get; }
        public string Format { get; }
        public override bool AllowsHighResolution { get; }

        public WmsMapSource(string name, string url, string layer, bool supportsTime, string format = "image/jpeg", bool allowsHighResolution = true)
            : base(name, supportsTime)
        {
            BaseUrl = url;
            Layer = layer;
            Format = format;
            AllowsHighResolution = allowsHighResolution;
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

            double xLeft = xCenter - TileSize / 2.0;
            double xRight = xCenter + TileSize / 2.0;
            double yBottom = yCenter - TileSize / 2.0;
            double yTop = yCenter + TileSize / 2.0;

            string url = BuildWmsUrl(year, xLeft, yBottom, xRight, yTop, resolution);

            int maxAttempts = Math.Max(1, maxRetries);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(url);
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

                    // Delay for specified time before next attempt
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            throw new Exception("Pobieranie podkładu nie powiodło się.");
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
