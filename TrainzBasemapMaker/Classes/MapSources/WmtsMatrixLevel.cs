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
    /// Represents a specific TileMatrix level definition in a WMTS TileMatrixSet.
    /// </summary>
    internal class WmtsMatrixLevel
    {
        public string Identifier { get; }
        public double ScaleDenominator { get; }
        public double PixelSize => ScaleDenominator * 0.00028;

        public WmtsMatrixLevel(string identifier, double scaleDenominator)
        {
            Identifier = identifier;
            ScaleDenominator = scaleDenominator;
        }

        public override string ToString() => $"{Identifier} (Scale: {ScaleDenominator:F2}, {PixelSize:F4}m/px)";
    }
}