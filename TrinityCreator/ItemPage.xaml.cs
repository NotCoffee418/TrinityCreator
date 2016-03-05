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
        public ItemPage(Item _item, ItemPreview _preview)
        {
            InitializeComponent();
            item = _item;

            // load preview
            preview = _preview;
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

            // Set item bounds
            itemBoundsCb.ItemsSource = ItemBonding.GetItemBondingList();

            // Load flags groupbox
            item.Flags = BitmaskStackPanel.GetItemFlags();
            flagsBitMaskGroupBox.Content = item.Flags;
            flagsBitMaskGroupBox.Visibility = Visibility.Collapsed; // by default

            // load allowedclass groupbox
            item.AllowedClass = BitmaskStackPanel.GetClassFlags();
            limitClassBitMaskGroupBox.Content = item.AllowedClass;
            limitClassBitMaskGroupBox.Visibility = Visibility.Collapsed;
            preview.PrepareClassLimitations(item.AllowedClass);

            // load allowedrace groupbox
            item.AllowedRace = BitmaskStackPanel.GetRaceFlags();
            limitRaceBitMaskGroupBox.Content = item.AllowedRace;
            limitRaceBitMaskGroupBox.Visibility = Visibility.Collapsed;
            preview.PrepareRaceLimitations(item.AllowedRace);
        }

        Item item;

        /// <summary>
        /// Shows & hides UI groupboxes for certain item classes
        /// </summary>
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
            try
            {
                if (itemQuoteTxt.Text == "")
                    preview.itemQuoteLbl.Visibility = Visibility.Collapsed;
                else
                {
                    preview.itemQuoteLbl.Visibility = Visibility.Visible;
                    preview.itemQuoteLbl.Content = '"' + itemQuoteTxt.Text + '"';
                }
            }
            catch { /* Exception on initial load */ }
        }
        private void itemQualityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemQuality q = (ItemQuality)itemQualityCb.SelectedValue;
            preview.itemNameLbl.Foreground = new SolidColorBrush(q.QualityColor);

            // set/unset account bound flags
            try
            {
                var bmcbr = from cb in item.Flags.Children.OfType<BitmaskCheckBox>()
                            where (uint)cb.Tag == 134217728
                            select cb;
                if (q.Id == 7)
                    bmcbr.FirstOrDefault().IsChecked = true;
                else bmcbr.FirstOrDefault().IsChecked = false;
            }
            catch
            { /* Exception on initial load */ }
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
            if (sc == null)
                return;

            // Load equip
            inventoryTypeCb.ItemsSource = sc.LockedInventoryType;
            inventoryTypeCb.SelectedIndex = 0;

            // Load correct box
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

        private void itemBoundsCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemBonding b = (ItemBonding)itemBoundsCb.SelectedValue;
            preview.itemBoundsLbl.Content = b.Name;

            // Don't display when no bounds
            if (b.Id == 0)
                preview.itemBoundsLbl.Visibility = Visibility.Collapsed;
            else
                preview.itemBoundsLbl.Visibility = Visibility.Visible;
        }

        private void inventoryTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemInventoryType it = (ItemInventoryType)inventoryTypeCb.SelectedValue;
            if (it == null)
                return;

            preview.subclassLeftNoteLbl.Content = it.Description;
        }

        private void changeFlagsCb_Checked(object sender, RoutedEventArgs e)
        {
            flagsBitMaskGroupBox.Visibility = Visibility.Visible;
        }
        private void changeFlagsCb_Unchecked(object sender, RoutedEventArgs e)
        {
            flagsBitMaskGroupBox.Visibility = Visibility.Collapsed;
        }

        private void limitClassCb_Checked(object sender, RoutedEventArgs e)
        {
            limitClassBitMaskGroupBox.Visibility = Visibility.Visible;
            preview.itemClassRequirementsLbl.Visibility = Visibility.Visible;
        }
        private void limitClassCb_Unchecked(object sender, RoutedEventArgs e)
        {
            limitClassBitMaskGroupBox.Visibility = Visibility.Collapsed;
            if (preview.itemClassRequirementsLbl.Content.ToString().Contains(": All"))
                preview.itemClassRequirementsLbl.Visibility = Visibility.Collapsed;
        }
        private void limitRaceCb_Checked(object sender, RoutedEventArgs e)
        {
            limitRaceBitMaskGroupBox.Visibility = Visibility.Visible;
            preview.itemRaceRequirementsLbl.Visibility = Visibility.Visible;
        }
        private void limitRaceCb_Unchecked(object sender, RoutedEventArgs e)
        {
            limitRaceBitMaskGroupBox.Visibility = Visibility.Collapsed;
            if (preview.itemRaceRequirementsLbl.Content.ToString().Contains(": All"))
                preview.itemRaceRequirementsLbl.Visibility = Visibility.Collapsed;
        }

        private void buyPriceGTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.buyGoldLbl.Content = buyPriceGTxt.Text;
                preview.buyDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        private void buyPriceSTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.buySilverLbl.Content = buyPriceSTxt.Text;
                preview.buyDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        private void buyPriceCTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.buyCopperLbl.Content = buyPriceCTxt.Text;
                preview.buyDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        private void sellPriceGTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.sellGoldLbl.Content = sellPriceGTxt.Text;
                preview.sellDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        private void sellPriceSTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.sellSilverLbl.Content = sellPriceSTxt.Text;
                preview.sellDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        private void sellPriceCTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                preview.sellCopperLbl.Content = sellPriceCTxt.Text;
                preview.sellDockPanel.Visibility = Visibility.Visible;
            } catch { /* Exception on initial load */ }
        }
        #endregion

        #region Click event handlers
        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
            item.EntryId = int.Parse(entryIdTxt.Text);
            item.Quote = itemQuoteTxt.Text;
            item.Class = (ItemClass)itemClassCb.SelectedValue;
            item.ItemSubClass = (ItemSubClass)itemSubClassCb.SelectedValue;
            item.Name = itemNameTxt.Text;
            item.DisplayId = int.Parse(displayIdTxt.Text);
            item.Quality = (ItemQuality)itemQualityCb.SelectedValue;
            item.Binds = (ItemBonding)itemBoundsCb.SelectedValue;
            item.MinLevel = int.Parse(itemPlayerLevelTxt.Text);
            item.MaxAllowed = int.Parse(itemMaxCountTxt.Text);
            //item.AllowedClass; Already set in constructor
            //item.AllowedRace; Already set in constructor
            item.ValueBuy = new Currency(buyPriceGTxt.Text, buyPriceSTxt.Text, buyPriceCTxt.Text).Amount;
            item.ValueSell = new Currency(sellPriceGTxt.Text, sellPriceSTxt.Text, sellPriceCTxt.Text).Amount;
            item.InventoryType = (ItemInventoryType)inventoryTypeCb.SelectedValue;
            // Material set in ItemSubClass
            // sheath set in InventoryType
            // Flags set in constructor




            item.GenerateSqlQuery();
            // todo: Save query to sql file

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
