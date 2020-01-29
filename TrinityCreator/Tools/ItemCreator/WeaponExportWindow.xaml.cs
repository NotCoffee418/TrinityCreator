using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using TrinityCreator.Helpers;
using TrinityCreator.Profiles;

namespace TrinityCreator.Tools.ItemCreator
{
    /// <summary>
    /// Interaction logic for WeaponExportWindow.xaml
    /// </summary>
    public partial class WeaponExportWindow : Window
    {
        /// <summary>
        /// This window provides information about exporting weapons since it's broken on new ids since lich king.
        /// It'll grab the query generated in ItemPage and optionally add a delete query for the same id, then export as it would before
        /// if the user chooses to do so.
        /// </summary>
        /// <param name="query">insert query</param>
        /// <param name="id">item id</param>
        /// <param name="_saveType">where to</param>
        public WeaponExportWindow(string query, int itemId, SaveType _saveType)
        {
            InitializeComponent();
            Query = query;
            ItemId = itemId;
            _SaveType = _saveType;
        }

        // For testing UI, it will break if you try to export with this constructor
        public WeaponExportWindow()
        {
            InitializeComponent();
        }

        public enum SaveType
        {
            File = 0,
            Database = 1
        }

        public string Query { get; set; }
        public int ItemId { get; set; }
        public SaveType _SaveType { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the RTF document from resources
            var documentBytes = Encoding.UTF8.GetBytes(Properties.Resources.weapon_notice);
            using (var reader = new MemoryStream(documentBytes))
            {
                reader.Position = 0;
                weaponNoticeRtb.SelectAll();
                weaponNoticeRtb.Selection.Load(reader, DataFormats.Rtf);
            }

            // Readonly after load
            weaponNoticeRtb.IsReadOnly = true;
        }

        private void justExportBtn_Click(object sender, RoutedEventArgs e)
        {
            DoExport();
        }

        private void deleteExportBtn_Click(object sender, RoutedEventArgs e)
        {
            // Generate delete query
            var ek = new ExpKvp("EntryId", ItemId, Export.C.Item);
            string deleteQuery = $"DELETE FROM {ek.SqlTableName} WHERE {ek.SqlKey} = {ItemId};";

            // Add it to query and DoExport
            Query = deleteQuery + Environment.NewLine + Query;
            DoExport();
        }

        /// <summary>
        /// Export to chosen output and close window
        /// </summary>
        private void DoExport()
        {
            try
            {
                if (_SaveType == SaveType.Database)
                    SaveQuery.ToDatabase(Query);
                else SaveQuery.ToFile($"Item {ItemId}", Query);

                // Change never show setting if checked
                Properties.Settings.Default.disableWeaponCreationNotice = neverShowAgainCb.IsChecked == true;
                Properties.Settings.Default.Save();
            }
            catch
            {
                Logger.Log("WeaponExportWindow: Failed to generate query.", Logger.Status.Error, true);
            }
            Close();
        }
    }
}
