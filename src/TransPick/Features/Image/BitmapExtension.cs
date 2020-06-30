using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TransPick.Entities.Enums;

namespace TransPick.Features.Image
{
    internal static class BitmapExtension
    {
        #region ::Extension Methods::

        internal static void Save(this BitmapImage image, BitmapFormat bitmapFormat, string filePath)
        {
            if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute))
                throw new UriFormatException($"Invalid format of file path({filePath}).");

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            BitmapEncoder encoder = new PngBitmapEncoder();

            if (bitmapFormat == BitmapFormat.Bmp)
                encoder = new BmpBitmapEncoder();
            else if (bitmapFormat == BitmapFormat.Gif)
                encoder = new GifBitmapEncoder();
            else if (bitmapFormat == BitmapFormat.Jpeg)
                encoder = new JpegBitmapEncoder();
            else if (bitmapFormat == BitmapFormat.Png)
                encoder = new PngBitmapEncoder();
            else if (bitmapFormat == BitmapFormat.Tiff)
                encoder = new TiffBitmapEncoder();
            else if (bitmapFormat == BitmapFormat.Wmp)
                encoder = new WmpBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
                stream.Close();
            }
        }

        #endregion
    }
}
