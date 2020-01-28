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

namespace TrinityCreator.UI.UIElements
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        // Don't use cefsharp. Default browser control might work in the future, who knows.
        public Browser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blankFirst">modelViewer requires blank page first because js</param>
        public void LoadUrl(string url, bool blankFirst = false)
        {
            //System.Diagnostics.Process.Start(url);
            webViewControl.Source = new Uri(url);
            webViewControl.BringIntoView();
        }
    }
}
