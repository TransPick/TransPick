using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using TransPick.Unmanaged;

namespace TransPick.Capturers
{
    /// <summary>
    /// A class that provides area capture features.
    /// </summary>
    internal static class AreaCapturer
    {
        /// <summary>
        /// Captures the specified area by leftUpperPoint and size.
        /// </summary>
        /// <param name="leftUpperPoint">The left upper point of the area to be captured.</param>
        /// <param name="size">The size of the area to be captured.</param>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture(Point leftUpperPoint, Size size)
        {
            try
            {
                // Specify the screen boundary limit values.
                int leftLimit = Display.GetLeft();
                int topLimit = Display.GetTop();
                int rightLimit = Display.GetRight();
                int bottomLimit = Display.GetBottom();

                if (leftUpperPoint.X < leftLimit || leftUpperPoint.Y < topLimit || leftUpperPoint.X > rightLimit || leftUpperPoint.Y > bottomLimit)
                {
                    throw new ArgumentOutOfRangeException($"The specified LeftUpperPoint is out of screen range(Input: {leftUpperPoint.X}, {leftUpperPoint.Y}, Minimum: {leftLimit}, {topLimit}, Maximum: {rightLimit}, {bottomLimit}).");
                }

                if (leftUpperPoint.X + size.Width > Display.GetWidth())
                {
                    throw new ArgumentOutOfRangeException($"The horizontal size of the specified area exceeds the screen range(Input: {size.Width}, Maximum: {Display.GetWidth()}).");
                }

                if (leftUpperPoint.Y + size.Height > Display.GetHeight())
                {
                    throw new ArgumentOutOfRangeException($"The vertical size of the specified area exceeds the screen range(Input: {size.Height}, Maximum: {Display.GetHeight()}).");
                }

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb))
                {
                    // Creates a Graphics instance to modify bitmap image.
                    using (Graphics graphics = Graphics.FromImage(temp))
                    {
                        // Captures and saves the area from the screen.
                        graphics.CopyFromScreen(leftUpperPoint.X, leftUpperPoint.Y, 0, 0, temp.Size);
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

                    temp.Dispose();
                }

                return bitmap;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Captures the specified area by points.
        /// </summary>
        /// <param name="left">The left point of the area that will be captured.</param>
        /// <param name="top">The top point of the area that will be captured.</param>
        /// <param name="right">The right point of the area that will be captured.</param>
        /// <param name="bottom">The bottom point of the area that will be captured.</param>
        /// <returns>Captured bitmap image.</returns>
        internal static BitmapImage Capture(int left, int top, int right, int bottom)
        {
            try
            {
                // Specify the screen boundary limit values.
                int leftLimit = Display.GetLeft();
                int topLimit = Display.GetTop();
                int rightLimit = Display.GetRight();
                int bottomLimit = Display.GetBottom();

                if (left < leftLimit || top < topLimit || right > rightLimit || bottom > bottomLimit)
                {
                    throw new ArgumentOutOfRangeException($"The specified LeftUpperPoint is out of screen range(Input(LTRB): {left}, {top}, {right}, {bottom}, Minimum: {leftLimit}, {topLimit}, Maximum: {rightLimit}, {bottomLimit}).");
                }

                if (right - left > Display.GetWidth())
                {
                    throw new ArgumentOutOfRangeException($"The horizontal size of the specified area exceeds the screen range(Input: {right - left}, Maximum: {Display.GetWidth()}).");
                }

                if (bottom - top > Display.GetHeight())
                {
                    throw new ArgumentOutOfRangeException($"The vertical size of the specified area exceeds the screen range(Input: {bottom - top}, Maximum: {Display.GetHeight()}).");
                }

                BitmapImage bitmap = new BitmapImage();

                // Creates an instance to temporarily store the bitmap.
                using (Bitmap temp = new Bitmap(right - left, bottom - top, PixelFormat.Format32bppArgb))
                {
                    // Creates a Graphics instance to modify bitmap image.
                    using (Graphics graphics = Graphics.FromImage(temp))
                    {
                        // Captures and saves the area from the screen.
                        graphics.CopyFromScreen(left, top, 0, 0, temp.Size);
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

                    temp.Dispose();
                }

                return bitmap;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
