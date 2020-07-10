using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using TransPick.Unmanaged;
using TransPick.Unmanaged.Types;
using Window = System.Windows.Window;

namespace TransPick.Selectors
{
    internal class AreaSelector
    {
        Window _blind;
        IntPtr _blindHandle;

        Timer _blindTimer = new Timer(10);

        internal AreaSelector()
        {
            // Initializes blind window.
            _blind = new Window();
            _blind.Background = new SolidColorBrush(Color.FromArgb((byte)0.1f, 0, 0, 0));

            _blind.Left = Display.GetLeft();
            _blind.Top = Display.GetTop();

            _blind.Width = Display.GetWidth();
            _blind.Height = Display.GetHeight();

            _blind.MouseLeftButtonDown += OnMouseLeftButtonDown;
            _blind.MouseLeftButtonUp += OnMouseLeftButtonUp;

            // Sets blind window handle.
            _blindHandle = new WindowInteropHelper(_blind).Handle;

            // Starts Blind timer.
            _blindTimer.Elapsed += OnBlindTimerTick;
            _blindTimer.Enabled = true;
            _blindTimer.Start();
        }

        private void OnBlindTimerTick(object sender, ElapsedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Debug.WriteLine("WM_LBTN_DOWN");
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
