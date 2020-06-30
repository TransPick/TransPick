using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TransPick.Entities.Enums;

namespace TransPick.Features.Unmanaged
{
    internal static class System
    {
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SystemMetric smIndex);
    }
}
