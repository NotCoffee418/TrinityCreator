using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TrinityCreator.Database;
using TrinityCreator.Properties;
using TrinityCreator.DBC;
using System.Timers;
using System.Collections.Generic;
using System.Xml;

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
            App._MainWindow = this;

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

            // prepare lookup tool
            ContentGrid.ColumnDefinitions[2].Width = new GridLength(25);
            ContentGridSplitter.Visibility = Visibility.Collapsed;
            
            // Load usable creators
            ItemTab.Content = new ItemPage();
            QuestTab.Content = new QuestPage();
            ModelViewerTabFrame.Content = new ModelViewerPage();
            CreatureCreatorTab.Content = new CreatureCreatorPage();
            LootCreatorTab.Content = new LootPage();
            VendorCreatorTab.Content = new VendorPage();
            
            // Set emulator
            switch (Properties.Settings.Default.emulator)
            {
                case 0: // trinity335a
                    trinity335a64Rb.IsChecked = true;
                    break;
                case 1: // cMangos112
                    cMangos112Rb.IsChecked = true;
                    break;
                case 2: // azeroth335a
                    azeroth335aRb.IsChecked = true;
                    break;
                case 3: // cMangos335a
                    cMangos335aRb.IsChecked = true;
                    break;
                case 4: // cMangos112
                    trinity335a201901Rb.IsChecked = true;
                    break;
            }

            // Load randomTip
            tipTimer.Elapsed += ChangeRandomTip;
            tipTimer.Interval = 200; // don't change interval here
            tipTimer.Start();
        }

        static Random random = new Random();
        Timer tipTimer = new Timer();
        
        public void ShowLookupTool()
        {
            LookupToolExpander.IsExpanded = true;
        }

        public void ShowModelViewer()
        {
            ModelViewer.IsSelected = true;
        }

        private void configureMysql_Click(object sender, RoutedEventArgs e)
        {
            new DbConfigWindow().Show();
        }

        private void configureDbc_Click(object sender, RoutedEventArgs e)
        {
            new DBC.DbcConfigWindow().Show();
        }

        #region Settings
        // select the emulator
        private void trinity335a64Rb_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.emulator = 0;
            Properties.Settings.Default.Save();
        }
        private void cMangos112Rb_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.emulator = 1;
            Properties.Settings.Default.Save();
        }
        private void azeroth335a_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.emulator = 2;
            Properties.Settings.Default.Save();

            // TODO: Lazy fix, take this out when updating emulator system
            try
            {
                ((CreatureCreatorPage)CreatureCreatorTab.Content).MinMaxDmgDp.Visibility = Visibility.Visible;
            }
            catch { /* will fail on startup, page load event also has one */ }
        }
        private void cMangos335aRb_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.emulator = 3;
            Properties.Settings.Default.Save();
        }
        private void trinity335a201901Rb_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.emulator = 4;
            Properties.Settings.Default.Save();
        }
        #endregion


        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            new CreditsWindow().Show();
        }

        private void ReportBugs_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Nadromar/TrinityCreator/issues/new");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var r = MessageBox.Show("You may have unsaved changed. Are you sure you want to close TrinityCreator?", "Closing", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (r == MessageBoxResult.No)
                e.Cancel = true;
            else Process.GetCurrentProcess().Kill(); // quick-fix suicide, something's hanging the program
        }

        private void LookupToolExpander_Expanded(object sender, RoutedEventArgs e)
        {
            ContentGrid.ColumnDefinitions[2].Width = new GridLength(Settings.Default.lookupToolWidth);
            ContentGridSplitter.Visibility = Visibility.Visible;
        }

        private void LookupToolExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //_lookupToolWidth = ContentGrid.ColumnDefinitions[2].Width.Value;
            ContentGrid.ColumnDefinitions[2].Width = new GridLength(25);
            ContentGridSplitter.Visibility = Visibility.Collapsed;
        }

        private void ContentGridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Settings.Default.lookupToolWidth = ContentGrid.ColumnDefinitions[2].Width.Value;
            Settings.Default.Save();
        }


        private void ChangeRandomTip(object sender, ElapsedEventArgs e)
        {
            tipTimer.Interval = 30000; // 30sec
            string[] allTips = Properties.Resources.RandomTips.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string oldText = "";
            string newText = "";
            do
            {
                if (allTips.Length > 0)
                {
                    newText = allTips[random.Next(allTips.Length - 1)];
                    randomTipTxt.Dispatcher.Invoke(new Action(() =>
                    {
                        oldText = randomTipTxt.Text;
                        randomTipTxt.Text = newText;
                    }));
                }
            } while (oldText == newText);         
        }

        private void modelViewerTab_Selected(object sender, RoutedEventArgs e)
        {
            ShowLookupTool();
        }
    }
}