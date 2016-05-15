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
using System.Windows.Forms.Integration;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        public BrowserControl ChromiumControl { get; private set; }

        public Browser(string url) : this()
        {
            LoadUrl(url);
        }
        public Browser()
        {
            InitializeComponent();

            // might need this in loaded event
            // Use winforms because webgl issues in wpf
            ChromiumControl = new BrowserControl("about:blank");
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = ChromiumControl;
            HostGrid.Children.Add(host);
        }

        /// <summary>
        /// Load url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blankFirst">modelViewer requires blank page first because js</param>
        public void LoadUrl(string url, bool blankFirst = false)
        {
            try
            {
                if (blankFirst)
                    ChromiumControl.Chromium.Load("about:blank");
                ChromiumControl.Chromium.Load(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to load URL", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}
