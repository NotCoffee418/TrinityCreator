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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            HandleArgs();
        }

        private void HandleArgs() // 0 is current exe path
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 2)
            {
                if (args[1].Contains("-TrinityUpdater.exe") && System.IO.File.Exists(args[1])) // finished update
                    System.IO.File.Delete(args[1]);
            }
        }

        public static LookupTool LookupTool { get; internal set; }
        public static MainWindow _MainWindow { get; internal set; }
    }
}