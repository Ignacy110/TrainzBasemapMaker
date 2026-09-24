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
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TrainzBasemapMaker.Classes
{
    internal class WcsElevationProvider
    {
        private static readonly HttpClient _httpClient;

        static WcsElevationProvider()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                MaxConnectionsPerServer = 10,
                EnableMultipleHttp2Connections = true
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30),
                DefaultRequestVersion = new Version(1, 1)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        /// <summary>
        /// Pobiera siatke wysokosciowa NMT (Arc/Info ASCII Grid) z WCS Geoportalu i parsuje do tablicy dwuwymiarowej [76, 76].
        /// Wspolrzedne podane w ukladzie EPSG:2180 dla srodka kafla. Kafel ma wymiary 720m x 720m,
        /// pobierany jest bufor 760m x 760m ze skokiem 10m (76 wierzcholkow), idealnie dopasowany do formatu GND Trainz.
        /// </summary>
        public async Task<float[,]> GetElevationGridAsync(long centerX, long centerY, CancellationToken cancellationToken = default)
        {
            if (!GeoHelperEPSG2180.IsWithin2180Bounds(centerX, centerY))
            {
                throw new ArgumentException($"Wspolrzedne ({centerX}, {centerY}) znajduja sie poza poprawnym obszarem Polski w ukladzie EPSG:2180.");
            }

            // Trainz uzywa 76 wierzcholkow ze spacingiem 10m dla kafla 720m (zakres -20m do +730m).
            // BBOX Geoportalu definiuje zewnetrzne granice pikseli:
            double minX = centerX - 385.0;
            double maxX = centerX + 375.0;
            double minY = centerY - 375.0;
            double maxY = centerY + 385.0;

            string url = $"https://mapy.geoportal.gov.pl/wss/service/PZGIK/NMT/GRID1/WCS/DigitalTerrainModel?SERVICE=WCS&VERSION=1.0.0&REQUEST=GetCoverage&COVERAGE=DTM_PL-EVRF2007-NH&FORMAT=image/x-aaigrid&BBOX={minX.ToString(CultureInfo.InvariantCulture)},{minY.ToString(CultureInfo.InvariantCulture)},{maxX.ToString(CultureInfo.InvariantCulture)},{maxY.ToString(CultureInfo.InvariantCulture)}&CRS=EPSG:2180&WIDTH=76&HEIGHT=76";

            string asciiGrid = string.Empty;
            Exception? lastEx = null;

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    using (HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        asciiGrid = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    if (attempt < 3)
                    {
                        await Task.Delay(400 * attempt, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(asciiGrid))
            {
                throw new InvalidOperationException($"Nie udalo sie pobrac danych wysokosciowych z Geoportalu po 3 probach: {lastEx?.Message}", lastEx);
            }

            if (asciiGrid.TrimStart().StartsWith("<", StringComparison.Ordinal))
            {
                string errMsg = ExtractServiceException(asciiGrid);
                throw new InvalidDataException($"Geoportal WCS zwrocil blad uslugi: {errMsg}");
            }

            return ParseAsciiGrid(asciiGrid);
        }

        private static string ExtractServiceException(string xml)
        {
            try
            {
                var match = Regex.Match(xml, @"<ServiceException[^>]*>(.*?)</ServiceException>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }
            catch
            {
                // Ignore regex parsing error
            }
            return "Nieznany blad WCS (odpowiedz XML zamiast siatki AAIGRID)";
        }

        private float[,] ParseAsciiGrid(string gridText)
        {
            float[,] grid = new float[76, 76];
            float noDataVal = -9999f;

            using (StringReader reader = new StringReader(gridText))
            {
                int ncols = 0, nrows = 0;
                string? line = reader.ReadLine();

                // Czytanie naglowka
                while (line != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        line = reader.ReadLine();
                        continue;
                    }

                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // Jesli linia zawiera dane (liczby wiersza)
                    if (parts.Length > 2 || (parts.Length > 0 && float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out _)))
                    {
                        break;
                    }

                    if (parts.Length >= 2)
                    {
                        if (parts[0].Equals("ncols", StringComparison.OrdinalIgnoreCase))
                            ncols = int.Parse(parts[1], CultureInfo.InvariantCulture);
                        else if (parts[0].Equals("nrows", StringComparison.OrdinalIgnoreCase))
                            nrows = int.Parse(parts[1], CultureInfo.InvariantCulture);
                        else if (parts[0].Equals("nodata_value", StringComparison.OrdinalIgnoreCase))
                        {
                            if (float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedNoData))
                            {
                                noDataVal = parsedNoData;
                            }
                        }
                    }

                    line = reader.ReadLine();
                }

                if (ncols != 76 || nrows != 76)
                {
                    throw new InvalidDataException($"Pobrano grid o nieprawidlowym rozmiarze: {ncols}x{nrows}. Oczekiwano 76x76.");
                }

                // Wczytywanie wierszy z danymi (y = 0: Polnoc, y = 75: Poludnie)
                for (int y = 0; y < nrows; y++)
                {
                    if (line == null) throw new InvalidDataException("Niespodziewany koniec danych w pliku ASCII Grid.");

                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != ncols)
                    {
                        throw new InvalidDataException($"Oczekiwano {ncols} wartosci w wierszu {y}, a znaleziono {parts.Length}.");
                    }

                    for (int x = 0; x < ncols; x++)
                    {
                        if (float.TryParse(parts[x], NumberStyles.Any, CultureInfo.InvariantCulture, out float val))
                        {
                            grid[x, y] = val;
                        }
                        else
                        {
                            grid[x, y] = noDataVal;
                        }
                    }

                    line = reader.ReadLine();
                }
            }

            FixNoDataValues(grid, noDataVal);

            return grid;
        }

        private static void FixNoDataValues(float[,] grid, float noDataVal)
        {
            float validSum = 0;
            int validCount = 0;

            for (int y = 0; y < 76; y++)
            {
                for (int x = 0; x < 76; x++)
                {
                    float v = grid[x, y];
                    if (Math.Abs(v - noDataVal) > 0.01f && v > -1000f)
                    {
                        validSum += v;
                        validCount++;
                    }
                }
            }

            float fallback = validCount > 0 ? (validSum / validCount) : 0f;

            for (int y = 0; y < 76; y++)
            {
                for (int x = 0; x < 76; x++)
                {
                    float v = grid[x, y];
                    if (Math.Abs(v - noDataVal) <= 0.01f || v <= -1000f)
                    {
                        float neighborSum = 0;
                        int neighborCount = 0;
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                int nx = x + dx;
                                int ny = y + dy;
                                if (nx >= 0 && nx < 76 && ny >= 0 && ny < 76)
                                {
                                    float nv = grid[nx, ny];
                                    if (Math.Abs(nv - noDataVal) > 0.01f && nv > -1000f)
                                    {
                                        neighborSum += nv;
                                        neighborCount++;
                                    }
                                }
                            }
                        }
                        grid[x, y] = neighborCount > 0 ? (neighborSum / neighborCount) : fallback;
                    }
                }
            }
        }
    }
}
