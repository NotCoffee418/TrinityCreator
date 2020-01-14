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
using TrinityCreator.Profiles;

namespace TrinityCreator.Tools.VendorCreator
{
    /// <summary>
    /// Interaction logic for VendorPage.xaml
    /// </summary>
    public partial class VendorPage : Page
    {
        public VendorPage()
        {
            InitializeComponent();
            addItemBtn_Click(null, null); // Create an item on first load
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            vendorEntriesWp.Children.Add(new VendorEntryControl());
        }

        private void removeItemBtn_Click(object sender, RoutedEventArgs e)
        {
            vendorEntriesWp.Children.RemoveAt(vendorEntriesWp.Children.Count - 1);
        }

        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = Export.Vendor(this);
                SaveQuery.ToFile("Vendor "+ npcTb.Text + ".sql", query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void exportDbBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = Export.Vendor(this);
                SaveQuery.ToDatabase(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void npcLookupBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Creature;
        }

    }
}
