using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransPick.Capturers;
using TransPick.Capturers.Types;
using TransPick.Overlays;
using TransPick.Selectors;
using TransPick.Unmanaged;
using TransPick.Utilities;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Window = System.Windows.Window;

namespace TransPick.Widgets
{
    /// <summary>
    /// CaptureToolsWidget.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CaptureToolsWidget : Window
    {
        public CaptureToolsWidget()
        {
            InitializeComponent();
            SetWindowPosition();
        }

        private void SetWindowPosition()
        {
            Screen screen = Screen.FromPoint(InputDevices.GetCursorPoint());

            Left = screen.Bounds.Left + (screen.Bounds.Width / 2) - ((int)Width / 2);
            Top = screen.Bounds.Top + 20;
        }

        private void OnWidgetClick(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            StartAreaSelector();
        }

        private void StartAreaSelector()
        {
            AreaSelector selector = new AreaSelector();
            selector.AreaSelectedEvent += new AreaSelector.AreaSelectedEventHandler(() =>
            {
                selector.Stop();

                if (selector.GetFirstPoint().X <= selector.GetSecondPoint().X)
                {
                    AreaCapturer.Capture(PropertyCalculator.GetLeftUpperPoint(selector.GetFirstPoint(), selector.GetSecondPoint()), new Size(PropertyCalculator.GetWidth(selector.GetFirstPoint().X, selector.GetSecondPoint().X), PropertyCalculator.GetWidth(selector.GetFirstPoint().Y, selector.GetSecondPoint().Y))).Save(@"E:\test.png", BitmapFormat.Png);
                }
            });

            selector.Start();
        }
    }
}
