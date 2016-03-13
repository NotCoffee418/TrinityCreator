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
using System.Diagnostics;
using Microsoft.Win32;
using System.Data;

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

            // load preview & set item
            item = new TrinityItem();
            DataContext = item;
            preview = new ItemPreview(item);
            previewBox.Content = preview;

            Loaded += ItemPage_Loaded;
        }

        public ItemPage(DataRow dr) : this()
        {
            entryIdTxt.Text = dr["entry"].ToString();
            itemQuoteTxt.Text = dr["description"].ToString();
            //SetIndexOfId((int)dr["class"], itemClassCb);
            //SetIndexOfId((int)dr["subclass"], itemSubClassCb);
            itemNameTxt.Text = dr["name"].ToString();
            displayIdTxt.Text = dr["displayid"].ToString();
            SetIndexOfId((int)dr["Quality"], itemQualityCb);
            SetIndexOfId((int)dr["bonding"], itemBoundsCb);
            itemPlayerLevelTxt.Text = dr["RequiredLevel"].ToString();
            itemMaxCountTxt.Text = dr["maxcount"].ToString();
            item.AllowedClass.BitmaskValue = (uint)dr["AllowableClass"];
            item.AllowedRace.BitmaskValue = (uint)dr["AllowableRace"];

            Currency buyPrice = new Currency((int)dr["BuyPrice"]);
            buyPriceGTxt.Text = buyPrice.Gold.ToString();
            buyPriceSTxt.Text = buyPrice.Silver.ToString();
            buyPriceCTxt.Text = buyPrice.Copper.ToString();

            Currency sellPrice = new Currency((int)dr["SellPrice"]);
            sellPriceGTxt.Text = sellPrice.Gold.ToString();
            sellPriceSTxt.Text = sellPrice.Silver.ToString();
            sellPriceCTxt.Text = sellPrice.Copper.ToString();
            
            //SetIndexOfId((int)dr["InventoryType"], inventoryTypeCb);
            // Material auto?
            // sheath auto?
            item.Flags.BitmaskValue = (uint)dr["Flags"];
            item.Flags.BitmaskValue = (uint)dr["FlagsExtra"];
            buyCountTxt.Text = dr["BuyCount"].ToString();
            itemStackCountTxt.Text = dr["stackable"].ToString();
            containerSlotsTxt.Text = dr["ContainerSlots"].ToString();
            damageMinTxt.Text = dr["dmg_min1"].ToString();
            damageMaxTxt.Text = dr["dmg_max1"].ToString();
            SetIndexOfId((int)dr["dmg_type1"], damageTypeCb);
        }

        TrinityItem item;
        ItemPreview preview;

        private void ItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set class
            itemClassCb.SelectedIndex = 0;

            // Set socket bonus
            socketBonusCb.SelectedIndex = 0;
            

            // Set quality
            itemQualityCb.ItemsSource = ItemQuality.GetQualityList();
            itemQualityCb.SelectedIndex = 0;

            ShowCorrectClassBox();

            // Set item bounds
            itemBoundsCb.ItemsSource = ItemBonding.GetItemBondingList();
            itemBoundsCb.SelectedIndex = 0;

            // Set weapon groupbox
            damageTypeCb.ItemsSource = DamageType.GetDamageTypes();
            damageTypeCb.SelectedIndex = 0;

            // Set gemSockets groupbox
            item.GemSockets.Changed += GemDataChangedHander;

            // set statsBox
            item.Stats.Changed += StatsChangedHandler;
        }

        private void SetIndexOfId(int requestId, ComboBox cb)
        {
            for (int i = 0; i < cb.Items.Count; i++)
                if (((IKeyValue)cb.Items[i]).Id == requestId)
                {
                    cb.SelectedIndex = i;
                    return;
                }
        }

        #region Changed event handlers
        private void itemQualityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemQuality q = (ItemQuality)itemQualityCb.SelectedValue;
            preview.itemNameLbl.Foreground = new SolidColorBrush(q.QualityColor);

            // set/unset account bound flags
            try
            {
                var bmcbr1 = from cb in item.Flags.Children.OfType<BitmaskCheckBox>()
                            where (uint)cb.Tag == 134217728
                            select cb;
                var bmcbr2 = from cb in item.FlagsExtra.Children.OfType<BitmaskCheckBox>()
                             where (uint)cb.Tag == 131072
                             select cb;
                if (q.Id == 7)
                {
                    bmcbr1.FirstOrDefault().IsChecked = true;
                    bmcbr2.FirstOrDefault().IsChecked = true;
                }
                else
                {
                    bmcbr1.FirstOrDefault().IsChecked = false;
                    bmcbr2.FirstOrDefault().IsChecked = false;
                }
            }
            catch
            { /* Exception on initial load */ }
        }

        private void itemClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectClassBox();
            }
            catch { // fix this
            }
        }

        private void GemDataChangedHander(object sender, EventArgs e)
        {
            preview.gemsPanel.Children.Clear();
            try
            {
                foreach (var line in item.GemSockets.GetUserInput())
                {
                    int count = int.Parse(line.Value);
                    Socket sock = (Socket)line.Key;
                    for (int i = 0; i < count; i++)
                    {
                        DockPanel dp = new DockPanel();
                        dp.Margin = new Thickness(5, 0, 0, 0);

                        Image img = new Image();
                        img.Source = sock.SocketImage;
                        img.Width = 15;
                        img.Height = 15;
                        dp.Children.Add(img);

                        Label lab = new Label();
                        lab.Content = sock.Description + " Socket";
                        lab.Foreground = Brushes.Gray;
                        lab.Margin = new Thickness(0, -5, 0, 0);
                        dp.Children.Add(lab);

                        preview.gemsPanel.Children.Add(dp);
                    }
                }
            }
            catch
            {
                preview.gemsPanel.Children.Clear();
            }
        }

        private void StatsChangedHandler(object sender, EventArgs e)
        {
            preview.statsSp.Children.Clear();
            try
            {
                foreach (var line in item.Stats.GetUserInput())
                {
                    Stat stat = (Stat)line.Key;
                    if (line.Value != "0")
                    {
                        Label lab = new Label();
                        string amount = int.Parse(line.Value).ToString("+#;-#"); // Validate, add + or -
                        lab.Content = amount + " " + stat.Description;
                        lab.Foreground = Brushes.White;
                        lab.Margin = new Thickness(0, -5, 0, 0);
                        preview.statsSp.Children.Add(lab);
                    }
                }
            }
            catch
            {
                preview.statsSp.Children.Clear();
            }
        }
        #endregion

        #region Click event handlers
        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {

                string query = item.GenerateSqlQuery();
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".sql";
                sfd.FileName = "Item " + item.EntryId;
                sfd.Filter = "SQL File (.sql)|*.sql";
                if (sfd.ShowDialog() == true)
                    System.IO.File.WriteAllText(sfd.FileName, query);
                
                // Increase next item's entry id
                Properties.Settings.Default.nextid_item = int.Parse(entryIdTxt.Text) + 1;
                Properties.Settings.Default.Save();

                MessageBox.Show("Your item has been saved.", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            /*}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query.", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private void newItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to discard this item and clear the form?", "Discard item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                ClearForm();
        }
        #endregion


        /// <summary>
        /// Shows & hides UI groupboxes for certain item classes
        /// </summary>
        private void ShowCorrectClassBox()
        {
            // Hide everything
            armorBox.Visibility = Visibility.Collapsed;
            equipmentBox.Visibility = Visibility.Collapsed;
            containerBox.Visibility = Visibility.Collapsed;
            vendorBox.Visibility = Visibility.Collapsed;
            addResistancesCb.Visibility = Visibility.Collapsed;
            addGemSocketsCb.Visibility = Visibility.Collapsed;
            statsBox.Visibility = Visibility.Collapsed;
            preview.statsSp.Visibility = Visibility.Collapsed;
            try
            {
                switch (item.Class.Id)
                {
                    case 0: // Consumable

                        break;
                    case 1: // Container
                        containerBox.Visibility = Visibility.Visible;
                        vendorBox.Visibility = Visibility.Visible;
                        break;
                    case 2: // Weapon
                        equipmentBox.Visibility = Visibility.Visible;
                        vendorBox.Visibility = Visibility.Visible;
                        addResistancesCb.Visibility = Visibility.Visible;
                        addGemSocketsCb.Visibility = Visibility.Visible;
                        statsBox.Visibility = Visibility.Visible;
                        preview.statsSp.Visibility = Visibility.Visible;
                        break;
                    case 3: // Gems
                        vendorBox.Visibility = Visibility.Visible;
                        break;
                    case 4: // Armor
                        armorBox.Visibility = Visibility.Visible;
                        equipmentBox.Visibility = Visibility.Visible;
                        vendorBox.Visibility = Visibility.Visible;
                        addResistancesCb.Visibility = Visibility;
                        addGemSocketsCb.Visibility = Visibility.Visible;
                        statsBox.Visibility = Visibility.Visible;
                        preview.statsSp.Visibility = Visibility.Visible;
                        break;
                    case 5: // Reagent
                        vendorBox.Visibility = Visibility.Visible;
                        break;
                    case 6: // Projectile
                        vendorBox.Visibility = Visibility.Visible;
                        break;
                    case 7: // Trade goods

                        break;
                    case 9: // Recipe

                        break;
                    case 11: // Quiver

                        break;
                    case 12: // Quest

                        break;
                    case 13: // Key

                        break;
                    case 15: // Miscellaneous

                        break;
                    case 16: // Glyph

                        break;
                }
            }
            catch
            { //fix this
            }
        }

        private void ClearForm()
        {
            entryIdTxt.Text = Properties.Settings.Default.nextid_item.ToString();
            MessageBox.Show("Not implemented yet"); // Probably just load new ItemPage, don't clear all the fields manually :P
        }

        private void containerSlotsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*try
            {
                ItemSubClass isc = (ItemSubClass)itemSubClassCb.SelectedValue;
                preview.subclassLeftNoteLbl.Content = containerSlotsTxt.Text + " slot " + isc.Description;
            }
            catch { /* fix this }*/
        }

    }
}
