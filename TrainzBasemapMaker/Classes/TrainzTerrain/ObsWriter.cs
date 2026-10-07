using System.Collections.Generic;
using System.IO;
using System.Text;

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

    public class ObsWriter
    {
        public byte[] CreateObsFile(List<ObsObject> objects)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Header
                bw.Write(9);
                bw.Write(5);
                bw.Write(9);

                if (objects == null || objects.Count == 0)
                {
                    bw.Write(-1); // No objects
                }
                else
                {
                    bw.Write(0); // Start index

                    for (int i = 0; i < objects.Count; i++)
                    {
                        var obj = objects[i];
                        bw.Write(new byte[] { 0x4A, 0x42, 0x4F, 0x6D }); // Signature "mOBJ"
                        bw.Write(obj.KuidPart2); // KuidPart2
                        bw.Write(obj.KuidPart1); // KuidPart1
                        bw.Write(43);            // Record payload length (43 bytes)
                        bw.Write(3);             // Record version ID (3)
                        bw.Write((uint)0xFF000000); // Color / Flags
                        bw.Write((byte)1);       // 1 byte: Sub-version byte (1 = MapObjectBase sub-version 1 with Layer support)
                        bw.Write(obj.SegX);      // 2 bytes: Baseboard Segment X (short)
                        bw.Write(obj.SegY);      // 2 bytes: Baseboard Segment Y (short)
                        bw.Write(obj.X);         // 4 bytes: float X (local to baseboard)
                        bw.Write(obj.Y);         // 4 bytes: float Y (local to baseboard)
                        bw.Write(obj.Z);         // 4 bytes: float Z
                        bw.Write(obj.RotZ);      // 4 bytes: float RotZ
                        bw.Write(obj.RotY);      // 4 bytes: float RotY
                        bw.Write(obj.RotX);      // 4 bytes: float RotX
                        bw.Write(obj.LayerId);   // 1 byte: Layer ID (0 = route-layer, 1 = layer 1...)
                        bw.Write((int)1);        // 4 bytes: String length including trailing null terminator (1 for empty name)
                        bw.Write((byte)0);       // 1 byte: Null terminator for empty string name

                        if (i == objects.Count - 1)
                            bw.Write(-1); // End of list
                        else
                            bw.Write(i + 1); // Next object index
                    }
                }

                // EOF block
                bw.Write(16);
                bw.Write((uint)0xFEEDCEEB);
                bw.Write(5);
                bw.Write(9);
                bw.Write(-1);

                return ms.ToArray();
            }
        }
    }
}
