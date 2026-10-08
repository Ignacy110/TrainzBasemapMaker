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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace TrainzBasemapMaker.Classes
{
    internal class TrainzMeshGenerator
    {
        public static bool Generate3DBasemap(float[,] elevationGrid, string outputImPath, int size, string tmiPath, float zOffset = 0f, float baseHeight = 0f)
        {
            if (elevationGrid.GetLength(0) != Constants.GridVertexCount || elevationGrid.GetLength(1) != Constants.GridVertexCount)
                return false;

            // We need a mesh of size x size. 
            // The grid is 76x76 covering 750x750m (-20m to +730m).
            // We want 720m x 720m (or whatever size is, usually 720 or 1000).
            // Let's assume size=720, grid spacing=10m.
            // We'll use the inner 73x73 vertices (indices 2 to 74).
            
            int offset = 2; // skip first two to start at 0m (since it starts at -20m)
            int steps = size / 10; // usually 72
            if (steps > 73) steps = 73; // cap at 730m
            int vertCount = steps + 1; // max 74

            // ~2 triangles per quad, ~450 chars of XML per triangle - pre-size to avoid repeated reallocations.
            StringBuilder xml = new StringBuilder(steps * steps * 2 * 450 + 1024);
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

                    float z00 = elevationGrid[gx, gy] - baseHeight + zOffset;
                    float z10 = elevationGrid[gx + 1, gy] - baseHeight + zOffset;
                    float z01 = elevationGrid[gx, gy + 1] - baseHeight + zOffset;
                    float z11 = elevationGrid[gx + 1, gy + 1] - baseHeight + zOffset;

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
            Directory.CreateDirectory(tempDir);

            // Unique file names per call so several basemaps can be generated in parallel.
            string uniqueId = Guid.NewGuid().ToString("N");
            string safeXmlPath = Path.Combine(tempDir, $"basemap_{uniqueId}.xml");
            string safeImPath = Path.Combine(tempDir, $"basemap_{uniqueId}.im");

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
                        // Read both streams concurrently - reading them one after another can deadlock
                        // when the child process fills the other pipe's buffer.
                        Task<string> stdoutTask = p.StandardOutput.ReadToEndAsync();
                        Task<string> stderrTask = p.StandardError.ReadToEndAsync();

                        bool exited = p.WaitForExit(TmiTimeoutMs);
                        if (!exited)
                        {
                            try { p.Kill(entireProcessTree: true); } catch { /* already exited */ }
                        }

                        string stdout = stdoutTask.Wait(2000) ? stdoutTask.Result : string.Empty;
                        string stderr = stderrTask.Wait(2000) ? stderrTask.Result : string.Empty;
                        if (!exited)
                        {
                            stderr += $"\n[TrainzBasemapMaker] TMI przekroczył limit czasu ({TmiTimeoutMs / 1000} s) i został zatrzymany.";
                        }

                        // Copy log to the final destination for debugging
                        string logFile = Path.Combine(Path.GetDirectoryName(outputImPath)!, "tmi_log.txt");
                        File.WriteAllText(logFile, $"TMI Output:\n{stdout}\n\nError:\n{stderr}");
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
                try { if (File.Exists(safeXmlPath)) File.Delete(safeXmlPath); } catch { }
                try { if (File.Exists(safeImPath)) File.Delete(safeImPath); } catch { }
            }

            return false;
        }

        /// <summary>
        /// Maximum time TrainzMeshImporter may run for a single basemap before it is killed.
        /// </summary>
        private const int TmiTimeoutMs = 60_000;

        private static void AddVertex(StringBuilder xml, float x, float y, float z, float u, float v)
        {
            // The interpolated Append(IFormatProvider, ...) overload formats numbers directly into the
            // builder without allocating intermediate strings (~31k vertices per basemap).
            var culture = CultureInfo.InvariantCulture;
            xml.Append(culture, $"        <vertex>\r\n          <position>{x},{y},{z}</position>\r\n          <texcoord>{u},{v}</texcoord>\r\n        </vertex>\r\n");
        }
    }
}
