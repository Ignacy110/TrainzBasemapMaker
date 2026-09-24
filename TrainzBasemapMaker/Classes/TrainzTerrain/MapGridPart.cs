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
using System.IO;

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    internal class MapGridPart
    {
        private const int DATASIZE = 76;
        private const byte ROT_SETTING = 0x00;

        private float[,] _heights = new float[DATASIZE, DATASIZE];
        
        public int SegmentX { get; }
        public int SegmentY { get; }
        public float[,] Heights => _heights;

        public MapGridPart(int segmentX, int segmentY)
        {
            SegmentX = segmentX;
            SegmentY = segmentY;
        }

        public MapGridPart(int segmentX, int segmentY, float[,] heights) : this(segmentX, segmentY)
        {
            SetHeights(heights);
        }

        public void SetHeights(float[,] heights)
        {
            if (heights.GetLength(0) != DATASIZE || heights.GetLength(1) != DATASIZE)
                throw new ArgumentException($"Zly wymiar siatki wysokosci: {heights.GetLength(0)}x{heights.GetLength(1)}");

            _heights = heights;
        }

        public void WriteToFile(BinaryWriter writer, byte defaultTextureId)
        {
            // 4 bajty zera na poczatku
            writer.Write((int)0);

            // Zapis w ukladzie: ZACHOD -> WSCHOD (kolumny/Easting), a wewnatrz: POLNOC -> POLUDNIE (wiersze/Northing)
            for (int easting = 0; easting < DATASIZE; easting++)
            {
                for (int northing = 0; northing < DATASIZE; northing++)
                {
                    float h = _heights[easting, northing];
                    // Zapis pojedynczego kafelka wysokosciowego:
                    writer.Write((byte)0xfd);
                    writer.Write(defaultTextureId);
                    writer.Write(ROT_SETTING);
                    writer.Write(h);
                    writer.Write((byte)0x00);
                    writer.Write((byte)0x00);
                    writer.Write((byte)0x7f);
                }
            }
        }

        public byte[] GetTileData(byte defaultTextureId)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                WriteToFile(bw, defaultTextureId);
                return ms.ToArray();
            }
        }

        public byte[] GetPreviewImageData()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // TGA Header (18 bytes) 128x128 32-bit
                byte[] header = {
                    0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x80, 0x00, 0x80, 0x00, 0x20, 0x08
                };
                bw.Write(header);

                float minH = float.MaxValue;
                float maxH = float.MinValue;
                for (int x = 0; x < DATASIZE; x++)
                {
                    for (int y = 0; y < DATASIZE; y++)
                    {
                        if (_heights[x, y] < minH) minH = _heights[x, y];
                        if (_heights[x, y] > maxH) maxH = _heights[x, y];
                    }
                }
                if (Math.Abs(maxH - minH) < 0.001f) maxH = minH + 1f;

                // TGA 128x128: py = 0 (South) to py = 127 (North)
                for (int py = 0; py < 128; py++)
                {
                    float gridY = (127 - py) * (75.0f / 127.0f);
                    int y0 = Math.Clamp((int)gridY, 0, 74);
                    int y1 = y0 + 1;
                    float fy = gridY - y0;

                    for (int px = 0; px < 128; px++)
                    {
                        float gridX = px * (75.0f / 127.0f);
                        int x0 = Math.Clamp((int)gridX, 0, 74);
                        int x1 = x0 + 1;
                        float fx = gridX - x0;

                        // Interpolacja dwuliniowa wysokosci
                        float h = (1 - fx) * (1 - fy) * _heights[x0, y0] +
                                  fx * (1 - fy) * _heights[x1, y0] +
                                  (1 - fx) * fy * _heights[x0, y1] +
                                  fx * fy * _heights[x1, y1];

                        // Cieniowanie zbocza (swiatlo z polnocnego zachodu)
                        float dx = (_heights[x1, y0] - _heights[x0, y0]);
                        float dy = (_heights[x0, y0] - _heights[x0, y1]);
                        float shade = Math.Clamp(0.85f + (dx - dy) * 0.02f, 0.5f, 1.3f);

                        // Kolorowanie terenu (zielen -> braz -> skala)
                        float normH = Math.Clamp((h - minH) / (maxH - minH), 0f, 1f);
                        byte r = (byte)Math.Clamp((65 + normH * 110) * shade, 0, 255);
                        byte g = (byte)Math.Clamp((120 + normH * 60) * shade, 0, 255);
                        byte b = (byte)Math.Clamp((60 + normH * 50) * shade, 0, 255);

                        // TGA 32-bit: B, G, R, A
                        bw.Write(b);
                        bw.Write(g);
                        bw.Write(r);
                        bw.Write((byte)255); // Alpha
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
