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
    /// Metadata model saved as group_info.json inside each basemap group folder.
    /// Stores the reference anchor and generation settings for accurate grid alignment.
    /// </summary>
    public class BasemapGroupInfo
    {
        public string? GroupName { get; set; }
        public string? Designation { get; set; }
        public string? Epsg { get; set; } // "EPSG:2180" or "EPSG:3857"
        public long? AnchorX { get; set; }
        public long? AnchorY { get; set; }
        public double? AnchorCosLat { get; set; }
        public string? MapSource { get; set; }
        public int? Resolution { get; set; }
        public string? Year { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
