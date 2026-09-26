using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace TrainzBasemapMaker.Classes
{
    internal class TrainzMeshGenerator
    {
        public static bool Generate3DBasemap(float[,] elevationGrid, string outputImPath, int size, string tmiPath, float zOffset = 0f)
        {
            if (elevationGrid.GetLength(0) != 76 || elevationGrid.GetLength(1) != 76)
                return false;

            string tempXmlPath = Path.ChangeExtension(outputImPath, ".xml");

            // We need a mesh of size x size. 
            // The grid is 76x76 covering 750x750m (-20m to +730m).
            // We want 720m x 720m (or whatever size is, usually 720 or 1000).
            // Let's assume size=720, grid spacing=10m.
            // We'll use the inner 73x73 vertices (indices 2 to 74).
            
            int offset = 2; // skip first two to start at 0m (since it starts at -20m)
            int steps = size / 10; // usually 72
            if (steps > 73) steps = 73; // cap at 730m
            int vertCount = steps + 1; // max 74

            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<trainzImport>");
            xml.AppendLine("  <version>1</version>");
            xml.AppendLine("  <mesh>");
            xml.AppendLine("    <name>m.basemap</name>");
            xml.AppendLine("    <triangles>");

            for (int y = 0; y < steps; y++)
            {
                for (int x = 0; x < steps; x++)
                {
                    // For each quad, we generate 2 triangles
                    // We need coordinates. X, Y in meters. Z in meters from elevation.
                    // UV from 0 to 1
                    
                    int gx = x + offset;
                    int gy = y + offset;

                    float x0 = x * 10f;
                    float y0 = y * 10f;
                    float x1 = (x + 1) * 10f;
                    float y1 = (y + 1) * 10f;

                    float u0 = (float)x / steps;
                    float v0 = (float)y / steps; // y=0 is North, v=0 is Top of texture
                    float u1 = (float)(x + 1) / steps;
                    float v1 = (float)(y + 1) / steps;

                    // Trainz coords: Z is up, +Y is North, +X is East
                    float halfSize = size / 2f;
                    float p_x0 = x0 - halfSize;
                    float p_y0 = halfSize - y0; // INVERTED Y (y=0 is North, so +360)
                    float p_x1 = x1 - halfSize;
                    float p_y1 = halfSize - y1; // INVERTED Y

                    float z00 = elevationGrid[gx, gy] + zOffset;
                    float z10 = elevationGrid[gx + 1, gy] + zOffset;
                    float z01 = elevationGrid[gx, gy + 1] + zOffset;
                    float z11 = elevationGrid[gx + 1, gy + 1] + zOffset;

                    // Trainz coords: Z is up
                    // Triangle 1 (Top-Left -> Bottom-Left -> Top-Right)
                    xml.AppendLine("      <triangle>");
                    xml.AppendLine("        <materialId>0</materialId>");
                    AddVertex(xml, p_x0, p_y0, z00, u0, v0);
                    AddVertex(xml, p_x0, p_y1, z01, u0, v1);
                    AddVertex(xml, p_x1, p_y0, z10, u1, v0);
                    xml.AppendLine("      </triangle>");
                    
                    // Triangle 2 (Bottom-Left -> Bottom-Right -> Top-Right)
                    xml.AppendLine("      <triangle>");
                    xml.AppendLine("        <materialId>0</materialId>");
                    AddVertex(xml, p_x0, p_y1, z01, u0, v1);
                    AddVertex(xml, p_x1, p_y1, z11, u1, v1);
                    AddVertex(xml, p_x1, p_y0, z10, u1, v0);
                    xml.AppendLine("      </triangle>");
                }
            }

            xml.AppendLine("    </triangles>");
            xml.AppendLine("  </mesh>");
            xml.AppendLine("  <materials>");
            xml.AppendLine("    <material>");
            xml.AppendLine("      <name>m.onetex</name>");
            xml.AppendLine("      <id>0</id>");
            xml.AppendLine("      <diffuse>1,1,1</diffuse>");
            xml.AppendLine("      <textures>");
            xml.AppendLine("        <texture>");
            xml.AppendLine("          <textureName>basemap-basemap.texture</textureName>");
            xml.AppendLine("          <typeName>diffuse</typeName>");
            xml.AppendLine("        </texture>");
            xml.AppendLine("      </textures>");
            xml.AppendLine("    </material>");
            xml.AppendLine("  </materials>");
            xml.AppendLine("</trainzImport>");

            string tempDir = Path.Combine(Path.GetTempPath(), "TrainzBasemapMaker_TMI");
            if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
            
            string safeXmlPath = Path.Combine(tempDir, "basemap.xml");
            string safeImPath = Path.Combine(tempDir, "basemap.im");
            
            // Clean up previous runs if any
            if (File.Exists(safeXmlPath)) File.Delete(safeXmlPath);
            if (File.Exists(safeImPath)) File.Delete(safeImPath);

            File.WriteAllText(safeXmlPath, xml.ToString());

            // Run TMI
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(tmiPath)
                {
                    Arguments = $"-inFile \"{safeXmlPath}\" -outFile \"{safeImPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                using (Process? p = Process.Start(psi))
                {
                    if (p != null)
                    {
                        string stdout = p.StandardOutput.ReadToEnd();
                        string stderr = p.StandardError.ReadToEnd();
                        p.WaitForExit(10000); // 10s timeout
                        
                        // Copy log to the final destination for debugging
                        string logFile = Path.Combine(Path.GetDirectoryName(outputImPath)!, "tmi_log.txt");
                        File.WriteAllText(logFile, $"TMI Output:\n{stdout}\n\nError:\n{stderr}");
                    }
                }
                
                // If it fails to create outFile, it might create it without the -outFile argument effect
                if (!File.Exists(safeImPath))
                {
                    string fallbackIm = Path.Combine(tempDir, "basemap.im");
                    if (File.Exists(fallbackIm))
                    {
                        safeImPath = fallbackIm;
                    }
                }

                if (File.Exists(safeImPath))
                {
                    File.Copy(safeImPath, outputImPath, true);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                // Cleanup temp files
                if (File.Exists(safeXmlPath)) File.Delete(safeXmlPath);
                if (File.Exists(safeImPath)) File.Delete(safeImPath);
            }

            return false;
        }

        private static void AddVertex(StringBuilder xml, float x, float y, float z, float u, float v)
        {
            var culture = CultureInfo.InvariantCulture;
            xml.AppendLine("        <vertex>");
            xml.AppendLine($"          <position>{x.ToString(culture)},{y.ToString(culture)},{z.ToString(culture)}</position>");
            xml.AppendLine($"          <texcoord>{u.ToString(culture)},{v.ToString(culture)}</texcoord>");
            xml.AppendLine("        </vertex>");
        }
    }
}
