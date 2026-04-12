
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

namespace TrainzBasemapMaker
{
    // a class that uses the ProjNET library to convert to the EPSG:2180 coordinate system
    internal class GeoHelperEPSG2180
    {
        private static readonly CoordinateTransformationFactory ctfFac = new CoordinateTransformationFactory();
        private static readonly CoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
        private static readonly CoordinateSystem epsg2180;
        private static readonly MathTransform transformTo2180;
        private static readonly MathTransform transformToWgs84;

        static GeoHelperEPSG2180()
        {
            string wkt2180 = @"PROJCS[""ETRS89 / Poland CS92"",GEOGCS[""ETRS89"",DATUM[""European_Terrestrial_Reference_System_1989"",SPHEROID[""GRS 1980"",6378137,298.257222101,AUTHORITY[""EPSG"",""7019""]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[""EPSG"",""6258""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4258""]],PROJECTION[""Transverse_Mercator""],PARAMETER[""latitude_of_origin"",0],PARAMETER[""central_meridian"",19],PARAMETER[""scale_factor"",0.9993],PARAMETER[""false_easting"",500000],PARAMETER[""false_northing"",-5300000],UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],AUTHORITY[""EPSG"",""2180""]]";
            var csFac = new CoordinateSystemFactory();
            epsg2180 = csFac.CreateFromWkt(wkt2180);

            transformTo2180 = ctfFac.CreateFromCoordinateSystems(wgs84, epsg2180).MathTransform;
            transformToWgs84 = ctfFac.CreateFromCoordinateSystems(epsg2180, wgs84).MathTransform;
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
    }
}
