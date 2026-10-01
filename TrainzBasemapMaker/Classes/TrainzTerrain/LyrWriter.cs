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
using System.IO;
using System.Linq;
using System.Text;

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    /// <summary>
    /// Represents a layer definition in Trainz mapfile.lyr.
    /// </summary>
    public class TrainzLayer
    {
        public short LayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Flags { get; set; } = 0x01; // Default active/visible

        public TrainzLayer() { }

        public TrainzLayer(short layerId, string name, byte flags = 0x01)
        {
            LayerId = layerId;
            Name = name;
            Flags = flags;
        }
    }

    /// <summary>
    /// Binary writer for Trainz route layers definition file (mapfile.lyr).
    /// Binary format:
    /// - 4 bytes (int32): Number of layers N
    /// - For each layer:
    ///   - 2 bytes (int16): Layer ID
    ///   - 4 bytes (int32): String length in bytes including trailing null terminator (name.Length + 1)
    ///   - ASCII / UTF-8 string bytes
    ///   - 1 byte (0x00): Null terminator
    ///   - 1 byte: Layer flags / visibility (0x01)
    /// </summary>
    public class LyrWriter
    {
        public byte[] CreateLyrFile(IEnumerable<TrainzLayer>? layers)
        {
            var layerList = layers?.ToList() ?? new List<TrainzLayer>();
            if (layerList.Count == 0)
            {
                layerList.Add(new TrainzLayer(1, "route-layer", 0x01));
            }

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Encoding.UTF8))
            {
                bw.Write(layerList.Count);

                foreach (var layer in layerList)
                {
                    bw.Write(layer.LayerId);
                    byte[] nameBytes = Encoding.UTF8.GetBytes(layer.Name);
                    bw.Write(nameBytes.Length + 1);
                    bw.Write(nameBytes);
                    bw.Write((byte)0);
                    bw.Write(layer.Flags);
                }

                return ms.ToArray();
            }
        }
    }
}
