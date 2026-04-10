namespace TrainzBasemapMaker
{
    internal class TrainzFileManager
    {
        private const string RootFolder = "Kuids";
        public bool CreateTrainzFiles(byte[] imageBytes, string basemapGroup, long x, long y, string basemapGroupDesignation, int counter, string kuidPart1)
        {
            // 1. building paths with Path.Combine
            string groupPath = Path.Combine(RootFolder, basemapGroup);

            if (!Directory.Exists(groupPath))
            {
                Directory.CreateDirectory(groupPath);
            }

            // 2. duplicate Check
            string searchPattern = $"*_{x}_{y}";
            var existingTiles = Directory.GetDirectories(groupPath, searchPattern);

            if (existingTiles.Length > 0)
            {
                return false; // informing Form that we have done nothing (duplicate)
            }

            // 3. creating a destination folder
            string targetFolderName = $"basemap_{basemapGroupDesignation}_{counter}_{x}_{y}";
            string targetFolder = Path.Combine(groupPath, targetFolderName);
            Directory.CreateDirectory(targetFolder);

            // 4. writing files
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap.jpg"), imageBytes);
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap.im"), Properties.Resources.basemap_im);
            File.WriteAllBytes(Path.Combine(targetFolder, "basemap-basemap.texture.txt"), Properties.Resources.basemap_basemap_texture_txt);

            // 5. creating config.txt
            string configText = System.Text.Encoding.UTF8.GetString(Properties.Resources.config_txt);

            configText = configText.Replace("value1", kuidPart1)
                                   .Replace("value2", counter.ToString())
                                   .Replace("designation", basemapGroupDesignation)
                                   .Replace("counter", counter.ToString())
                                   .Replace("lon", x.ToString())
                                   .Replace("lat", y.ToString());

            File.WriteAllText(Path.Combine(targetFolder, "config.txt"), configText);

            return true;
        }

        public List<string> GetBasemapGroups()
        {
            if (!Directory.Exists(RootFolder)) return new List<string>();

            return Directory.GetDirectories(RootFolder)
                            .Select(Path.GetFileName)
                            .ToList();
        }

        public List<string> GetKuidsInGroup(string groupName)
        {
            string groupPath = Path.Combine(RootFolder, groupName);
            if (!Directory.Exists(groupPath)) return new List<string>();

            return Directory.GetDirectories(groupPath)
                            .Select(Path.GetFileName)
                            .ToList();
        }

        public string GetImagePath(string groupName, string folderName)
        {
            return Path.Combine(RootFolder, groupName, folderName, "basemap.jpg");
        }
    }
}
