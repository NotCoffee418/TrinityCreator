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
    /// Interaction logic for LootPage.xaml
    /// </summary>
    public partial class LootPage : Page
    {
        public LootPage()
        {
            InitializeComponent();
            AddLootRow();
        }


        public void AddLootRow()
        {
            lootRowSp.Children.Add(new LootRowControl());
        }
        public void RemoveLootRow()
        {
            if (lootRowSp.Children.Count > 1)
            lootRowSp.Children.RemoveAt(lootRowSp.Children.Count - 1);
        }

        private void addLineBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLootRow();
        }

        private void removeLineBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveLootRow();
        }

        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = EmulatorHandler.GenerateQuery(this);
                ExportQuery.ToFile(((ComboBoxItem)lootTypeCb.SelectedValue).Content.ToString() + " Loot Template " + entryTb.Text + ".sql", query);
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
                ExportQuery.ToDatabase(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
