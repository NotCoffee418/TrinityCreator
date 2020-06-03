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
using TrinityCreator.Profiles;
using TrinityCreator.Tools.ItemCreator;
using TrinityCreator.Tools.QuestCreator;
using TrinityCreator.Tools.ModelViewer;
using TrinityCreator.Tools.CreatureCreator;
using TrinityCreator.Tools.LootCreator;
using TrinityCreator.Tools.VendorCreator;
using TrinityCreator.Tools.ProfileCreator;
using TrinityCreator.Helpers;
using System.Windows.Controls;

namespace TrinityCreator.UI
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

            // prepare lookup tool
            ContentGrid.ColumnDefinitions[2].Width = new GridLength(25);
            ContentGridSplitter.Visibility = Visibility.Collapsed;
            
            // Load usable creators
            ItemTab.Content = new ItemPage();
            QuestTab.Content = new QuestPage();
            CreatureCreatorTab.Content = new CreatureCreatorPage();
            LootCreatorTab.Content = new LootPage();
            VendorCreatorTab.Content = new VendorPage();

            // Load randomTip
            tipTimer.Elapsed += ChangeRandomTip;
            tipTimer.Interval = 200; // don't change interval here
            tipTimer.Start();

            // Load model viewer on windows 10 or message on older
            try
            {
                if (System.Environment.OSVersion.Version >= new Version(6, 2))
                    ModelViewerTabFrame.Content = new ModelViewerPage();
                else
                {
                    TextBox tb = new TextBox();
                    tb.Text = "Model Viewer is not supported before Windows 10.";
                    ModelViewerTabFrame.Content = tb;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception has occurred loading model viewer: " + ex.Message);
            }
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
            new DbcConfigWindow().Show();
        }

        private void configureProfile_Click(object sender, RoutedEventArgs e)
        {
            new ProfileSelectionWindow().Show();
        }
 
        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            new CreditsWindow().Show();
        }

        private void ReportBugs_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/NotCoffee418/TrinityCreator/issues/new");
        }

        private void DiscordBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.com/invite/gUXwShK");
        }

        private void ViewLogsDir_Click(object sender, RoutedEventArgs e)
        {
            string logsDir = Logger.GetLogsDirectory();
            if (System.IO.Directory.Exists(logsDir))
                Process.Start(logsDir);
        }

        private void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            Updater.Run(force: true);
        }

        private void ProfileCreator_Click(object sender, RoutedEventArgs e)
        {
            var win = new ProfileCreatorWindow();
            win.Show();
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