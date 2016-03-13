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
            if (item.EntryId == 0)
                item.EntryId = Properties.Settings.Default.nextid_item;

            // Set item bounds
            itemBoundsCb.ItemsSource = ItemBonding.GetItemBondingList();
            itemBoundsCb.SelectedIndex = 0;

            // Load flags groupbox
            item.Flags = BitmaskStackPanel.GetItemFlags();

            // Load FlagsExtra
            item.FlagsExtra = BitmaskStackPanel.GetItemFlagsExtra();

            // load allowedclass groupbox
            item.AllowedClass = BitmaskStackPanel.GetClassFlags();
            preview.PrepareClassLimitations(item.AllowedClass);

            // load allowedrace groupbox
            item.AllowedRace = BitmaskStackPanel.GetRaceFlags();
            preview.PrepareRaceLimitations(item.AllowedRace);

            // Set weapon groupbox
            item.DamageInfo = new Damage();
            damageTypeCb.ItemsSource = DamageType.GetDamageTypes();
            damageTypeCb.SelectedIndex = 0;

            // Set resistance groupbox
            item.Resistances = new DynamicDataControl(
                DamageType.GetDamageTypes(magicOnly: true), 6, unique: true);

            // Set gemSockets groupbox
            item.GemSockets = new DynamicDataControl(
                Socket.GetSocketList(), 3, unique: false, header1: "Socket Type", header2: "Amount", defaultValue: "0");
            preview.gemsPanel.Visibility = Visibility.Collapsed;
            item.GemSockets.Changed += GemDataChangedHander;

            // set statsBox
            item.Stats = new DynamicDataControl(Stat.GetStatList(), 10, unique: false, header1: "Stat", header2: "Value", defaultValue: "0");
            item.Stats.Changed += StatsChangedHandler;

            // BagFamily
            item.BagFamily = BitmaskStackPanel.GetBagFamilies();
            StackPanel containerContent = (StackPanel)containerBox.Content;
            containerContent.Children.Add(item.BagFamily);
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
            /*
            //try {
                try
                {
                    item.EntryId = int.Parse(entryIdTxt.Text);
                }
                catch
                {
                    throw new Exception("Invalid entry ID");
                }
                item.Quote = itemQuoteTxt.Text;
                item.Class = (ItemClass)itemClassCb.SelectedValue;
                item.ItemSubClass = (ItemSubClass)itemSubClassCb.SelectedValue;
                item.Name = itemNameTxt.Text;
                try
                {
                    item.DisplayId = int.Parse(displayIdTxt.Text);
                }
                catch
                {
                    throw new Exception("Invalid display ID");
                }
                item.Quality = (ItemQuality)itemQualityCb.SelectedValue;
                item.Binds = (ItemBonding)itemBoundsCb.SelectedValue;
                try
                {
                    item.MinLevel = int.Parse(itemPlayerLevelTxt.Text);
                }
                catch
                {
                    item.MinLevel = 0;
                }

                try
                {
                    item.MaxAllowed = int.Parse(itemMaxCountTxt.Text);
                }
                catch
                {
                    item.MaxAllowed = 0;
                }
                //item.AllowedClass; Already set in constructor
                //item.AllowedRace; Already set in constructor
                item.ValueBuy = new Currency(buyPriceGTxt.Text, buyPriceSTxt.Text, buyPriceCTxt.Text);
                item.ValueSell = new Currency(sellPriceGTxt.Text, sellPriceSTxt.Text, sellPriceCTxt.Text);
                item.InventoryType = (ItemInventoryType)inventoryTypeCb.SelectedValue;
                // Material set in ItemSubClass
                // sheath set in InventoryType
                // Flags set in constructor
                // FlagsExtra set in constructor
                try
                {
                    item.BuyCount = int.Parse(buyCountTxt.Text);
                }
                catch
                {
                    item.BuyCount = 1;
                }

                try
                {
                    item.Stackable = int.Parse(itemStackCountTxt.Text);
                }
                catch
                {
                    item.Stackable = 1;
                }

                try
                {
                    item.ContainerSlots = int.Parse(containerSlotsTxt.Text);
                }
                catch
                {
                    item.ContainerSlots = 0;
                }
                // dmg_min, dmg_max, dmg_type, delay is changed in item with valid changedevents
                // resistances set in constructor

                // set ammo_type if needed
                ItemSubClass isc = (ItemSubClass)itemSubClassCb.SelectedValue;
                if (isc.Description == "Bow")
                    item.AmmoType = 2;
                else if (isc.Description == "Gun")
                    item.AmmoType = 3;
                else item.AmmoType = 0;

                // set durability
                try
                {
                    item.Durability = int.Parse(durabilityTxt.Text);
                }
                catch
                {
                    item.Durability = 0;
                }
                // sockets set in constructor
                item.SocketBonus = (SocketBonus)socketBonusCb.SelectedValue;

                try
                {
                    item.Armor = int.Parse(itemArmorTxt.Text);
                }
                catch
                {
                    item.Armor = 0;
                }

                try
                {
                    item.Block = int.Parse(itemBlockTxt.Text);
                }
                catch
                {
                    item.Block = 0;
                }
                */

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
