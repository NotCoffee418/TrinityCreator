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
using TrinityCreator.Database;
using System.Data;
using System.Threading;
using System.Windows.Threading;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for LookupTool.xaml
    /// </summary>
    public partial class LookupTool : UserControl
    {
        public LookupTool()
        {
            InitializeComponent();
            Target = "Find item by name";
        }

        string _target = "Searching in...";    

        /// <summary>
        /// Target query
        /// </summary>
        public string Target {
            get { return _target; }
            set
            {
                targetLbl.Content = value;
                _target = value;
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            searchBtn.IsEnabled = false;

            // clear
            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;

            // Show searching message
            DataGridTextColumn col = new DataGridTextColumn();
            col.Header = "Searching...";
            dataGrid.Columns.Add(col);

            string search = searchTxt.Text;
            SetResults(search);
        }

        private void SetResults(string search)
        {
            Connection.Open();
            switch (Target)
            {
                case "Find item by name":
                    SetGridSource(ItemTemplateQuery.FindItemsByName(search));
                    break;

                default:
                    DataGridTextColumn col = new DataGridTextColumn();
                    col.Header = "Error. Press a find button before searching.";
                    dataGrid.Columns.Add(col);
                    break;
            }
        }

        /// <summary>
        /// Set the grid's itemssource from another thread
        /// </summary>
        /// <param name="dt"></param>
        private void SetGridSource(DataTable dt)
        {
            // clear
            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;

            // set 
            dataGrid.ItemsSource = dt.DefaultView;
            searchBtn.IsEnabled = true;
        }
    }
}
