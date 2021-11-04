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
using TrinityCreator.Shared.Data;

namespace TrinityCreator.Shared.UI.UIElements
{
    /// <summary>
    /// Interaction logic for CustomDisplayFieldControl.xaml
    /// </summary>
    public partial class CustomDisplayFieldControl : UserControl
    {
        public CustomDisplayFieldControl(CustomDisplayField field)
        {
            InitializeComponent();
            DataContext = field;
        }
    }
}
