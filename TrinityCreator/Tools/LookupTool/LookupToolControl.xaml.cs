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
using TrinityCreator.Profiles;

namespace TrinityCreator.Tools.LookupTool
{
    /// <summary>
    /// Interaction logic for LookupTool.xaml
    /// </summary>
    public partial class LookupToolControl : UserControl
    {
        public LookupToolControl()
        {
            InitializeComponent();
            App.LookupTool = this;
        }

        Target _target;

        /// <summary>
        /// Target query
        /// </summary>
        public Target SelectedTarget {
            get { return _target; }
            set
            {
                if (_target != value)
                    SetGridSource(new DataTable());
                _target = value;
                targetSelectCb.SelectedIndex = (int)value;
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
            switch (SelectedTarget)
            {
                case Target.QuestSort:
                    result = DBCQuery.GetQuestSort();
                    break;

                case Target.Map:
                    result = DBCQuery.GetMap();
                    break;

                case Target.Faction:
                    result = DBCQuery.GetFaction();
                    break;

                case Target.Title:
                    result = DBCQuery.GetCharTitles();
                    break;
                case Target.Emotes:
                    result = DBCQuery.GetEmotes();
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
            switch (SelectedTarget)
            {
                case Target.Item:
                    SetGridSource(LookupQuery.FindItemsByName(search));
                    break;
                case Target.Quest:
                    SetGridSource(LookupQuery.FindQuestByName(search));
                    break;
                case Target.Creature:
                    SetGridSource(LookupQuery.FindCreatureByName(search));
                    break;
                case Target.GameObject:
                    SetGridSource(LookupQuery.FindGoByName(search));
                    break;
                case Target.Spell: // Combined SQL & DBC
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
                if (SelectedTarget == Target.Spell) // hybrid
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
            DataTable dbc = DBCQuery.GetSpells(); // "m_ID", "m_name_lang_1", "m_description_lang_1"
            DataTable sql = LookupQuery.GetSpells(search); // Id, Comment(name)
            DataRow newRow = null;

            // Add DBC spells
            if (dbc.Rows.Count != 1 && dbc.Columns.Count != 1) // Invalid DBC config
            {
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

        private void targetSelectCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((int)SelectedTarget != targetSelectCb.SelectedIndex)
                SelectedTarget = (Target)targetSelectCb.SelectedIndex;
        }

        /// <summary>
        /// Use model viewer when possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dataGrid.SelectedItem;
                if (SelectedTarget == Target.Item) // by displayid
                {
                    int id = Convert.ToInt32((uint)drv.Row[1]);
                    App.ModelViewer.LoadModel(id, "Item", "Display ID");
                    App._MainWindow.ShowModelViewer();
                }
                else if (SelectedTarget == Target.Creature) // By entry id
                {
                    int id = Convert.ToInt32((uint)drv.Row[0]);
                    App.ModelViewer.LoadModel(id, "Creature", "Entry ID");
                    App._MainWindow.ShowModelViewer();
                }
            }
            catch { /* Happens when clicking notification row, do nothing */ }
        }
        
        private void Context_Copy(object sender, RoutedEventArgs e)
        {
            // Usually copy target is col 0, add exceptions below
            int copyColTarget = 0;
            switch (SelectedTarget)
            {
                case Target.Item:
                case Target.Creature:
                    copyColTarget = 1;
                    break;
                default:
                    copyColTarget = 0;
                    break;
            }

            try
            {
                DataRowView r = dataGrid.SelectedItem as DataRowView;
                Clipboard.SetText(r.Row[copyColTarget].ToString());
            }
            catch // Occurs when not selecting a row
            {
                MessageBox.Show("Failed to copy item. Type it out manually.", 
                    "Failed to copy", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Context_Delete(object sender, RoutedEventArgs e)
        {
            // Generate delete query
            List<string[]> qParts = new List<string[]>();
            bool success = true;
            switch (SelectedTarget)
            {
                case Target.Creature:
                    qParts.Add(new string[2] { "creature_template", "entry" });
                    qParts.Add(new string[2] { "creature_template_addon", "entry" });
                    qParts.Add(new string[2] { "creature", "id" });
                    break;
                case Target.Item:
                    qParts.Add(new string[2] { "item_template", "entry" });
                    break;
                case Target.Quest:
                    qParts.Add(new string[2] { "quest_template", "ID" });
                    qParts.Add(new string[2] { "quest_addon", "ID" });
                    break;
                default:
                    success = false;
                    MessageBox.Show(
                        String.Format("Cannot delete a {0} in Trinity Creator.\nPlease delete it manually in the database.", Enum.GetName(typeof(Target), SelectedTarget)),
                        "Failed to delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

            // Confirm & execute
            try
            {
                DataRowView r = dataGrid.SelectedItem as DataRowView; // trigger possible error before asking confirmation
                int id = int.Parse(r.Row[0].ToString());
                string selectedTargetName = Enum.GetName(typeof(Target), SelectedTarget);
                Export.C type;

                if (success && MessageBoxResult.Yes == MessageBox.Show(
                    string.Format("Are you sure you want to delete {0} {1}?", selectedTargetName, id),
                    "Confirm delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    switch(SelectedTarget)
                    {
                        case Target.Creature:
                            type = Export.C.Creature;
                            break;
                        case Target.Quest:
                            type = Export.C.Quest;
                            break;
                        case Target.Item:
                            type = Export.C.Item;
                            break;
                        case Target.GameObject:
                            type = Export.C.GameObject;
                            break;
                        default:
                            MessageBox.Show($"TrinityCreator is unable to delete {selectedTargetName}s.");
                            return;
                    }

                    // Delete & refresh
                    SaveQuery.DeleteCreation(type, id);
                    searchBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                    // notify
                    MessageBox.Show(string.Format("Successfully deleted id {0} from database.", id), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch // Occurs when not selecting a row
            {
                MessageBox.Show("Failed to delete selected target. Please delete it manually.",
                    "Failed to delete", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
