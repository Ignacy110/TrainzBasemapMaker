
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

namespace TrainzBasemapMaker
{
    internal class WmsSource
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Layer { get; set; }
        public bool SupportsTime { get; set; }
        public string Format { get; set; }

        private static readonly HttpClient _httpClient = new HttpClient();

        public const long TileSize = 500;

        public WmsSource(string name, string url, string layer, bool supportsTime, string format = "image/jpeg")
        {
            Name = name;
            BaseUrl = url;
            Layer = layer;
            SupportsTime = supportsTime;
            Format = format;
        }

        public override string ToString() => Name;

        public static List<WmsSource> availableMaps = new List<WmsSource>
        {
            new WmsSource("Ortofotomapa",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/StandardResolutionTime?",
                "Raster", true),
            new WmsSource("Cieniowanie",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WMS/ShadedRelief?",
                "Raster", false)
        };

        public async Task<byte[]> GetMapImageAsync(string year, double xLeft, double yTop, int resolution)
        {
            double xRight = xLeft + TileSize;
            double yBottom = yTop - TileSize;

            string url = BuildWmsUrl(year, xLeft, yBottom, xRight, yTop, resolution);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        private string BuildWmsUrl(string year, double xLeft, double yBottom, double xRight, double yTop, int resolution)
        {
            xLeft = xLeft - (TileSize / 2);
            yBottom = yBottom + (TileSize / 2);
            xRight = xRight - (TileSize / 2);
            yTop = yTop + (TileSize / 2);

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
