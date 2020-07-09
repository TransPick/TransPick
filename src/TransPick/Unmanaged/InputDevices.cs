using System;
using System.Drawing;
using System.Runtime.InteropServices;
using TransPick.Unmanaged.Types;

namespace TransPick.Unmanaged
{
    internal static class InputDevices
    {
        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(VKeys vKey);

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out Point lpPoint);

        internal static Point GetCursorPoint()
        {
            Point point;

            if (GetCursorPos(out point))
            {
                return point;
            }
            else
            {
                throw new InvalidOperationException("Unable to get mouse cursor position.");
            }
        }
    }
}
