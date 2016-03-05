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
        public ItemPreview(Item _item)
        {
             InitializeComponent();
             item = _item;

             // hide unset values
             buyDockPanel.Visibility = Visibility.Collapsed;
             sellDockPanel.Visibility = Visibility.Collapsed;
             itemClassRequirementsLbl.Visibility = Visibility.Collapsed;
             itemRaceRequirementsLbl.Visibility = Visibility.Collapsed;
        }

        private Item item;

        private void screenshotClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void screenshotDiskBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        internal void PrepareClassLimitations(BitmaskStackPanel bitmaskStackPanel)
        {
            foreach (BitmaskCheckBox c in bitmaskStackPanel.Children)
            {
                c.Checked += UpdateClassLimitations;
                c.Unchecked += UpdateClassLimitations;
            }
        }

        internal void PrepareRaceLimitations(BitmaskStackPanel bitmaskStackPanel)
        {
            foreach (BitmaskCheckBox c in bitmaskStackPanel.Children)
            {
                c.Checked += UpdateRaceLimitations;
                c.Unchecked += UpdateRaceLimitations;
            }
        }

        private void UpdateClassLimitations(object sender, RoutedEventArgs e)
        {
            itemClassRequirementsLbl.Content = "Classes: " + item.AllowedClass.ToString();
        }

        private void UpdateRaceLimitations(object sender, RoutedEventArgs e)
        {
            itemRaceRequirementsLbl.Content = "Races: " + item.AllowedRace.ToString();
        }

    }
}
