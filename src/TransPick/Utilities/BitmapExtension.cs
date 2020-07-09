using System;
using System.IO;
using System.Windows.Media.Imaging;
using TransPick.Capturers.Types;

namespace TransPick.Utilities
{
    /// <summary>
    /// The class that implements the extended method of the BitmapImage class.
    /// </summary>
    internal static class BitmapExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image">BitmapImage instance to save.</param>
        /// <param name="filePath">The path where the file will be stored.</param>
        /// <param name="bitmapFormat">Format for saving files.</param>
        internal static void Save(this BitmapImage image, string filePath, BitmapFormat bitmapFormat)
        {
            // Uri format checking.
            if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute))
            {
                throw new UriFormatException($"Invalid format of file path({filePath}).");
            }

            // If the file directory does not exist, creates the directory.
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            try
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                // Sets the file format.
                if (bitmapFormat == BitmapFormat.Bmp)
                {
                    encoder = new BmpBitmapEncoder();
                }
                else if (bitmapFormat == BitmapFormat.Gif)
                {
                    encoder = new GifBitmapEncoder();
                }
                else if (bitmapFormat == BitmapFormat.Jpeg)
                {
                    encoder = new JpegBitmapEncoder();
                }
                else if (bitmapFormat == BitmapFormat.Png)
                {
                    encoder = new PngBitmapEncoder();
                }
                else if (bitmapFormat == BitmapFormat.Tiff)
                {
                    encoder = new TiffBitmapEncoder();
                }
                else if (bitmapFormat == BitmapFormat.Wmp)
                {
                    encoder = new WmpBitmapEncoder();
                }

                encoder.Frames.Add(BitmapFrame.Create(image));

                // Save BitmapImage using file stream.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Save(stream);
                    stream.Close();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
