using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
using TransPick.Selectors;
using TransPick.Unmanaged;
using TransPick.Utilities;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace TransPick.Widgets
{
    /// <summary>
    /// CaptureToolsWidget.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CaptureToolsWidget : System.Windows.Window
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
        }
    }
}
