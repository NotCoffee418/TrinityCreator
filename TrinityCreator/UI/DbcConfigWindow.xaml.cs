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
using System.Windows.Shapes;
using System.IO;
using TrinityCreator.DBC;

namespace TrinityCreator.UI
{
    /// <summary>
    /// Interaction logic for DbcConfigWindow.xaml
    /// </summary>
    public partial class DbcConfigWindow : Window
    {
        public DbcConfigWindow()
        {
            InitializeComponent();
            dbcDirTxt.Text = Properties.Settings.Default.DbcDir;
            ShowInfo();
        }

        private void ShowInfo()
        {
            infoTxt.Text = string.Format("This setting is optional and only used by the Lookup Tool in some instances.{0}" +
                "If you already have a DBC directory, you should point this setting there.{0}" +
                "Alternatively you can point to your WoW directory to extract the DBC files automatically.{0}{0}" +
                "This must point to 3.3.5a DBC files to work correctly. Leaving this empty is fine as well."
                , Environment.NewLine);
        }

        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = new System.Windows.Forms.FolderBrowserDialog();
            var r = d.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                dbcDirTxt.Text = d.SelectedPath;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string dir = dbcDirTxt.Text;
            if (File.Exists(dir + @"\Wow.exe")) // Is wow dir, add dbc to path
                dir += @"\dbc";

            // Save path for verification
            string oldDbcPath = Properties.Settings.Default.DbcDir;
            Properties.Settings.Default.DbcDir = dir;
            Properties.Settings.Default.Save();

            // Verify & notify or undo
            if (DbcHandler.VerifyDbcDir(openConfigWindowOnError: false))
            {
                MessageBox.Show("DBC Directory is configured correctly.", "Settings saved", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                Properties.Settings.Default.DbcDir = oldDbcPath;
                Properties.Settings.Default.Save();
            }
        }
    }
}
