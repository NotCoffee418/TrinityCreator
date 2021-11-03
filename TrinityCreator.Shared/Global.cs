using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Shared.Tools.LookupTool;
using TrinityCreator.Shared.Tools.ModelViewer;
using TrinityCreator.Shared.UI;

namespace TrinityCreator.Shared
{
    public static class Global
    {
        /// <summary>
        /// Global propertries
        /// </summary>
        public static Dictionary<string, dynamic> Properties { get; } 
            = new Dictionary<string, dynamic>();

        public static LookupToolControl LookupTool { get; set; }

        public static MainWindow _MainWindow { get; set; }

        public static ModelViewerPage ModelViewer { get; set; }
    }
}
