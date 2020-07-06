using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TransPick.Entities.Enums;

namespace TransPick.Features.Unmanaged
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
                return point;
            else
                throw new Exception("Unable to get mouse pointer position.");
        }
    }
}
