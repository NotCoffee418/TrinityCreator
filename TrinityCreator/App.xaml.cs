using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

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

            // Handle startup args
            HandleArgs();

            // Check for updates
            Updater.Run();

            // Run application
            MainWindow mw = new MainWindow();
            mw.Show();
        }

        private void HandleArgs() // 0 is current exe path
        {
            string[] args = Environment.GetCommandLineArgs();
            // niy
        }

        public static LookupTool LookupTool { get; internal set; }
        public static MainWindow _MainWindow { get; internal set; }
        public static ModelViewerPage ModelViewer { get; internal set; }
    }
}