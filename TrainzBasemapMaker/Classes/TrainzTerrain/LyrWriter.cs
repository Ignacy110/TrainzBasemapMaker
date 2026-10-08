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
    /// Binary writer for Trainz route layers definition file (mapfile.lyr).
    /// Verified binary format:
    /// - 4 bytes (int32): Format Version (always 1)
    /// - 1 byte (uint8): Number of layers N
    /// - For each layer:
    ///   - 1 byte (uint8): Layer ID (0 for route-layer, 1 for second layer, etc.)
    ///   - 4 bytes (int32): String length in bytes including trailing null terminator (name.Length + 1)
    ///   - ASCII string bytes
    ///   - 1 byte (0x00): Null terminator
    ///   - 1 byte (uint8): Layer flags / visibility (0x01 = visible)
    /// </summary>
    public class LyrWriter
    {
        public byte[] CreateLyrFile(IEnumerable<TrainzLayer>? layers)
        {
            var layerList = layers?.ToList() ?? new List<TrainzLayer>();
            if (layerList.Count == 0)
            {
                layerList.Add(new TrainzLayer(0, "route-layer", 0x01));
            }

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Encoding.ASCII))
            {
                bw.Write((int)1);                  // Format Version = 1 (int32)
                bw.Write((byte)layerList.Count);   // Layer count (byte)

                for (int i = 0; i < layerList.Count; i++)
                {
                    var layer = layerList[i];
                    bw.Write(layer.LayerId);       // Layer ID (0-based byte)

                    // Ensure clean ASCII name (strip Polish diacritics)
                    string cleanName = FormHelpers.RemoveDiacritics(layer.Name);
                    byte[] nameBytes = Encoding.ASCII.GetBytes(cleanName);

                    bw.Write(nameBytes.Length + 1); // Length including trailing \0 (int32)
                    bw.Write(nameBytes);
                    bw.Write((byte)0);              // Null terminator
                    bw.Write(layer.Flags);          // Flags (byte, 0x01)
                }

                return ms.ToArray();
            }
        }
    }
}
