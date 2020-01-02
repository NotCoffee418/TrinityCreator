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
using TrinityCreator.Emulator;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for VendorPage.xaml
    /// </summary>
    public partial class VendorPage : Page
    {
        public VendorPage()
        {
            InitializeComponent();
        }

        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = EmulatorHandler.GenerateQuery(this);
                Query.ToFile("Vendor"+ npcTb.Text + " Item " + itemTb.Text + ".sql", query);
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
                string query = EmulatorHandler.GenerateQuery(this);
                Query.ToDatabase(query);
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

        private void itemLookupBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }
    }
}
