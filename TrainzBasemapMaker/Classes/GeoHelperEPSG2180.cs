
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

// This class utilizes the ProjNET library (version 2.1.0) 
// for coordinate system transformations.
// ProjNET is authored by Morten Nielsen and the NetTopologySuite-Team
// and is licensed under the GNU Lesser General Public License v2.1.

using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace TrainzBasemapMaker.Classes
{
    // A class that handles coordinate system transformations (EPSG:2180 for Poland and EPSG:3857 worldwide)
    internal class GeoHelperEPSG2180
    {
        private static readonly CoordinateTransformationFactory ctfFac = new CoordinateTransformationFactory();
        private static readonly CoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
        private static readonly CoordinateSystem epsg2180;
        private static readonly MathTransform transformTo2180;
        private static readonly MathTransform transformToWgs84;

        private const double OriginShift = 20037508.342789244;

        static GeoHelperEPSG2180()
        {
            string wkt2180 = @"PROJCS[""ETRS89 / Poland CS92"",GEOGCS[""ETRS89"",DATUM[""European_Terrestrial_Reference_System_1989"",SPHEROID[""GRS 1980"",6378137,298.257222101,AUTHORITY[""EPSG"",""7019""]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[""EPSG"",""6258""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4258""]],PROJECTION[""Transverse_Mercator""],PARAMETER[""latitude_of_origin"",0],PARAMETER[""central_meridian"",19],PARAMETER[""scale_factor"",0.9993],PARAMETER[""false_easting"",500000],PARAMETER[""false_northing"",-5300000],UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],AUTHORITY[""EPSG"",""2180""]]";
            var csFac = new CoordinateSystemFactory();
            epsg2180 = csFac.CreateFromWkt(wkt2180);

            transformTo2180 = ctfFac.CreateFromCoordinateSystems(wgs84, epsg2180).MathTransform;
            transformToWgs84 = ctfFac.CreateFromCoordinateSystems(epsg2180, wgs84).MathTransform;
        }

        public static bool IsWithinPolandBounds(double lat, double lon)
        {
            return lat >= 48.0 && lat <= 56.5 && lon >= 13.5 && lon <= 25.5;
        }

        public static bool IsWithin2180Bounds(double x, double y)
        {
            return x >= 100000.0 && x <= 950000.0 && y >= 100000.0 && y <= 900000.0;
        }

        public static (double x, double y) LatLonToMeters2180(double lat, double lon)
        {
            double[] from = new double[] { lon, lat };
            double[] to = transformTo2180.Transform(from);
            return (to[0], to[1]);
        }

        public static (double lat, double lon) Meters2180ToLatLon(double x, double y)
        {
            double[] from = new double[] { x, y };
            double[] to = transformToWgs84.Transform(from);
            return (to[1], to[0]);
        }

        public static (double x, double y) LatLonToWebMercator(double lat, double lon)
        {
            double x = lon * OriginShift / 180.0;
            double clampedLat = Math.Clamp(lat, -85.05112878, 85.05112878);
            double latRad = clampedLat * Math.PI / 180.0;
            double y = Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0)) * OriginShift / Math.PI;
            return (x, y);
        }

        public static (double lat, double lon) WebMercatorToLatLon(double x, double y)
        {
            double lon = x * 180.0 / OriginShift;
            double lat = (2.0 * Math.Atan(Math.Exp(y * Math.PI / OriginShift)) - Math.PI / 2.0) * 180.0 / Math.PI;
            return (lat, lon);
        }

        public static (double x, double y) LatLonToMeters(double lat, double lon)
        {
            if (IsWithinPolandBounds(lat, lon))
            {
                return LatLonToMeters2180(lat, lon);
            }
            return LatLonToWebMercator(lat, lon);
        }

        public static (double lat, double lon) MetersToLatLon(double x, double y)
        {
            if (IsWithin2180Bounds(x, y))
            {
                return Meters2180ToLatLon(x, y);
            }
            return WebMercatorToLatLon(x, y);
        }
    }
}
