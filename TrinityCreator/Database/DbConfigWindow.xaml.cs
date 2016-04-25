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
            string connStr = Settings.Default.worldDb;
            if (connStr != "")
            {
                string[] connData = connStr.Split(';');
                foreach (string part in connData)
                {
                    string[] sub = part.Split('=');
                    switch (sub[0])
                    {
                        case "server":
                            HostTxt.Text = sub[1];
                            break;
                        case "user":
                            UserTxt.Text = sub[1];
                            break;
                        case "password":
                            PasswordTxt.Text = sub[1];
                            break;
                        case "database":
                            DatabaseTxt.Text = sub[1];
                            break;
                    }
                }
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string connString = "server=" + HostTxt.Text + ";user=" + UserTxt.Text + ";password=" + PasswordTxt.Text + ";database=" + DatabaseTxt.Text + ";";

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