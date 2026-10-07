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

using System.Drawing;
using System.Drawing.Imaging;

namespace TrainzBasemapMaker.Classes
{
    /// <summary>
    /// Shared helpers for bitmap creation and JPEG encoding.
    /// </summary>
    internal static class ImageHelpers
    {
        /// <summary>
        /// JPEG quality used for all re-encoded basemaps (GDI+ default is ~75).
        /// </summary>
        public const long JpegQuality = 90L;

        private static readonly ImageCodecInfo? JpegCodec =
            ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

        /// <summary>
        /// Creates an opaque 24bpp bitmap. JPEG has no alpha channel, so 24bpp uses 25% less memory
        /// than the default 32bpp ARGB format and is faster to draw into.
        /// </summary>
        public static Bitmap CreateRgbBitmap(int width, int height)
        {
            return new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Encodes the image as JPEG with an explicit quality setting.
        /// </summary>
        public static byte[] EncodeJpeg(Image image, long quality = JpegQuality)
        {
            using var ms = new MemoryStream();
            if (JpegCodec != null)
            {
                using var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                image.Save(ms, JpegCodec, encoderParams);
            }
            else
            {
                image.Save(ms, ImageFormat.Jpeg);
            }
            return ms.ToArray();
        }
    }
}
