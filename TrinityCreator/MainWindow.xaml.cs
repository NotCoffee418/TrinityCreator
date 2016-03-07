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
using System.Diagnostics;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            // Load usable creators
            Item item = new Item();
            ItemPreview preview = new ItemPreview(item);
            itemTab.Content = new ItemPage(item, preview);



            // view unfinished when set & on debug
            try
            {
                updatesBrowser.Navigate("https://github.com/RStijn/TrinityCreator/commits/master");
            }
            catch
            { /* too bad */ }
            bool unfinishedLoaded = false;
            if (Properties.Settings.Default.viewUnfinishedCreators)
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

            // make visible in case user enables while app is running
            //questTab.Content = new Quest();
            questTab.Visibility = Visibility.Visible;
            //Creature.Content = new Creature();
            creatureTab.Visibility = Visibility.Visible;

            return true;
        }
        private void HideUnfinishedPages()
        {
            questTab.Visibility = Visibility.Collapsed;
            creatureTab.Visibility = Visibility.Collapsed;
        }

        private void settingViewUnfinished_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.viewUnfinishedCreators = false;
            if (settingViewUnfinished.IsChecked)
            {
                string msg = string.Format("Not all displayed features in unfinished creators will work correctly.{0}" +
                    "Are you sure you want to view Unfinished creators?", Environment.NewLine);
                var result = MessageBox.Show(msg, "View unfinished creators", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                    Properties.Settings.Default.viewUnfinishedCreators = true;
                else settingViewUnfinished.IsChecked = false;
            }
                
            Properties.Settings.Default.Save();
        }


        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            Donate();
        }

        private void WhyDonate_Click(object sender, RoutedEventArgs e)
        {
            string msg = string.Format("Trinity Creator is released as an open source project created by a passionate IT student, so donating is completely optional.{0}{0}"+
                "If you're new to the emulation scene, you can use this program to create your own world and even release it as a public server.{0}" +
                "Or if you're already running a profitable server, you'll be able to save a lot of development time when releasing new content.{0}{0}"+
                "So, do you want to motivate me to implement more features or thank me for making things easier? Then toss me a few bucks! :)", Environment.NewLine);
            var result = MessageBox.Show(msg, "Why donate?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                Donate();
        }

        private void Donate()
        {
            Process.Start("https://paypal.me/RStijn");
        }
    }
}
