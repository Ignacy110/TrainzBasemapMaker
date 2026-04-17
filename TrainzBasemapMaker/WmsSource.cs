
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
        public string Name { get; set; }        // Nazwa wyświetlana w UI
        public string BaseUrl { get; set; }     // Główny link do serwera
        public string Layer { get; set; }        // Nazwa warstwy (np. "Raster")
        public bool SupportsTime { get; set; }  // Czy ta mapa wspiera wybór roku?
        public string Format { get; set; }      // jpeg czy png

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
            "Raster", false),
        };

        public string BuildWmsUrl(string year, double xLeft, double yBottom, double xRight, double yTop, int resolution)
        {
            var culture = CultureInfo.InvariantCulture;

            // Używamy "this", aby pobrać dane z bieżącego obiektu
            string url = $"{this.BaseUrl}" +
                         "SERVICE=WMS&REQUEST=GetMap&VERSION=1.1.1" +
                         $"&LAYERS={this.Layer}" +
                         "&SRS=EPSG:2180" +
                         $"&BBOX={xLeft.ToString(culture)},{yBottom.ToString(culture)},{xRight.ToString(culture)},{yTop.ToString(culture)}" +
                         $"&WIDTH={resolution}&HEIGHT={resolution}" +
                         $"&FORMAT={this.Format}" +
                         "&STYLES=";

            // Sprawdzamy SupportsTime dla tego konkretnego obiektu
            if (this.SupportsTime && !string.IsNullOrEmpty(year))
            {
                url += $"&TIME={year}";
            }

            return url;
        }
    }
}
