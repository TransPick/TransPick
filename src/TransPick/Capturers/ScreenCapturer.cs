using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TransPick.Unmanaged;

namespace TransPick.Capturers
{
    /// <summary>
    /// A class that provide screen capture features.
    /// </summary>
    internal static class ScreenCapturer
    {
        /// <summary>
        /// Capture all available displays.
        /// </summary>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture()
        {
            try
            {
                int width = Display.GetWidth();
                int height = Display.GetHeight();

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    // Creates a Graphics instance to modify bitmap image.
                    using (Graphics graphics = Graphics.FromImage(temp))
                    {
                        // Captures and saves the screen.
                        graphics.CopyFromScreen(Display.GetLeft(), Display.GetTop(), 0, 0, temp.Size);
                    }

                    // To send a bitmap to BitmapImage instance, save it to Memory Stream.
                    using (MemoryStream memory = new MemoryStream())
                    {
                        temp.Save(memory, ImageFormat.Bmp);
                        memory.Position = 0;

                        bitmap.BeginInit();
                        bitmap.StreamSource = memory;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                    }
                }

                return bitmap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Captures the specified display.
        /// </summary>
        /// <param name="screen">Specifies the display to capture.</param>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture(Screen screen)
        {
            try
            {
                if (screen == null)
                    throw new NullReferenceException();

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb))
                {
                    // Creates a Graphics instance to modify bitmap image.
                    using (Graphics graphics = Graphics.FromImage(temp))
                    {
                        // Captures and saves the screen.
                        graphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, temp.Size);
                    }

                    // To send a bitmap to BitmapImage instance, save it to Memory Stream.
                    using (MemoryStream memory = new MemoryStream())
                    {
                        temp.Save(memory, ImageFormat.Bmp);
                        memory.Position = 0;

                        bitmap.BeginInit();
                        bitmap.StreamSource = memory;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                    }
                }

                return bitmap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
