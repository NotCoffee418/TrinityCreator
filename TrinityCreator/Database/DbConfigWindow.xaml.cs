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
                HostTxt.Text = connStrB.Server;
                UserTxt.Text = connStrB.UserID;
                PasswordTxt.Text = connStrB.Password;
                DatabaseTxt.Text = connStrB.Database;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var connString = new MySqlConnectionStringBuilder();
            connString.Server = HostTxt.Text;
            connString.UserID = UserTxt.Text;
            connString.Password = PasswordTxt.Text;
            connString.Database = DatabaseTxt.Text;

            StatusLbl.Text = "Testing connection...";
            StatusLbl.Foreground = Brushes.Black;

            var testException = Connection.Test(connString);
            if (testException == null) // Success
            {
                StatusLbl.Text = "Connection test successful. Saved to settings.";
                StatusLbl.Foreground = Brushes.Green;

                Settings.Default.worldDb = connString;
                Settings.Default.Save();
            }
            else // failed
            {
                StatusLbl.Text = "Connection test failed: " + testException.Message;
                StatusLbl.Foreground = Brushes.Red;
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}