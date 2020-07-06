using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TransPick.Entities.Enums;
using TransPick.Features.Unmanaged;

namespace TransPick.Features.Image
{
    internal static class ImageCapturer
    {
        #region ::Screen Capturer::

        internal static BitmapImage CaptureAllScreens()
        {
            int width = Monitor.GetWidth();
            int height = Monitor.GetHeight();

            BitmapImage bitmap = new BitmapImage();

            using (Bitmap temp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                using (Graphics graphics = Graphics.FromImage(temp))
                {
                    // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                    graphics.CopyFromScreen(Monitor.GetLeft(), Monitor.GetTop(), 0, 0, temp.Size);
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
            }

            return bitmap;
        }

        internal static BitmapImage CaptureSelectedScreen(Screen screen)
        {
            if (screen == null)
                throw new NullReferenceException();

            BitmapImage bitmap = new BitmapImage();

            using (Bitmap temp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb))
            {
                // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                using (Graphics graphics = Graphics.FromImage(temp))
                {
                    // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                    graphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, temp.Size);
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
            }

            return bitmap;
        }

        #endregion

        #region ::Window Capturer::

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
                IntPtr hWnd = Window.FindWindow(null, windowTitle);

                if (!Window.IsWindowHandleExists(hWnd))
                    throw new Exception($"The handle for the specified window could not be found(Title: {windowTitle}, Handle: {hWnd}).");

                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

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

        #region ::Control Capturer::

        internal static BitmapImage CaptureControl()
        {
            try
            {
                IntPtr hWnd = Window.WindowFromPoint(Unmanaged.Cursor.GetCursorPoint());

                Rectangle rectangle = Rectangle.Empty;
                Graphics windowGraphics = Graphics.FromHwnd(hWnd);
                rectangle = Rectangle.Round(windowGraphics.VisibleClipBounds);

                BitmapImage bitmap = new BitmapImage();

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

        #region ::Area Capturer::

        internal static BitmapImage CaptureArea(Point leftUpperPoint, Size size)
        {
            int left = Monitor.GetLeft();
            int top = Monitor.GetTop();
            int right = Monitor.GetRight();
            int bottom = Monitor.GetBottom();

            if (leftUpperPoint.X < left || leftUpperPoint.Y < top || leftUpperPoint.X > right || leftUpperPoint.Y > bottom)
                throw new ArgumentOutOfRangeException($"The specified LeftUpperPoint is out of screen range(Input: {leftUpperPoint.X}, {leftUpperPoint.Y}, Minimum: {left}, {top}, Maximum: {right}, {bottom}).");

            if (leftUpperPoint.X + size.Width > Monitor.GetWidth())
                throw new ArgumentOutOfRangeException($"The horizontal size of the specified area exceeds the screen range(Input: {size.Width}, Maximum: {Monitor.GetWidth()}).");

            if (leftUpperPoint.Y + size.Height > Monitor.GetHeight())
                throw new ArgumentOutOfRangeException($"The vertical size of the specified area exceeds the screen range(Input: {size.Height}, Maximum: {Monitor.GetHeight()}).");

            BitmapImage bitmap = new BitmapImage();

            using (Bitmap temp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb))
            {
                // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                using (Graphics graphics = Graphics.FromImage(temp))
                {
                    // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                    graphics.CopyFromScreen(leftUpperPoint.X, leftUpperPoint.Y, 0, 0, temp.Size);
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

                temp.Dispose();
            }

            return bitmap;
        }

        #endregion
    }
}
