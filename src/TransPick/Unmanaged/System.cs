using System.Runtime.InteropServices;
using TransPick.Unmanaged.Types;

namespace TransPick.Unmanaged
{
    internal static class System
    {
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SystemMetric smIndex);
    }
}
