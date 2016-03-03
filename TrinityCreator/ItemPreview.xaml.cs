using System;
using System.Collections.Generic;
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
    /// Interaction logic for ItemPreview.xaml
    /// </summary>
    public partial class ItemPreview : UserControl
    {
        public ItemPreview()
        {
            InitializeComponent();

        }
        public ItemPreview(Item item) : this()
        {
            UpdatePreview(item);
        }


        /// <summary>
        /// Display ArmorWeaponItem info in the preview 
        /// </summary>
        /// <param name="item"></param>
        public void UpdatePreview(Item item)
        {
            itemNameLbl.Content = item.Name;
            itemNameLbl.Foreground = new SolidColorBrush(item.Quality.QualityColor);
        }
    }
}
