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

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    /// <summary>
    /// Represents a layer definition in Trainz mapfile.lyr.
    /// </summary>
    public class TrainzLayer
    {
        public byte LayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Flags { get; set; } = 0x01; // Default active/visible

        public TrainzLayer() { }

        public TrainzLayer(byte layerId, string name, byte flags = 0x01)
        {
            LayerId = layerId;
            Name = name;
            Flags = flags;
        }
    }
}
