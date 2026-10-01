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
        public int LayerId { get; set; } = 1;
        public float X { get; set; }
        public float Y { get; set; }
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
                        bw.Write(new byte[] { 0x4A, 0x42, 0x4F, 0x6D }); // Signature "mOBJ" in expected byte order
                        bw.Write(obj.KuidPart2); // Actually KuidPart2
                        bw.Write(obj.KuidPart1); // KuidPart1
                        bw.Write(43);            // Magic type ID
                        bw.Write(3);             // Magic version ID
                        bw.Write((uint)0xFF000000);
                        bw.Write(obj.LayerId); // Layer ID
                        bw.Write((byte)0);
                        bw.Write(obj.X);
                        bw.Write(obj.Y);
                        bw.Write(obj.Z);
                        bw.Write(obj.RotZ);
                        bw.Write(obj.RotY);
                        bw.Write(obj.RotX);
                        bw.Write((byte)0);
                        bw.Write(1); // Unknown or Scale
                        bw.Write((byte)0);

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
