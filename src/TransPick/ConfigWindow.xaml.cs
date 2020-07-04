using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransPick.Features.Image;

namespace TransPick
{
    /// <summary>
    /// ConfigWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void OnTitleBarClick(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(0);
        }

        private void OnTabClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(GeneralTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/GeneralTab.xaml", UriKind.Relative);
            }
            else if (sender.Equals(ShortCutTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/ShortcutsTab.xaml", UriKind.Relative);
            }
            else if (sender.Equals(OCRTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/OCRTab.xaml", UriKind.Relative);
            }
            else if (sender.Equals(TranslatorTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/TranslatorTab.xaml", UriKind.Relative);
            }
            else if (sender.Equals(ExtensionTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/ExtensionTab.xaml", UriKind.Relative);
            }
            else if (sender.Equals(InfoTabButton))
            {
                TabPageFrame.Source = new Uri("Pages/Config/InfoTab.xaml", UriKind.Relative);
            }
        }
    }
}
