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
using TrinityCreator.DBC;

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
            App.LookupTool = this;
        }

        string _target = "Searching in...";    

        /// <summary>
        /// Target query
        /// </summary>
        public string Target {
            get { return _target; }
            set
            {
                _target = value;
                targetLbl.Content = value;
                HandleDbcQuery();
            }
        }

        public DataTable FullDbcTable { get; set; }

        public bool LocalSearch { get; set; }

        /// <summary>
        /// DBC definitions
        /// </summary>
        /// <param name="target"></param>
        private void HandleDbcQuery()
        {
            LocalSearch = true;
            DataTable result = null;
            switch (Target)
            {
                case "Find quest sort":
                    result = Queries.GetQuestSort();
                    break;

                // SQL query will be handled in searchBtn_Click
                default:
                    LocalSearch = false;
                    break;
            }

            // Copy DataTable when DBC
            if (LocalSearch)
            {
                FullDbcTable = result.Copy();
                SetGridSource(result);
            }
        }

        /// <summary>
        /// SQL Definitions
        /// </summary>
        /// <param name="search"></param>
        private void HandleSqlQuery(string search)
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


        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            searchBtn.IsEnabled = false;
            string search = searchTxt.Text;

            if (LocalSearch)
            {
                DataTable filteredTable = new DataTable();
                foreach (DataColumn dc in FullDbcTable.Columns)
                    filteredTable.Columns.Add(dc.ColumnName, dc.DataType);

                List<DataRow> filteredRows = new List<DataRow>();
                foreach (DataRow row in FullDbcTable.Rows)
                {
                    for (int i = 0; i < FullDbcTable.Columns.Count; i++)
                        if (row[i].ToString().ToLower().Contains(search.ToLower()))
                        {
                            filteredTable.ImportRow(row);
                            break;
                        }
                }
                SetGridSource(filteredTable);
            }
            else // Query SQL
            {
                // clear
                dataGrid.Columns.Clear();
                dataGrid.ItemsSource = null;

                // Show searching message
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = "Searching...";
                dataGrid.Columns.Add(col);
                HandleSqlQuery(search);
            }
        }

        /// <summary>
        /// Set the grid's itemssource
        /// </summary>
        /// <param name="dt"></param>
        private void SetGridSource(DataTable dt)
        {
            // clear
            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = null;

            if (dt.Rows.Count > 200) // Limit to reduce lag
            {
                // import 200 rows
                DataTable limited = new DataTable();
                foreach (DataColumn dc in FullDbcTable.Columns)
                    limited.Columns.Add(dc.ColumnName, dc.DataType);
                for (int i = 0; i < 200; i++)
                    limited.ImportRow(dt.Rows[i]);

                // mesage
                DataRow info = limited.NewRow();
                info[1] = "Limited to 200 results. Search to filter.";
                limited.Rows.Add(info);

                // bind
                dataGrid.ItemsSource = limited.DefaultView;
            }
            else dataGrid.ItemsSource = dt.DefaultView;
            searchBtn.IsEnabled = true;
        }
    }
}
