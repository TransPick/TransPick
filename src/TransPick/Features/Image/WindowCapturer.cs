using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TransPick.Features.Unmanaged;

namespace TransPick.Features.Image
{
    internal static class WindowCapturer
    {
        #region ::Window Handle Related Methods::

        internal static IntPtr GetFocusedWindowHandle()
        {
            return Window.GetForegroundWindow();
        }

        internal static IntPtr GetWindowHandleByTitle(string windowTitle)
        {
            IntPtr hWnd = Window.FindWindow(null, windowTitle);
            return hWnd;
        }

        internal static bool IsWindowHandleExists(IntPtr hWnd)
        {
            if (hWnd.Equals(0))
                return false;
            else
                return true;
        }

        #endregion

        #region ::Capture Methods::

        internal static BitmapImage CaptureWindowByHandle(IntPtr hWnd)
        {
            try
            {
                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

                using (Bitmap temp = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics bitmapGraphics = Graphics.FromImage(temp);
                    IntPtr bitmapDeviceContextHandle = bitmapGraphics.GetHdc();
                    bool isSucceeded = Window.PrintWindow(hWnd, bitmapDeviceContextHandle, 3);

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

        internal static BitmapImage CaptureWindowByTitle(string windowTitle)
        {
            try
            {
                IntPtr hWnd = GetWindowHandleByTitle(windowTitle);

                if (!IsWindowHandleExists(hWnd))
                    throw new Exception($"The handle for the specified window could not be found(Title: {windowTitle}, Handle: {hWnd}).");

                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

                using (Bitmap temp = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics bitmapGraphics = Graphics.FromImage(temp);
                    IntPtr bitmapDeviceContextHandle = bitmapGraphics.GetHdc();
                    bool isSucceeded = Window.PrintWindow(hWnd, bitmapDeviceContextHandle, 3);

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

        #endregion
    }
}
