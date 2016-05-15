using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using CefSharp;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        public Browser(string url) : this()
        {
            LoadUrl(url);
        }
        public Browser()
        {
            InitializeComponent();
            Chromium.BrowserSettings = new BrowserSettings()
            {
                OffScreenTransparentBackground = false
            };
        }

        public void LoadUrl(string url)
        {
            try
            {
                Chromium.Address = url;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to load URL", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}
