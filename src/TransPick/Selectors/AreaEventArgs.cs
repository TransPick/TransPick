using System;
using System.Drawing;

namespace TransPick.Selectors
{
    /// <summary>
    /// An event arguments that organizes information about the capture area.
    /// </summary>
    internal class AreaEventArgs : EventArgs
    {
        /// <summary>
        /// The left upper point of the area to be captured.
        /// </summary>
        internal Point LeftUpperPoint;

        /// <summary>
        /// The size of the area to be captured.
        /// </summary>
        internal Size Size;

        /// <summary>
        /// The horizontal size of the area to be captured.
        /// </summary>
        internal int Width;

        /// <summary>
        /// The vertical size of the area to be captured.
        /// </summary>
        internal int Height;

        /// <summary>
        /// The left point of the area that will be captured.
        /// </summary>
        internal int Left;

        /// <summary>
        /// The top point of the area that will be captured.
        /// </summary>
        internal int Top;

        /// <summary>
        /// The right point of the area that will be captured.
        /// </summary>
        internal int Right;

        /// <summary>
        /// The bottom point of the area that will be captured.
        /// </summary>
        internal int Bottom;

        /// <summary>
        /// Create an instance of AreaEventArgs.
        /// </summary>
        /// <param name="leftUpperPoint">The left upper point of the area to be captured.</param>
        /// <param name="width">The horizontal size of the area to be captured.</param>
        /// <param name="height">The vertical size of the area to be captured.</param>
        public AreaEventArgs(Point leftUpperPoint, int width, int height)
        {
            LeftUpperPoint = leftUpperPoint;
            Size = new Size(width, height);

            Width = width;
            Height = height;

            Left = leftUpperPoint.X;
            Top = leftUpperPoint.Y;
            Right = leftUpperPoint.X + width;
            Bottom = leftUpperPoint.Y + height;
        }

        /// <summary>
        /// Create an instance of AreaEventArgs.
        /// </summary>
        /// <param name="leftUpperPoint">The left upper point of the area to be captured.</param>
        /// <param name="size">The size of the area to be captured.</param>
        public AreaEventArgs(Point leftUpperPoint, Size size)
        {
            LeftUpperPoint = leftUpperPoint;
            Size = size;

            Width = size.Width;
            Height = size.Height;

            Left = leftUpperPoint.X;
            Top = leftUpperPoint.Y;
            Right = leftUpperPoint.X + size.Width;
            Bottom = leftUpperPoint.Y + size.Height;
        }

        /// <summary>
        /// Create an instance of AreaEventArgs.
        /// </summary>
        /// <param name="left">The left point of the area that will be captured.</param>
        /// <param name="top">The top point of the area that will be captured.</param>
        /// <param name="right">The right point of the area that will be captured.</param>
        /// <param name="bottom">The bottom point of the area that will be captured.</param>
        public AreaEventArgs(int left, int top, int right, int bottom)
        {
            Left = left <= right ? left : right;
            Top = top <= bottom ? top : bottom;
            Right = right >= left ? right : left;
            Bottom = bottom >= top ? bottom : top;

            LeftUpperPoint = new Point(Left, Top);
            Size = new Size(Right - Left, Bottom - Top);

            Width = Right - Left;
            Height = Bottom - Top;
        }
    }
}
