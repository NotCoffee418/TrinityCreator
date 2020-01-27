using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using TrinityCreator.Profiles;
using TrinityCreator.Tools.LookupTool;
using TrinityCreator.Tools.ModelViewer;
using TrinityCreator.UI;
using TrinityCreator.Helpers;

namespace TrinityCreator
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Related to not breaking float input on different locales
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // Indicate that the app has started to shut down background thread when application closes
            App.Current.Properties["IsRunning"] = true;
            Logger.Log("Application Starting...");

            // Upgrade settings
            if (TrinityCreator.Properties.Settings.Default.UpgradeRequired)
            {
                Logger.Log("First run or version has changed since last run. Running Settings.Upgrade()");
                TrinityCreator.Properties.Settings.Default.Upgrade();
                TrinityCreator.Properties.Settings.Default.UpgradeRequired = false;
                TrinityCreator.Properties.Settings.Default.Save();
            }

            // Handle startup args
            HandleArgs();

            // Check for application update
            Updater.Run();

            // Check for and install profile updates
            Updater.UpdateProfiles();

            // Ensure we have a valid profile before loading MainWindow
            Profile.ActiveProfileChangedEvent += Profile_ActiveProfileChangedEvent;

            // Force profile input if it doesn't know a working one
            string knownActiveProfilePath = TrinityCreator.Properties.Settings.Default.ActiveProfilePath;
            if (knownActiveProfilePath == String.Empty ||
                !System.IO.File.Exists(knownActiveProfilePath) ||
                Profile.LoadFile(knownActiveProfilePath) == null)
            {
                Logger.Log("First run or corrupt profile. Prompting user to select a profile before starting.");
                var pwin = new ProfileSelectionWindow();
                pwin.Show();
            }
            else Profile.Active = Profile.LoadFile(knownActiveProfilePath);
        }
        
        // Open mainwindow when we have a valid profile active
        private void Profile_ActiveProfileChangedEvent(object sender, EventArgs e)
        {
            var x = Profile.Active;
            if (Profile.Active != null)
            {
                _MainWindow = new MainWindow();
                _MainWindow.Show();

                // Only once on launch.
                Profile.ActiveProfileChangedEvent -= Profile_ActiveProfileChangedEvent;
            }
        }

        private void HandleArgs() // 0 is current exe path
        {
            string[] args = Environment.GetCommandLineArgs();
            // niy
        }

        public static LookupToolControl LookupTool { get; internal set; }
        public static MainWindow _MainWindow { get; internal set; }
        public static ModelViewerPage ModelViewer { get; internal set; }
    }
}