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
    /// <summary>
    /// Abstract base class providing common properties and HTTP client setup for map sources.
    /// </summary>
    internal abstract class MapSourceBase : IMapSource
    {
        /// <summary>
        /// Default tile footprint size in meters (500m x 500m or 720m x 720m).
        /// </summary>
        public static long TileSize => Properties.Settings.Default.BasemapSize;

        protected static readonly HttpClient HttpClient;

        static MapSourceBase()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                MaxConnectionsPerServer = 10,
                EnableMultipleHttp2Connections = true
            };

            HttpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30),
                DefaultRequestVersion = new Version(1, 1)
            };

            if (!HttpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                HttpClient.DefaultRequestHeaders.Add("User-Agent", "TrainzBasemapMaker/1.0 (https://github.com/Ignacy110/TrainzBasemapMaker)");
            }
        }

        public string Name { get; protected set; }
        public bool SupportsTime { get; protected set; }
        public virtual bool AllowsHighResolution => true;

        protected MapSourceBase(string name, bool supportsTime)
        {
            Name = name;
            SupportsTime = supportsTime;
        }

        public override string ToString() => Name;

        public abstract Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3, CancellationToken cancellationToken = default);

        protected static async Task<byte[]?> FetchTileBytesWithRetryAsync(string url, int maxRetries, int delaySeconds, CancellationToken cancellationToken = default)
        {
            int maxAttempts = Math.Max(1, maxRetries);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using HttpResponseMessage response = await HttpClient.GetAsync(url, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    // Retry on transient network errors
                }

                if (attempt < maxAttempts)
                {
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
                }
            }

            return null;
        }
    }
}
