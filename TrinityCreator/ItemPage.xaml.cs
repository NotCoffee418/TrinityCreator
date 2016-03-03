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
    /// Interaction logic for ArmorWeaponPage.xaml
    /// </summary>
    public partial class ItemPage : UserControl
    {
        public ItemPage()
        {
            InitializeComponent();

            // load preview
            preview = new ItemPreview();
            previewBox.Content = preview;

            // Set quality
            itemQualityCb.ItemsSource = ItemQuality.GetQualityList();
            itemQualityCb.SelectedIndex = 0;

            // Set class & subclass
            itemClassCb.ItemsSource = ItemClass.GetClassList();
            itemClassCb.SelectedIndex = 0;

            ShowCorrectClassBox();
            armorBox.Visibility = Visibility.Collapsed;
            entryIdTxt.Text = Properties.Settings.Default.nextid_item.ToString();

        }

        private void ShowCorrectClassBox()
        {
            // Hide everything
            weaponBox.Visibility = Visibility.Collapsed;
            armorBox.Visibility = Visibility.Collapsed;

            // Show selected
            ItemClass selectedClass = (ItemClass)itemClassCb.SelectedValue;
            switch (selectedClass.Id)
            {
                case 2:
                    weaponBox.Visibility = Visibility.Visible;
                    break;
                case 4:
                    armorBox.Visibility = Visibility.Visible;
                    break;
            }
        }

        ItemPreview preview;

        #region Changed event handlers
        private void itemNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            preview.itemNameLbl.Content = itemNameTxt.Text;
        }
        private void itemQuoteTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (itemQuoteTxt.Text == "")
                preview.itemQuoteLbl.Visibility = Visibility.Collapsed;
            else
            {
                preview.itemQuoteLbl.Visibility = Visibility.Visible;
                preview.itemQuoteLbl.Content = '"' + itemQuoteTxt.Text + '"';
            }
        }
        private void itemQualityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemQuality q = (ItemQuality)itemQualityCb.SelectedValue;
            preview.itemNameLbl.Foreground = new SolidColorBrush(q.QualityColor);
        }
        
        private void itemClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemClass c = (ItemClass)itemClassCb.SelectedValue;
            itemSubClassCb.ItemsSource = ItemSubClass.GetSubclassList(c);
            itemSubClassCb.SelectedIndex = 0;
            ShowCorrectClassBox();
        }

        private void itemSubClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemSubClass sc = (ItemSubClass)itemSubClassCb.SelectedValue;

            if (sc.PreviewNoteLeft == "")
                preview.subclassLeftNoteLbl.Visibility = Visibility.Collapsed;
            else
            {
                preview.subclassLeftNoteLbl.Content = sc.PreviewNoteLeft;
                preview.subclassRightNoteLbl.Visibility = Visibility.Visible;
            }

            if (sc.PreviewNoteRight == "")
                preview.subclassRightNoteLbl.Visibility = Visibility.Collapsed;
            else
            {
                preview.subclassRightNoteLbl.Content = sc.PreviewNoteRight;
                preview.subclassRightNoteLbl.Visibility = Visibility.Visible;
            }
        }




        #endregion

        #region Click event handlers
        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");

            // Increase next item's entry id
            Properties.Settings.Default.nextid_item = int.Parse(entryIdTxt.Text) + 1;
            Properties.Settings.Default.Save();
        }

        private void newItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to discard this item and clear the form?", "Discard item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                ClearForm();
        }
        #endregion

        private void ClearForm()
        {
            entryIdTxt.Text = Properties.Settings.Default.nextid_item.ToString();
            MessageBox.Show("Not implemented yet"); // Probably just load new ItemPage, don't clear all the fields manually :P
        }

        
    }
}
