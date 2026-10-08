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
    public class ObsObject
    {
        public int KuidPart1 { get; set; }
        public int KuidPart2 { get; set; }
        public int KuidVersion { get; set; } = 0;
        public byte LayerId { get; set; } = 0; // 0-based in mapfile.obs (0 = route-layer, 1 = first custom layer...)
        public short SegX { get; set; } = 0;
        public short SegY { get; set; } = 0;
        public float X { get; set; } = 360f;
        public float Y { get; set; } = 360f;
        public float Z { get; set; }
        public float RotX { get; set; } = 0f;
        public float RotY { get; set; } = 0f;
        public float RotZ { get; set; } = 0f;
    }
}
