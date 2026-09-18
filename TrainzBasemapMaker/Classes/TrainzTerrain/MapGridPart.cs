using System;
using System.IO;

namespace TrainzBasemapMaker.Classes.TrainzTerrain
{
    public class MapGridPart
    {
        private const int DATASIZE = 76;
        private const int MARGIN_TOP_LEFT = 2; // unused directly here because we supply 76x76 grid straight
        private const byte ROT_SETTING = 0x00;

        private float[,] _heights = new float[DATASIZE, DATASIZE];
        
        public int SegmentX { get; }
        public int SegmentY { get; }

        public MapGridPart(int segmentX, int segmentY)
        {
            SegmentX = segmentX;
            SegmentY = segmentY;
        }

        public void SetHeights(float[,] heights)
        {
            if (heights.GetLength(0) != DATASIZE || heights.GetLength(1) != DATASIZE)
                throw new ArgumentException($"Z³y wymiar siatki wysokoœci: {heights.GetLength(0)}x{heights.GetLength(1)}");

            _heights = heights;
        }

        public void WriteToFile(BinaryWriter writer, byte defaultTextureId)
        {
            // 4 bajty zera na pocz¹tku
            writer.Write((int)0);

            // Zapis w uk³adzie: ZACHÓD -> WSCHÓD (kolumny/Easting), a wewn¹trz: PÓ£NOC -> PO£UDNIE (wiersze/Northing)
            for (int easting = 0; easting < DATASIZE; easting++)
            {
                for (int northing = 0; northing < DATASIZE; northing++)
                {
                    float h = _heights[easting, northing];
                    // Zapis pojedyñczego kafelka wysokoœciowego:
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
                // Header (18 bytes)
                byte[] header = {
                    0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x80, 0x00, 0x80, 0x00, 0x20, 0x08
                };
                bw.Write(header);

                // 128x128 pikseli podgl¹du (wype³niamy na zielono/szaro dla testu)
                for (int i = 0; i < 128 * 128; i++)
                {
                    bw.Write((byte)100); // R
                    bw.Write((byte)150); // G
                    bw.Write((byte)100); // B
                    bw.Write((byte)0);   // Alpha
                }
                return ms.ToArray();
            }
        }
    }
}
