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

namespace TrainzBasemapMaker.Classes
{
    /// <summary>
    /// Registry of all available map sources in the application.
    /// </summary>
    internal static class MapSources
    {
        /// <summary>
        /// List of configured map providers available for selection in the UI.
        /// </summary>
        public static readonly List<IMapSource> AvailableMaps = new List<IMapSource>
        {
            new WmtsMapSource("Ortofotomapa WMTS",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMTS/StandardResolution",
                "ORTOFOTOMAPA", WmtsMapSource.StandardOrtoLevels, supportsTime: false, format: "image/jpeg"),
            new WmsMapSource("Ortofotomapa WMS, wybór roku",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/StandardResolutionTime?",
                "Raster", true, "image/jpeg", allowsHighResolution: true),
            new WmtsMapSource("Ortofotomapa wysoka rozdzielczość WMTS",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMTS/HighResolution",
                "ORTOFOTOMAPA", WmtsMapSource.StandardOrtoLevels, supportsTime: false, format: "image/jpeg"),
            new WmsMapSource("Ortofotomapa wysoka rozdzielczość WMS, wybór roku",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/HighResolutionTime?",
                "Image", true, "image/jpeg", allowsHighResolution: true),
            new WmtsMapSource("Cieniowanie WMTS",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WMTS/ShadedRelief",
                "ISOK_Cien", WmtsMapSource.TopoAndShadedLevels, supportsTime: false, format: "image/jpeg"),
            new WmsMapSource("Cieniowanie WMS",
                "https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WMS/ShadedRelief?",
                "Raster", false, "image/jpeg", allowsHighResolution: false),
            new WmtsMapSource("Mapa topograficzna WMTS",
                "https://mapy.geoportal.gov.pl/wss/service/WMTS/guest/wmts/TOPO",
                "MAPA TOPOGRAFICZNA", WmtsMapSource.TopoAndShadedLevels, supportsTime: false, format: "image/jpeg"),
            new XyzTileMapSource("OpenStreetMap",
                "https://tile.openstreetmap.org/{z}/{x}/{y}.png"),
            new XyzTileMapSource("OpenRailwayMap",
                "https://tile.openstreetmap.org/{z}/{x}/{y}.png",
                "https://{s}.tiles.openrailwaymap.org/standard/{z}/{x}/{y}.png")
        };
    }
}
