using System;
using System.Runtime.InteropServices;

namespace TransPick.Unmanaged
{
    internal static class Gdi
    {
        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
    }
}
