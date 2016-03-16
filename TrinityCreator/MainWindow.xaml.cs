using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TrinityCreator.Database;
using TrinityCreator.Properties;

namespace TrinityCreator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Check for updates
#if !DEBUG
            Updater.CheckLatestVersion();
#endif
            // presist settings on version change
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            // Load usable creators

            ItemTab.Content = new ItemPage();
            QuestTab.Content = new QuestPage();


            // view unfinished when set & on debug
            try
            {
                UpdatesBrowser.Navigate("https://github.com/RStijn/TrinityCreator/commits/master");
            }
            catch
            {
                /* too bad */
            }
            var unfinishedLoaded = false;
            if (Settings.Default.viewUnfinishedCreators)
                unfinishedLoaded = LoadUnfinishedPages(unfinishedLoaded);
#if DEBUG
            unfinishedLoaded = LoadUnfinishedPages(unfinishedLoaded);
#endif
            if (!unfinishedLoaded)
                HideUnfinishedPages();
        }


        private bool LoadUnfinishedPages(bool done)
        {
            if (done)
                return true;

            //questTab.Content = new Quest();
            //Creature.Content = new Creature();

            return true;
        }

        private void HideUnfinishedPages()
        {
            QuestTab.Visibility = Visibility.Collapsed;
            CreatureTab.Visibility = Visibility.Collapsed;
        }

        private void configureMysql_Click(object sender, RoutedEventArgs e)
        {
            var win = new DbConfigWindow();
            win.Show();
        }

        private void settingViewUnfinished_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.viewUnfinishedCreators = false;
            if (SettingViewUnfinished.IsChecked)
            {
                var msg = string.Format("Not all displayed features in unfinished creators will work correctly.{0}" +
                                        "Are you sure you want to view Unfinished creators?", Environment.NewLine);
                var result = MessageBox.Show(msg, "View unfinished creators", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Settings.Default.viewUnfinishedCreators = true;
                    MessageBox.Show("Restart Trinity Creator to view changes.", "Setting saved", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else SettingViewUnfinished.IsChecked = false;
            }

            Settings.Default.Save();
        }


        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            Donate();
        }

        private void WhyDonate_Click(object sender, RoutedEventArgs e)
        {
            var msg =
                string.Format(
                    "Trinity Creator is released as an open source project created by a passionate IT student, so donating is completely optional.{0}{0}" +
                    "If you're new to the emulation scene, you can use this program to create your own world and even release it as a public server.{0}" +
                    "Or if you're already running a profitable server, you'll be able to save a lot of development time when releasing new content.{0}{0}" +
                    "So, do you want to motivate me to implement more features or thank me for making things easier? Then toss me a few bucks! :)",
                    Environment.NewLine);
            var result = MessageBox.Show(msg, "Why donate?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                Donate();
        }

        private void Donate()
        {
            Process.Start("https://paypal.me/RStijn");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }
    }
}