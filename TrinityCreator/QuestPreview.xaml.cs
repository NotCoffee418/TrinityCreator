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

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for QuestPreview.xaml
    /// </summary>
    public partial class QuestPreview : UserControl
    {
        public QuestPreview()
        {
            InitializeComponent();
        }

        private string _questStatusIcon;

        public string QuestStatusIcon
        {
            get { return "Resources/" + _questStatusIcon + ".png"; }
            set { _questStatusIcon = value; }
        }
        
    }
}
