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
            Loaded += ItemPage_Loaded;
        }

        TrinityItem item;
        ItemPreview preview;

        private void ItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareItemPage();
            item.AllowedClass.BmspChanged += AllowedClass_BmspChanged;
            item.AllowedRace.BmspChanged += AllowedRace_BmspChanged;
        }

        private void AllowedClass_BmspChanged(object sender, EventArgs e)
        {
            preview.allowedClassesLb.Text = item.AllowedClass.ToString();
        }

        private void AllowedRace_BmspChanged(object sender, EventArgs e)
        {
            preview.allowedRacesLb.Text = item.AllowedRace.ToString();
        }

        private void PrepareItemPage()
        {
            preview = new ItemPreview(item);
            previewBox.Content = preview;

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


        #region Changed event handlers
        private void itemQualityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemQuality q = (ItemQuality)itemQualityCb.SelectedValue;
            preview.itemNameLbl.Foreground = new SolidColorBrush(q.QualityColor);

            // set/unset account bound flags
            try
            {
                var bmcbr1 = from cb in item.Flags.Children.OfType<BitmaskCheckBox>()
                            where (long)cb.Tag == 134217728
                            select cb;
                var bmcbr2 = from cb in item.FlagsExtra.Children.OfType<BitmaskCheckBox>()
                             where (long)cb.Tag == 131072
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
                ShowCorrectInfoLabel();
            }
            catch { // fix this
            }
        }
        private void itemSubClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectInfoLabel();
            }
            catch
            { // fix this
            }
        }

        private void inventortyTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectInfoLabel();
            }
            catch
            { // fix this
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
        }

        private void newItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to discard this item and clear the form?", "Discard item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                item = new TrinityItem();
                DataContext = null;
                DataContext = item;
                PrepareItemPage();
            }
        }
        #endregion


        /// <summary>
        /// Shows & hides UI groupboxes for certain item classes
        /// </summary>
        private void ShowCorrectClassBox()
        {
            // Hide everything
            weaponBox.Visibility = Visibility.Collapsed;
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
                        weaponBox.Visibility = Visibility.Visible;
                        equipmentBox.Visibility = Visibility.Visible;
                        vendorBox.Visibility = Visibility.Visible;
                        addResistancesCb.Visibility = Visibility.Visible;
                        addGemSocketsCb.Visibility = Visibility.Visible;
                        statsBox.Visibility = Visibility.Visible;
                        preview.statsSp.Visibility = Visibility.Visible;
                        break;
                    case 3: // Gems
                        vendorBox.Visibility = Visibility.Visible;
                        //GemPropertiesBox
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
                        //Recipe spell list & box?
                        break;
                    case 11: // Quiver
                        containerBox.Visibility = Visibility.Visible;
                        // auto set BagFamily based on subclass, clear on classchange
                        break;
                    case 12: // Quest
                        // empty, optional spell
                        break;
                    case 13: // Key
                        // lockid, from dbc?
                        break;
                    case 15: // Miscellaneous
                        // optional spell
                        break;
                }
            }
            catch
            { //fix this
            }
        }

        private void ShowCorrectInfoLabel()
        {
            int clid = item.Class.Id;
            preview.subclassLeftNoteLbl.Visibility = Visibility.Visible;

            if (clid == 1)  // Container
            {
                Binding b = new Binding();
                b.Source = item;
                b.Path = new PropertyPath("ContainerSlots");
                b.StringFormat = "{0} Slots";
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(preview.subclassLeftNoteLbl, TextBlock.TextProperty, b);
                preview.subclassRightNoteLbl.Text = item.ItemSubClass.Description;
            }
            else if (clid == 2 || clid == 4)  // Weapon & armor
            {
                preview.subclassLeftNoteLbl.Text = item.InventoryType.Description;
                preview.subclassRightNoteLbl.Text = item.ItemSubClass.PreviewNoteRight;
            }
            else if (clid >= 11 && clid <= 15) // right only
            {
                preview.subclassLeftNoteLbl.Visibility = Visibility.Collapsed;
                preview.subclassRightNoteLbl.Text = item.ItemSubClass.Description;
            }

            else // default
            {
                preview.subclassLeftNoteLbl.Text = ((ItemClass)itemClassCb.SelectedValue).Description;
                preview.subclassRightNoteLbl.Text = ((ItemSubClass)itemSubClassCb.SelectedValue).Description;
            }
        }
    }
}
