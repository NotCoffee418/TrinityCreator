using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace TrinityCreatorUpdater
{
    class Program
    {
        static string path = String.Empty;

        static void Main(string[] args)
        {
            Console.WriteLine("- Trinity Creator Updater -");
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TrinityCreator.exe";
            if (args.Length > 0)
                path = args[0];

            while (Process.GetProcessesByName("TrinityCreator").Length != 0)
            {
                Thread.Sleep(2000);
                Console.WriteLine("Waiting for Trinity Creator to close...");
            }
            
            StartUpdate();
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private static void StartUpdate()
        {
            Console.WriteLine("Downloading latest version of Trinity Creator...");
            WebClient c = new WebClient();
            c.DownloadFile("https://github.com/RStijn/TrinityCreator/blob/master/TrinityCreator/bin/Release/TrinityCreator.exe?raw=true", path);

            Console.WriteLine("Downloaded to {0}", path);
            Console.WriteLine("Opening Trinity Creator...");
            Process.Start(path);
        }
    }
}
