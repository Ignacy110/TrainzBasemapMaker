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

namespace TrainzBasemapMaker.Classes
{
    internal class TrainzFileManager
    {
        private static readonly string RootFolder = Path.Combine(AppContext.BaseDirectory, "Kuids");

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
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap.im"), Properties.Resources.basemap_im);
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap-basemap.texture.txt"), Properties.Resources.basemap_basemap_texture_txt);

            // 5. creating config.txt
            string configText = System.Text.Encoding.UTF8.GetString(Properties.Resources.config_txt);

            configText = configText.Replace("value1", kuidPart1)
                                   .Replace("value2", kuidPart2)
                                   .Replace("designation", basemapGroupDesignation)
                                   .Replace("counter", counter.ToString())
                                   .Replace("lon", x.ToString())
                                   .Replace("lat", y.ToString());

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

        public int GetNextFreeKuidPart2()
        {
            if (!Directory.Exists(RootFolder))
            {
                return 1;
            }

            // Find all folders in all subdirectories whose name starts with "basemap_"
            var allFolders = Directory.GetDirectories(RootFolder, "basemap_*", SearchOption.AllDirectories);

            HashSet<int> usedKuidsPart2 = new HashSet<int>();

            foreach (var folder in allFolders)
            {
                string folderName = new DirectoryInfo(folder).Name;
                if (TryParseTileFolderName(folderName, out var tileInfo) &&
                    int.TryParse(tileInfo.KuidPart2, out int parsedKuidPart2))
                {
                    usedKuidsPart2.Add(parsedKuidPart2);
                }
            }

            // Find the smallest unused value starting from 1
            int freeKuid = 1;
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
    }
}