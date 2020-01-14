using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using TrinityCreator.Database;
using TrinityCreator.Properties;

namespace TrinityCreator.UI
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
                        case "port":
                            PortTxt.Text = sub[1];
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
            string connString = "server=" + HostTxt.Text + ";user=" + UserTxt.Text + ";password=" + PasswordTxt.Text + ";database=" + DatabaseTxt.Text + ";port=" + PortTxt.Text + ";";

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

        private void restoreDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear your database settings? This will not affect your database.", 
                "Reset database settings?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Settings.Default.worldDb = "";
                Settings.Default.Save();
                HostTxt.Text = "";
                PortTxt.Text = "";
                UserTxt.Text = "";
                PasswordTxt.Text = "";
                DatabaseTxt.Text = "";
            }
        }
    }
}