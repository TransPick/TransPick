﻿using System;
using System.Runtime.InteropServices;
using TransPick.Unmanaged.Types;

namespace TransPick.Unmanaged
{
    internal static class Display
    {
        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        internal delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        internal static bool IsMultiMonitorSupport()
        {
            if (System.GetSystemMetrics(SystemMetric.SM_CMONITORS) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static bool IsSameDisplayFormat()
        {
            if (System.GetSystemMetrics(SystemMetric.SM_SAMEDISPLAYFORMAT) != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static int GetMonitorCount()
        {
            return System.GetSystemMetrics(SystemMetric.SM_CMONITORS);
        }

        internal static int GetWidth()
        {
            int left = System.GetSystemMetrics(SystemMetric.SM_XVIRTUALSCREEN);
            int right = System.GetSystemMetrics(SystemMetric.SM_CXVIRTUALSCREEN);
            return Math.Abs(left) + Math.Abs(right);
        }

        internal static int GetHeight()
        {
            int top = System.GetSystemMetrics(SystemMetric.SM_YVIRTUALSCREEN);
            int bottom = System.GetSystemMetrics(SystemMetric.SM_CYVIRTUALSCREEN);
            return Math.Abs(top) + Math.Abs(bottom);
        }

        internal static int GetLeft()
        {
            return System.GetSystemMetrics(SystemMetric.SM_XVIRTUALSCREEN);
        }

        internal static int GetTop()
        {
            return System.GetSystemMetrics(SystemMetric.SM_YVIRTUALSCREEN);
        }

        internal static int GetRight()
        {
            return System.GetSystemMetrics(SystemMetric.SM_CXVIRTUALSCREEN);
        }

        internal static int GetBottom()
        {
            return System.GetSystemMetrics(SystemMetric.SM_CYVIRTUALSCREEN);
        }
    }
}
