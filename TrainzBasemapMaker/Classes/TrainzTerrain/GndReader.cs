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

using System;
using System.Collections.Generic;
using System.IO;

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    internal class GndReader
    {
        private const int DATASIZE = 76;

        public static List<MapGridPart> ReadGndFile(byte[] gndData)
        {
            var blocks = new List<MapGridPart>();
            if (gndData == null || gndData.Length < 8) return blocks;

            using (var ms = new MemoryStream(gndData))
            using (var br = new BinaryReader(ms))
            {
                byte b0 = br.ReadByte();
                byte b1 = br.ReadByte();
                byte b2 = br.ReadByte();
                byte b3 = br.ReadByte();

                if (b0 != 'G' || b1 != 'N' || b2 != 'D' || b3 != 0x0b)
                {
                    throw new InvalidDataException("Nieprawidlowy naglowek pliku GND!");
                }

                int blockCount = br.ReadInt32();
                var blockEntries = new List<(int SegmentX, int SegmentY, int TileOffset, int ImageOffset)>(blockCount);

                for (int i = 0; i < blockCount; i++)
                {
                    int segX = br.ReadInt32();
                    int segY = br.ReadInt32();
                    int tileOffset = br.ReadInt32();
                    int imgOffset = br.ReadInt32();
                    blockEntries.Add((segX, segY, tileOffset, imgOffset));
                }

                foreach (var entry in blockEntries)
                {
                    int requiredBytes = 4 + DATASIZE * DATASIZE * 10;
                    if (entry.TileOffset + requiredBytes > gndData.Length)
                    {
                        continue;
                    }

                    ms.Seek(entry.TileOffset, SeekOrigin.Begin);
                    int zeroPrefix = br.ReadInt32();

                    float[,] heights = new float[DATASIZE, DATASIZE];
                    for (int easting = 0; easting < DATASIZE; easting++)
                    {
                        for (int northing = 0; northing < DATASIZE; northing++)
                        {
                            br.ReadByte(); // tag 0xfd
                            br.ReadByte(); // textureId
                            br.ReadByte(); // rot
                            float h = br.ReadSingle(); // height
                            br.ReadByte(); // 0x00
                            br.ReadByte(); // 0x00
                            br.ReadByte(); // 0x7f

                            heights[easting, northing] = h;
                        }
                    }

                    var part = new MapGridPart(entry.SegmentX, entry.SegmentY, heights);
                    blocks.Add(part);
                }
            }

            return blocks;
        }
    }
}
