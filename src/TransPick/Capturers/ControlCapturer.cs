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
    /// A class that provides control capture feature.
    /// </summary>
    class ControlCapturer
    {
        /// <summary>
        /// Captures a control under the cursor.
        /// </summary>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture()
        {
            try
            {
                // Stores the size of the control area to be captured in Rectangle.
                IntPtr hWnd = Window.WindowFromPoint(InputDevices.GetCursorPoint());

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
