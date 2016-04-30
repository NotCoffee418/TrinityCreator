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
                if (_target != value)
                    SetGridSource(new DataTable());
                _target = value;
                targetLbl.Content = value;
                HandleDbcQuery();
                App._MainWindow.ShowLookupTool();
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

                case "Find map":
                    result = Queries.GetMap();
                    break;

                case "Find faction":
                    result = Queries.GetFaction();
                    break;

                case "Find player title":
                    result = Queries.GetCharTitles();
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
                    SetGridSource(SqlQuery.FindItemsByName(search));
                    break;
                case "Find quest by name":
                    SetGridSource(SqlQuery.FindQuestByName(search));
                    break;
                case "Find creature by name":
                    SetGridSource(SqlQuery.FindCreatureByName(search));
                    break;
                case "Find game object by name":
                    SetGridSource(SqlQuery.FindGoByName(search));
                    break;
                case "Find spell by name": // Combined SQL & DBC
                    SetGridSource(FindSpell(search));
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
                if (Target == "Find spell by name") // hybrid
                {
                    FindSpell(search);
                }

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
                foreach (DataColumn dc in dt.Columns)
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


        #region Special queries
        private DataTable FindSpell(string search)
        {
            LocalSearch = true;
            DataTable result = new DataTable();
            List<uint> listed = new List<uint>();
            result.Columns.Add("ID", typeof(int));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("Description", typeof(string));

            // Get data
            DataTable dbc = Queries.GetSpells(); // "m_ID", "m_name_lang_1", "m_description_lang_1"
            DataTable sql = SqlQuery.GetSpells(search); // Id, Comment(name)

            // Add DBC spells
            DataRow newRow = null;
            foreach (DataRow dr in dbc.Rows)
                if (dr["m_name_lang_1"].ToString().Contains(search))
                {
                    newRow = result.NewRow();
                    newRow["ID"] = dr["m_ID"];
                    newRow["Name"] = dr["m_name_lang_1"];
                    newRow["Description"] = dr["m_description_lang_1"];
                    result.Rows.Add(newRow);
                    listed.Add((uint)dr["m_ID"]);
                }

            // Add unlisted SQL spells
            foreach (DataRow dr in sql.Rows)
                if (!listed.Contains((uint)dr["Id"]))
                {
                    newRow = result.NewRow();
                    newRow["ID"] = dr["Id"];
                    newRow["Name"] = dr["Comment"];
                    result.Rows.Add(newRow);
                }

            FullDbcTable = result.Copy();
            return result;
        }
        #endregion
    }
}
