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

using System.Text.Json;
using TrainzBasemapMaker.Classes.TrainzTerrain;

namespace TrainzBasemapMaker.Classes
{
    internal class TrainzFileManager
    {
        private static readonly string RootFolder = Path.Combine(AppContext.BaseDirectory, "Kuids");
        public const string GroupInfoFileName = "group_info.json";
        public const string TerrainInfoFileName = "terrain_info.json";

        /// <summary>
        /// Attempts to parse tile metadata from a basemap folder name.
        /// </summary>
        public static bool TryParseTileFolderName(
            string? folderName,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TileFolderInfo? tileInfo)
        {
            tileInfo = null;
            if (string.IsNullOrWhiteSpace(folderName)) return false;

            string[] parts = folderName.Split('_');
            if (parts.Length < 5 || parts[0] != "basemap") return false;

            string defaultKuid1 = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";

            if (parts.Length >= 7)
            {
                if (long.TryParse(parts[parts.Length - 4], out long x) &&
                    long.TryParse(parts[parts.Length - 3], out long y) &&
                    int.TryParse(parts[parts.Length - 5], out int counter))
                {
                    string kuid1 = parts[parts.Length - 2];
                    string kuid2 = parts[parts.Length - 1];
                    string designation = string.Join("_", parts.Skip(1).Take(parts.Length - 6));
                    tileInfo = new TileFolderInfo(designation, counter, x, y, kuid1, kuid2);
                    return true;
                }
            }

            // Fallback for 5 or 6 parts format: basemap_{designation}_{counter}_{x}_{y}[_{kuid1}]
            if (parts.Length >= 5)
            {
                if (long.TryParse(parts[3], out long x) &&
                    long.TryParse(parts[4], out long y) &&
                    int.TryParse(parts[2], out int counter))
                {
                    string designation = parts[1];
                    string kuid1 = parts.Length >= 6 ? parts[5] : defaultKuid1;
                    tileInfo = new TileFolderInfo(designation, counter, x, y, kuid1, "1");
                    return true;
                }
            }

            return false;
        }

        public string CreateRouteFiles(string routeName, string basemapGroup, string kuidPart1, string kuidPart2, byte[] gndData, TerrainRouteInfo? routeInfo = null)
        {
            string groupPath = Path.Combine(RootFolder, basemapGroup);
            if (!Directory.Exists(groupPath))
            {
                Directory.CreateDirectory(groupPath);
            }

            string safeRouteName = string.Join("_", routeName.Split(Path.GetInvalidFileNameChars()));
            string targetFolderName = $"route_{safeRouteName}_{kuidPart1}_{kuidPart2}";
            string targetFolder = Path.Combine(groupPath, targetFolderName);
            Directory.CreateDirectory(targetFolder);

            // Zapis mapfile.gnd (główny plik siatki)
            File.WriteAllBytes(Path.Combine(targetFolder, "mapfile.gnd"), gndData);

            // Zapis pustego mapfile.obs (obiekty - wymagane przez niektóre wersje Trainz)
            byte[] emptyObs = { 0x07, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };
            File.WriteAllBytes(Path.Combine(targetFolder, "mapfile.obs"), emptyObs);

            // Zapis pustego mapfile.trk (tory - wymagane przez niektóre wersje Trainz)
            byte[] emptyTrk = { 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };
            File.WriteAllBytes(Path.Combine(targetFolder, "mapfile.trk"), emptyTrk);

            string configText = $"kuid                                    <kuid:{kuidPart1}:{kuidPart2}>\r\nkind                                    \"map\"\r\nusername                                \"{routeName}\"\r\ncategory-class                          \"YM\"\r\ncategory-region                         \"PL\"\r\ncategory-era                            \"2020s\"\r\ntrainz-build                            2.9\r\n\r\nthumbnails\r\n{{\r\n  0\r\n  {{\r\n    image                               \"thumbnail.jpg\"\r\n    width                               240\r\n    height                              180\r\n  }}\r\n}}\r\n";
            File.WriteAllText(Path.Combine(targetFolder, "config.txt"), configText);
            File.WriteAllBytes(Path.Combine(targetFolder, "thumbnail.jpg"), Properties.Resources.thumbnail_jpg);

            if (routeInfo != null)
            {
                routeInfo.FolderPath = targetFolder;
                SaveTerrainRouteInfo(targetFolder, routeInfo);
            }

            return targetFolder;
        }

        public void SaveTerrainRouteInfo(string targetFolder, TerrainRouteInfo info)
        {
            if (string.IsNullOrWhiteSpace(targetFolder)) return;

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string jsonPath = Path.Combine(targetFolder, TerrainInfoFileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(info, options);
            File.WriteAllText(jsonPath, json);
        }

        public void DeleteRouteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        public List<TerrainRouteInfo> GetTerrainRoutes()
        {
            var result = new List<TerrainRouteInfo>();
            if (!Directory.Exists(RootFolder)) return result;

            var gndFiles = Directory.GetFiles(RootFolder, "mapfile.gnd", SearchOption.AllDirectories);
            foreach (var gndFile in gndFiles)
            {
                string folder = Path.GetDirectoryName(gndFile)!;
                string infoJsonPath = Path.Combine(folder, TerrainInfoFileName);

                TerrainRouteInfo? info = null;
                if (File.Exists(infoJsonPath))
                {
                    try
                    {
                        string json = File.ReadAllText(infoJsonPath);
                        info = JsonSerializer.Deserialize<TerrainRouteInfo>(json);
                    }
                    catch
                    {
                        info = null;
                    }
                }

                if (info == null)
                {
                    info = TryParseLegacyRouteFolder(folder, gndFile);
                }

                if (info != null)
                {
                    info.FolderPath = folder;
                    result.Add(info);
                }
            }

            return result.OrderBy(r => r.RouteName).ToList();
        }

        private TerrainRouteInfo? TryParseLegacyRouteFolder(string folder, string gndFile)
        {
            try
            {
                string routeName = Path.GetFileName(folder);
                string kuid1 = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";
                string kuid2 = "1";

                string configPath = Path.Combine(folder, "config.txt");
                if (File.Exists(configPath))
                {
                    var lines = File.ReadAllLines(configPath);
                    foreach (var line in lines)
                    {
                        var trimmed = line.Trim();
                        if (trimmed.StartsWith("username", StringComparison.OrdinalIgnoreCase))
                        {
                            int firstQuote = trimmed.IndexOf('"');
                            int lastQuote = trimmed.LastIndexOf('"');
                            if (firstQuote != -1 && lastQuote > firstQuote)
                            {
                                routeName = trimmed.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
                            }
                        }
                        else if (trimmed.StartsWith("kuid", StringComparison.OrdinalIgnoreCase))
                        {
                            int startKuid = trimmed.IndexOf("<kuid:");
                            int endKuid = trimmed.IndexOf(">");
                            if (startKuid != -1 && endKuid > startKuid)
                            {
                                string inside = trimmed.Substring(startKuid + 6, endKuid - (startKuid + 6));
                                var kuidParts = inside.Split(':');
                                if (kuidParts.Length >= 2)
                                {
                                    kuid1 = kuidParts[0];
                                    kuid2 = kuidParts[1];
                                }
                            }
                        }
                    }
                }

                var parts = GndReader.ReadGndFile(File.ReadAllBytes(gndFile));
                var tiles = new List<TerrainTileInfo>();
                int order = 1;
                foreach (var p in parts)
                {
                    tiles.Add(new TerrainTileInfo
                    {
                        Order = order++,
                        I = p.SegmentY,
                        J = -p.SegmentX,
                        X = 0,
                        Y = 0
                    });
                }

                return new TerrainRouteInfo
                {
                    RouteName = routeName,
                    KuidPart1 = kuid1,
                    KuidPart2 = kuid2,
                    Epsg = "EPSG:2180",
                    IsRelative = false,
                    Tiles = tiles,
                    FolderPath = folder
                };
            }
            catch
            {
                return null;
            }
        }
        public bool CreateTrainzFiles(byte[] imageBytes, string basemapGroup, long x, long y, string basemapGroupDesignation, int counter, string kuidPart1, string kuidPart2)
        {
            // 1. building paths with Path.Combine
            string groupPath = Path.Combine(RootFolder, basemapGroup);

            if (!Directory.Exists(groupPath))
            {
                Directory.CreateDirectory(groupPath);
            }

            var existingTiles = Directory.GetDirectories(groupPath, "basemap_*")
                .Select(Path.GetFileName)
                .Where(name =>
                {
                    if (string.IsNullOrEmpty(name)) return false;
                    return TryParseTileFolderName(name, out var tileInfo) &&
                           tileInfo.X == x && tileInfo.Y == y;
                });

            if (existingTiles.Any())
            {
                return false; // informing Form that we have done nothing (duplicate)
            }

            // 3. creating a destination folder
            string targetFolderName = $"basemap_{basemapGroupDesignation}_{counter}_{x}_{y}_{kuidPart1}_{kuidPart2}";
            string targetFolder = Path.Combine(groupPath, targetFolderName);
            Directory.CreateDirectory(targetFolder);

            // 4. writing files
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap.jpg"), imageBytes);
            File.WriteAllBytes(Path.Combine(targetFolder, "thumbnail.jpg"), Properties.Resources.thumbnail_jpg);
            if (Properties.Settings.Default.BasemapSize == 720)
                File.WriteAllBytes(Path.Combine(targetFolder, "basemap.im"), Properties.Resources.basemap720_im);
            else
                File.WriteAllBytes(Path.Combine(targetFolder, "basemap.im"), Properties.Resources.basemap_im);
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap-basemap.texture.txt"), Properties.Resources.basemap_basemap_texture_txt);

            // 5. creating config.txt
            string configText = System.Text.Encoding.UTF8.GetString(Properties.Resources.config_txt);

            configText = configText.Replace("{{value1}}", kuidPart1)
                                   .Replace("{{value2}}", kuidPart2)
                                   .Replace("{{designation}}", basemapGroupDesignation)
                                   .Replace("{{counter}}", counter.ToString())
                                   .Replace("{{lon}}", x.ToString())
                                   .Replace("{{lat}}", y.ToString());

            File.WriteAllText(Path.Combine(targetFolder, "config.txt"), configText);

            return true;
        }

        public void DeleteGroupFolder(string basemapGroup)
        {
            string groupPath = Path.Combine(RootFolder, basemapGroup);

            if (Directory.Exists(groupPath))
            {
                Directory.Delete(groupPath, true);
            }
        }

        public List<string> GetBasemapGroups()
        {
            if (!Directory.Exists(RootFolder)) return new List<string>();

            return Directory.GetDirectories(RootFolder)
                            .Select(Path.GetFileName)
                            .OfType<string>()
                            .ToList();
        }

        public List<string> GetKuidsInGroup(string groupName)
        {
            string groupPath = Path.Combine(RootFolder, groupName);
            if (!Directory.Exists(groupPath)) return new List<string>();

            return Directory.GetDirectories(groupPath)
                            .Select(Path.GetFileName)
                            .OfType<string>()
                            .ToList();
        }

        public string GetImagePath(string groupName, string folderName)
        {
            return Path.Combine(RootFolder, groupName, folderName, "basemap.jpg");
        }

        public int GetNextFreeKuidPart2(string? targetKuidPart1 = null)
        {
            if (!Directory.Exists(RootFolder))
            {
                return Math.Max(1, Properties.Settings.Default.MinKuidPart2);
            }

            bool filterByPart1 = Properties.Settings.Default.KuidAutoCountPerFirstPart;
            if (string.IsNullOrWhiteSpace(targetKuidPart1))
            {
                targetKuidPart1 = Properties.Settings.Default.DefaultKuidFirstPart ?? "0";
            }
            targetKuidPart1 = targetKuidPart1.Trim();

            HashSet<int> usedKuidsPart2 = new HashSet<int>();

            // Find all folders in all subdirectories whose name starts with "basemap_"
            var allFolders = Directory.GetDirectories(RootFolder, "basemap_*", SearchOption.AllDirectories);

            foreach (var folder in allFolders)
            {
                string folderName = new DirectoryInfo(folder).Name;
                if (TryParseTileFolderName(folderName, out var tileInfo) &&
                    int.TryParse(tileInfo.KuidPart2, out int parsedKuidPart2))
                {
                    if (!filterByPart1 || string.Equals(tileInfo.KuidPart1?.Trim(), targetKuidPart1, StringComparison.OrdinalIgnoreCase))
                    {
                        usedKuidsPart2.Add(parsedKuidPart2);
                    }
                }
            }

            // Find all folders in all subdirectories whose name starts with "route_"
            var routeFolders = Directory.GetDirectories(RootFolder, "route_*", SearchOption.AllDirectories);

            foreach (var folder in routeFolders)
            {
                string folderName = new DirectoryInfo(folder).Name;
                string[] parts = folderName.Split('_');
                if (parts.Length >= 4 &&
                    int.TryParse(parts[parts.Length - 1], out int parsedKuidPart2))
                {
                    string routeKuid1 = parts[parts.Length - 2].Trim();
                    if (!filterByPart1 || string.Equals(routeKuid1, targetKuidPart1, StringComparison.OrdinalIgnoreCase))
                    {
                        usedKuidsPart2.Add(parsedKuidPart2);
                    }
                }
            }

            int minKuid = Properties.Settings.Default.MinKuidPart2;
            if (minKuid < 1) minKuid = 1;

            // Find the smallest unused value starting from minKuid
            int freeKuid = minKuid;
            while (usedKuidsPart2.Contains(freeKuid))
            {
                freeKuid++;
            }

            return freeKuid;
        }

        public int GetNextFreeCounter(string basemapGroup)
        {
            if (string.IsNullOrWhiteSpace(basemapGroup)) return 1;

            string groupPath = Path.Combine(RootFolder, basemapGroup);

            if (!Directory.Exists(groupPath))
            {
                return 1;
            }

            var allFolders = Directory.GetDirectories(groupPath, "basemap_*", SearchOption.AllDirectories);

            HashSet<int> usedCounter = new HashSet<int>();

            foreach (var folder in allFolders)
            {
                string folderName = new DirectoryInfo(folder).Name;
                if (TryParseTileFolderName(folderName, out var tileInfo))
                {
                    usedCounter.Add(tileInfo.Counter);
                }
            }

            int freeCounter = 1;
            while (usedCounter.Contains(freeCounter))
            {
                freeCounter++;
            }

            return freeCounter;
        }

        /// <summary>
        /// Retrieves the group metadata from group_info.json if it exists.
        /// </summary>
        public BasemapGroupInfo? GetGroupInfo(string basemapGroup)
        {
            if (string.IsNullOrWhiteSpace(basemapGroup)) return null;

            string jsonPath = Path.Combine(RootFolder, basemapGroup, GroupInfoFileName);
            if (!File.Exists(jsonPath)) return null;

            try
            {
                string json = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<BasemapGroupInfo>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Saves or updates the group metadata in group_info.json.
        /// </summary>
        public void SaveGroupInfo(string basemapGroup, BasemapGroupInfo info)
        {
            if (string.IsNullOrWhiteSpace(basemapGroup)) return;

            string groupPath = Path.Combine(RootFolder, basemapGroup);
            if (!Directory.Exists(groupPath))
            {
                Directory.CreateDirectory(groupPath);
            }

            string jsonPath = Path.Combine(groupPath, GroupInfoFileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(info, options);
            File.WriteAllText(jsonPath, json);
        }
    }
}





