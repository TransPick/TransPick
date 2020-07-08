using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using TransPick.Unmanaged;
using TransPick.Unmanaged.Types;

namespace TransPick.Capturers
{
    /// <summary>
    /// A class that provide window capture features.
    /// </summary>
    internal static class WindowCapturer
    {
        /// <summary>
        /// Captures a specified application window by handle.
        /// </summary>
        /// <param name="hWnd">The handle of the window to capture.</param>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture(IntPtr hWnd)
        {
            try
            {
                // Stores the size of the window area to be captured in Rectangle.
                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics bitmapGraphics = Graphics.FromImage(temp);
                    IntPtr bitmapDeviceContextHandle = bitmapGraphics.GetHdc();
                    bool isSucceeded = Window.PrintWindow(hWnd, bitmapDeviceContextHandle, RegionFlags.COMPLEXREGION);

                    bitmapGraphics.ReleaseHdc(bitmapDeviceContextHandle);

                    if (!isSucceeded)
                    {
                        bitmapGraphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, temp.Size));
                    }

                    IntPtr hRgn = Gdi.CreateRectRgn(0, 0, 0, 0);
                    Window.GetWindowRgn(hWnd, hRgn);
                    Region region = Region.FromHrgn(hRgn);
                    if (!region.IsEmpty(bitmapGraphics))
                    {
                        bitmapGraphics.ExcludeClip(region);
                        bitmapGraphics.Clear(Color.Transparent);
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

                    bitmapGraphics.Dispose();
                }

                return bitmap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Captures a specified application window by title.
        /// </summary>
        /// <param name="windowTitle">The title of the window to capture.</param>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture(string windowTitle)
        {
            try
            {
                // Stores the size of the window area to be captured in Rectangle.
                IntPtr hWnd = Window.FindWindow(null, windowTitle);

                if (!Window.IsWindowHandleExists(hWnd))
                    throw new Exception($"The handle for the specified window could not be found(Title: {windowTitle}, Handle: {hWnd}).");

                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics bitmapGraphics = Graphics.FromImage(temp);
                    IntPtr bitmapDeviceContextHandle = bitmapGraphics.GetHdc();
                    bool isSucceeded = Window.PrintWindow(hWnd, bitmapDeviceContextHandle, RegionFlags.COMPLEXREGION);

                    bitmapGraphics.ReleaseHdc(bitmapDeviceContextHandle);

                    if (!isSucceeded)
                    {
                        bitmapGraphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, temp.Size));
                    }

                    IntPtr hRgn = Gdi.CreateRectRgn(0, 0, 0, 0);
                    Window.GetWindowRgn(hWnd, hRgn);
                    Region region = Region.FromHrgn(hRgn);
                    if (!region.IsEmpty(bitmapGraphics))
                    {
                        bitmapGraphics.ExcludeClip(region);
                        bitmapGraphics.Clear(Color.Transparent);
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

                    bitmapGraphics.Dispose();
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
