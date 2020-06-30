using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TransPick.Entities.Enums;
using TransPick.Entities.Structs;
using TransPick.Features.Unmanaged;

namespace TransPick.Features.Image
{
    internal static class FullScreenCapturer
    {
        #region ::Capture Methods::

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
    }
}
