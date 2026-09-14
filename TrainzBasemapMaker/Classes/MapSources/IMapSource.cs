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
    /// Represents a generic basemap provider capable of generating map imagery for given coordinates.
    /// </summary>
    internal interface IMapSource
    {
        /// <summary>
        /// Display name of the map source in the user interface.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates whether this map source supports historical/dated imagery.
        /// </summary>
        bool SupportsTime { get; }

        /// <summary>
        /// Indicates whether high resolution (e.g. 4096px) is supported by this map source.
        /// </summary>
        bool AllowsHighResolution { get; }

        /// <summary>
        /// Asynchronously retrieves the map image centered around the specified coordinates.
        /// </summary>
        /// <param name="year">Optional historical year string if supported.</param>
        /// <param name="xCenter">Center X coordinate in EPSG:2180.</param>
        /// <param name="yCenter">Center Y coordinate in EPSG:2180.</param>
        /// <param name="resolution">Desired output image resolution in pixels.</param>
        /// <param name="maxRetries">Maximum number of retry attempts upon network failure.</param>
        /// <param name="delaySeconds">Delay in seconds between retry attempts.</param>
        /// <returns>Raw image bytes (JPEG format).</returns>
        Task<byte[]> GetMapImageAsync(string year, double xCenter, double yCenter, int resolution, int maxRetries = 3, int delaySeconds = 3);
    }
}
