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
    /// Helper class providing spherical Mercator coordinate conversions for EPSG:3857 (WGS 84 / Pseudo-Mercator).
    /// Used globally by OpenStreetMap, OpenRailwayMap, Google Maps, Bing Maps, and international mapping standards.
    /// </summary>
    internal static class GeoHelperEPSG3857
    {
        // Half circumference of the Earth in meters (WGS 84 semi-major axis * PI)
        public const double OriginShift = 20037508.342789244;

        // Valid latitude range for spherical Mercator projection (in degrees)
        public const double MaxLatitude = 85.05112878;
        public const double MinLatitude = -85.05112878;

        /// <summary>
        /// Converts geographic coordinates (WGS84 Latitude/Longitude in degrees) to projected meters in EPSG:3857 (Web Mercator).
        /// </summary>
        public static (double x, double y) LatLonToMeters3857(double lat, double lon)
        {
            double x = lon * OriginShift / 180.0;
            double clampedLat = Math.Clamp(lat, MinLatitude, MaxLatitude);
            double latRad = clampedLat * Math.PI / 180.0;
            double y = Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0)) * OriginShift / Math.PI;
            return (x, y);
        }

        /// <summary>
        /// Converts projected meters in EPSG:3857 (Web Mercator) to geographic coordinates (WGS84 Latitude/Longitude in degrees).
        /// </summary>
        public static (double lat, double lon) Meters3857ToLatLon(double x, double y)
        {
            double lon = x * 180.0 / OriginShift;
            double lat = (2.0 * Math.Atan(Math.Exp(y * Math.PI / OriginShift)) - Math.PI / 2.0) * 180.0 / Math.PI;
            return (lat, lon);
        }

        /// <summary>
        /// Checks whether the given EPSG:3857 coordinates are within the valid global projection domain.
        /// </summary>
        public static bool IsWithin3857Bounds(double x, double y)
        {
            return x >= -OriginShift && x <= OriginShift && y >= -OriginShift && y <= OriginShift;
        }

        // Backward-compatible aliases
        public static (double x, double y) LatLonToWebMercator(double lat, double lon) => LatLonToMeters3857(lat, lon);
        public static (double lat, double lon) WebMercatorToLatLon(double x, double y) => Meters3857ToLatLon(x, y);
    }
}
