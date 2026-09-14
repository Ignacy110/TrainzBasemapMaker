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
        /// Default tile footprint size in meters (500m x 500m).
        /// </summary>
        public const long TileSize = 500;

        protected static readonly HttpClient HttpClient = new HttpClient();

        static MapSourceBase()
        {
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

        public abstract Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3);
    }
}
