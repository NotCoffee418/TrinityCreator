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

namespace TrinityCreator.Tools.VendorCreator
{
    /// <summary>
    /// Interaction logic for VendorEntryControl.xaml
    /// </summary>
    public partial class VendorEntryControl : UserControl
    {
        public VendorEntryControl()
        {
            InitializeComponent();
        }

        public event EventHandler RemoveRequestEvent;

        private void removeMeBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveRequestEvent(this, e);
        }

        private void itemLookupBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }
    }
}
