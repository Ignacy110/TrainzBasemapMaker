using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrainzBasemapMaker.Classes
{
    public class WcsElevationProvider
    {
        private readonly HttpClient _httpClient;

                        public WcsElevationProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            _httpClient.DefaultRequestVersion = new Version(1, 1);
        }

        /// <summary>
        /// Pobiera siatkÄ™ wysokoÅ›ciowÄ… NMT (Arc/Info ASCII Grid) z WCS Geoportalu i parsuje do tablicy dwuwymiarowej.
        /// WspÃ³Å‚rzÄ™dne podane w ukÅ‚adzie EPSG:2180 dla Å›rodka kafla. Kafel ma wymiary 720m x 720m, ale musimy pobraÄ‡ z lekkim marginesem,
        /// aby upewniÄ‡ siÄ™, Å¼e pobierzemy 76x76 wierzchoÅ‚kÃ³w ze stÄ™pem 10m (czyli de facto obszar 760mx760m).
        /// </summary>
                public async Task<float[,]> GetElevationGridAsync(long centerX, long centerY)
        {
            // Trainz u¿ywa 76 wierzcho³ków ze spacingiem 10m dla kafla 720m.
            // Zale¿y nam na idealnym trafieniu œrodków pikseli (10x10m) w grid Trainza (-20m do +730m).
            // Geoportal BBOX definiuje zewnêtrzne granice pikseli, wiêc dodajemy po 5m na krawêdzie.
            double minX = centerX - 385.0;
            double maxX = centerX + 375.0;
            double minY = centerY - 375.0;
            double maxY = centerY + 385.0;

            string url = $"https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WCS/DigitalTerrainModel?SERVICE=WCS&VERSION=1.0.0&REQUEST=GetCoverage&COVERAGE=DTM_PL-EVRF2007-NH&FORMAT=image/x-aaigrid&BBOX={minX.ToString(CultureInfo.InvariantCulture)},{minY.ToString(CultureInfo.InvariantCulture)},{maxX.ToString(CultureInfo.InvariantCulture)},{maxY.ToString(CultureInfo.InvariantCulture)}&CRS=EPSG:2180&WIDTH=76&HEIGHT=76";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

                        string asciiGrid = await response.Content.ReadAsStringAsync();
            System.IO.File.WriteAllText("last_wcs_response.txt", asciiGrid);
            return ParseAsciiGrid(asciiGrid);
        }

                private float[,] ParseAsciiGrid(string gridText)
        {
            float[,] grid = new float[76, 76];
            using (StringReader reader = new StringReader(gridText))
            {
                int ncols = 0, nrows = 0;
                string? line = reader.ReadLine();
                
                // Czytanie nag³ówka
                while (line != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) 
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    // Jeœli to nie jest linia z param-wartoœæ (np. zaczyna siê od liczby), to to jest pierwszy wiersz danych
                    if (parts.Length > 2 || (parts.Length > 0 && float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out _)))
                    {
                        break;
                    }

                    if (parts.Length >= 2)
                    {
                        if (parts[0].Equals("ncols", StringComparison.OrdinalIgnoreCase))
                            ncols = int.Parse(parts[1]);
                        else if (parts[0].Equals("nrows", StringComparison.OrdinalIgnoreCase))
                            nrows = int.Parse(parts[1]);
                    }
                    
                    line = reader.ReadLine();
                }

                if (ncols != 76 || nrows != 76)
                {
                    throw new InvalidDataException($"Pobrano grid o z³ym rozmiarze: {ncols}x{nrows}. Oczekiwano 76x76.");
                }

                // Wczytywanie wierszy z danymi
                for (int y = 0; y < nrows; y++)
                {
                    if (line == null) throw new InvalidDataException("Niespodziewany koniec danych w pliku ASCII Grid.");

                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length != ncols)
                    {
                        throw new InvalidDataException($"Oczekiwano {ncols} wartoœci w wierszu, a znaleziono {parts.Length}.");
                    }

                    for (int x = 0; x < ncols; x++)
                    {
                        if (float.TryParse(parts[x], NumberStyles.Any, CultureInfo.InvariantCulture, out float val))
                        {
                            if (val < -1000) val = 0; 
                            grid[x, y] = val;
                        }
                    }
                    
                    line = reader.ReadLine(); // advance to next row
                }
            }

            return grid;
        }
    }
}






