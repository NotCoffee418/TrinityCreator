using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using TrinityCreator.Properties;

namespace TrinityCreator.Database
{
    /// <summary>
    ///     Interaction logic for DbConfigWindow.xaml
    /// </summary>
    public partial class DbConfigWindow : Window
    {
        public DbConfigWindow()
        {
            InitializeComponent();

            LoadSavedInfo();
        }

        // Load saved connection info
        private void LoadSavedInfo()
        {
            var connStrB = Settings.Default.worldDb;
            if (connStrB != null)
            {
                hostTxt.Text = connStrB.Server;
                userTxt.Text = connStrB.UserID;
                passwordTxt.Text = connStrB.Password;
                databaseTxt.Text = connStrB.Database;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = hostTxt.Text;
            conn_string.UserID = userTxt.Text;
            conn_string.Password = passwordTxt.Text;
            conn_string.Database = databaseTxt.Text;

            statusLbl.Text = "Testing connection...";
            statusLbl.Foreground = Brushes.Black;

            var testException = Connection.Test(conn_string);
            if (testException == null) // Success
            {
                statusLbl.Text = "Connection test successful. Saved to settings.";
                statusLbl.Foreground = Brushes.Green;

                Settings.Default.worldDb = conn_string;
                Settings.Default.Save();
            }
            else // failed
            {
                statusLbl.Text = "Connection test failed: " + testException.Message;
                statusLbl.Foreground = Brushes.Red;
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}