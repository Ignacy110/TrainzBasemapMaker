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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    public class TerrainTileInfo
    {
        public int Order { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }

    public class TerrainRouteInfo
    {
        public string RouteName { get; set; } = string.Empty;
        public string KuidPart1 { get; set; } = string.Empty;
        public string KuidPart2 { get; set; } = string.Empty;
        public string Epsg { get; set; } = "EPSG:2180";
        public bool IsRelative { get; set; } = false;
        public double? AnchorElevation { get; set; }
        public long? AnchorX { get; set; }
        public long? AnchorY { get; set; }
        public List<TerrainTileInfo> Tiles { get; set; } = new List<TerrainTileInfo>();

        [JsonIgnore]
        public string FolderPath { get; set; } = string.Empty;

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(KuidPart1) && !string.IsNullOrEmpty(KuidPart2))
                return $"{RouteName} [{KuidPart1}:{KuidPart2}]";
            return RouteName;
        }
    }
}
