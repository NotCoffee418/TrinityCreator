using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Win32;
using TrinityCreator.Properties;
using System.Data;
using TrinityCreator.Profiles;
using TrinityCreator.Data;
using TrinityCreator.UI.UIElements;
using TrinityCreator.Helpers;

namespace TrinityCreator.Tools.ItemCreator
{
    /// <summary>
    ///     Interaction logic for ArmorWeaponPage.xaml
    /// </summary>
    public partial class ItemPage : UserControl
    {
        private TrinityItem _item;
        private ItemPreview _preview;
        private bool _firstLoadComplete = false;

        public ItemPage()
        {
            InitializeComponent();

            // load preview & set item
            _item = new TrinityItem();
            DataContext = _item;
            Loaded += ItemPage_Loaded;
            _preview = new ItemPreview(_item);
        }
        public ItemPage(DataRow dr) : base()
        {
            LoadFromDataRow(dr);
        }

        private void LoadFromDataRow(DataRow dr)
        {
            throw new NotImplementedException();
        }

        private void ItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_firstLoadComplete)
            {
                PrepareItemPage();
                ShowCorrectClassBox();
                _firstLoadComplete = true;
            }
        }

        private void PrepareItemPage()
        {
            PreviewBox.Content = _preview;

            // Set class
            ItemClassCb.SelectedIndex = 0;

            // Set socket bonus
            SocketBonusCb.ItemsSource = XmlKeyValue.FromXml("SocketBonus");
            SocketBonusCb.SelectedIndex = 0;

            // Set quality
            ItemQualityCb.ItemsSource = ItemQuality.GetQualityList();
            ItemQualityCb.SelectedIndex = 0;

            // Set item bounds
            ItemBoundsCb.ItemsSource = XmlKeyValue.FromXml("ItemBonding");
            ItemBoundsCb.SelectedIndex = 0;

            // Set weapon groupbox
            DamageTypeCb.ItemsSource = DamageType.GetDamageTypes();
            DamageTypeCb.SelectedIndex = 0;

            // Set gemSockets groupbox
            _item.GemSockets.DynamicDataChanged += GemDataChangedHander;

            // set statsBox
            _item.Stats.DynamicDataChanged += StatsChangedHandler;

            // set resistance box
            _item.Resistances.DynamicDataChanged += ResistanceChangedHandler;

            // Race & class allowed
            _item.AllowedClass.BmspChanged += AllowedClass_BmspChanged;
            _item.AllowedRace.BmspChanged += AllowedRace_BmspChanged;

            // show resistances in preview
            _item.Resistances.DynamicDataChanged += ResistanceChangedHandler;
        }


        /// <summary>
        ///     Shows & hides UI groupboxes for certain item classes
        /// </summary>
        private void ShowCorrectClassBox()
        {
            if (!IsLoaded) // Only run if UI is loaded
                return;

            // Hide everything
            WeaponBox.Visibility = Visibility.Collapsed;
            ArmorBox.Visibility = Visibility.Collapsed;
            EquipmentBox.Visibility = Visibility.Collapsed;
            ContainerBox.Visibility = Visibility.Collapsed;
            VendorBox.Visibility = Visibility.Collapsed;
            AddResistancesCb.Visibility = Visibility.Collapsed;
            AddGemSocketsCb.Visibility = Visibility.Collapsed;
            StatsBox.Visibility = Visibility.Collapsed;
            _preview.StatsSp.Visibility = Visibility.Collapsed;

            try
            {
                switch (_item.Class.Id)
                {
                    case 0: // Consumable

                        break;
                    case 1: // Container
                        ContainerBox.Visibility = Visibility.Visible;
                        VendorBox.Visibility = Visibility.Visible;
                        break;
                    case 2: // Weapon
                        WeaponBox.Visibility = Visibility.Visible;
                        EquipmentBox.Visibility = Visibility.Visible;
                        VendorBox.Visibility = Visibility.Visible;
                        AddResistancesCb.Visibility = Visibility.Visible;
                        AddGemSocketsCb.Visibility = Visibility.Visible;
                        StatsBox.Visibility = Visibility.Visible;
                        _preview.StatsSp.Visibility = Visibility.Visible;
                        break;
                    case 3: // Gems
                        VendorBox.Visibility = Visibility.Visible;
                        //GemPropertiesBox
                        break;
                    case 4: // Armor
                        ArmorBox.Visibility = Visibility.Visible;
                        EquipmentBox.Visibility = Visibility.Visible;
                        VendorBox.Visibility = Visibility.Visible;
                        AddResistancesCb.Visibility = Visibility;
                        AddGemSocketsCb.Visibility = Visibility.Visible;
                        StatsBox.Visibility = Visibility.Visible;
                        _preview.StatsSp.Visibility = Visibility.Visible;
                        break;
                    case 5: // Reagent
                        VendorBox.Visibility = Visibility.Visible;
                        break;
                    case 6: // Projectile
                        VendorBox.Visibility = Visibility.Visible;
                        break;
                    case 7: // Trade goods
                        VendorBox.Visibility = Visibility.Visible;
                        break;
                    case 9: // Recipe
                        VendorBox.Visibility = Visibility.Visible;
                        break;
                    case 11: // Quiver
                        ContainerBox.Visibility = Visibility.Visible;
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
            {
                //fix this
            }
        }

        private void ShowCorrectInfoLabel()
        {
            if (!IsLoaded)
                return;

            var clid = _item.Class.Id;
            _preview.SubclassLeftNoteLbl.Visibility = Visibility.Visible;

            if (clid == 1) // Container
            {
                var b = new Binding();
                b.Source = _item;
                b.Path = new PropertyPath("ContainerSlots");
                b.StringFormat = "{0} Slots";
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(_preview.SubclassLeftNoteLbl, TextBlock.TextProperty, b);
                _preview.SubclassRightNoteLbl.Text = _item.ItemSubClass.Description;
            }
            else if (clid == 2 || clid == 4) // Weapon & armor
            {
                _preview.SubclassLeftNoteLbl.Text = _item.InventoryType.Description;
                _preview.SubclassRightNoteLbl.Text = _item.ItemSubClass.PreviewNoteRight;
            }
            else if (clid >= 11 && clid <= 15) // right only
            {
                _preview.SubclassLeftNoteLbl.Visibility = Visibility.Collapsed;
                _preview.SubclassRightNoteLbl.Text = _item.ItemSubClass.Description;
            }

            else // default
            {
                _preview.SubclassLeftNoteLbl.Text = ((ItemClass) ItemClassCb.SelectedValue).Description;
                _preview.SubclassRightNoteLbl.Text = ((ItemSubClass) ItemSubClassCb.SelectedValue).Description;
            }
        }

        /// <summary>
        /// If weapon & lich king or higher, return true
        /// </summary>
        /// <returns></returns>
        private bool NeedsWeaponExportWindow()
        {
            var expansion = ProfileHelper.GetProfileGameVersion();
            return _item.Class.Id == 2 && !Properties.Settings.Default.disableWeaponCreationNotice &&
                (expansion == ProfileHelper.Expansion.Unknown || expansion >= ProfileHelper.Expansion.WrathOfTheLichKing);
        }
        private void OpenWeaponExportWindow(string query, WeaponExportWindow.SaveType saveType)
        {
            // Open the window
            var weaponExportWindow = new WeaponExportWindow(query, _item.EntryId, saveType);

            // Disable export buttons until weapon window closes
            exportDbBtn.IsEnabled = false;
            exportSqlBtn.IsEnabled = false;
            weaponExportWindow.Closed += WeaponExportWindow_Closed;

            // Show
            weaponExportWindow.Show();
        }
        private void WeaponExportWindow_Closed(object sender, EventArgs e)
        {
            exportDbBtn.IsEnabled = true;
            exportSqlBtn.IsEnabled = true;
        }

        #region Changed event handlers

        private void itemQualityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var q = (ItemQuality) ItemQualityCb.SelectedValue;
            if (q != null)
                _preview.ItemNameLbl.Foreground = new SolidColorBrush(q.QualityColor);

            // set/unset account bound flags
            try
            {
                var bmcbr1 = from cb in _item.Flags.Children.OfType<BitmaskCheckBox>()
                    where (long) cb.Tag == 134217728
                    select cb;
                var bmcbr2 = from cb in _item.FlagsExtra.Children.OfType<BitmaskCheckBox>()
                    where (long) cb.Tag == 131072
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
            {
                /* Exception on initial load */
            }
        }

        private void itemClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectClassBox();
                ShowCorrectInfoLabel();
            }
            catch
            {
                // fix this
            }
        }

        private void itemSubClassCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectInfoLabel();
            }
            catch
            {
                // fix this
            }
        }

        private void inventortyTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowCorrectInfoLabel();
            }
            catch
            {
                // fix this
            }
        }

        private void GemDataChangedHander(object sender, EventArgs e)
        {
            _preview.GemsPanel.Children.Clear();
            try
            {
                foreach (var line in _item.GemSockets.GetUserInput())
                {
                    var count = int.Parse(line.Value);
                    var sock = (Socket) line.Key;
                    for (var i = 0; i < count; i++)
                    {
                        var dp = new DockPanel();
                        dp.Margin = new Thickness(5, 0, 0, 0);

                        var img = new Image();
                        img.Source = sock.SocketImage;
                        img.Width = 15;
                        img.Height = 15;
                        dp.Children.Add(img);

                        var lab = new Label();
                        lab.Content = sock.Description + " Socket";
                        lab.Foreground = Brushes.Gray;
                        lab.Margin = new Thickness(0, -5, 0, 0);
                        dp.Children.Add(lab);

                        _preview.GemsPanel.Children.Add(dp);
                    }
                }
            }
            catch
            {
                _preview.GemsPanel.Children.Clear();
            }
        }

        private void StatsChangedHandler(object sender, EventArgs e)
        {
            _preview.StatsSp.Children.Clear();
            try
            {
                int count = 0;
                foreach (var line in _item.Stats.GetUserInput())
                {
                    var stat = (XmlKeyValue)line.Key;
                    if (line.Value != "0")
                    {
                        var lab = new Label();
                        var amount = int.Parse(line.Value).ToString("+#;-#"); // Validate, add + or -
                        lab.Content = amount + " " + stat.Description;
                        lab.Foreground = Brushes.White;
                        lab.Margin = new Thickness(0, -5, 0, 0);
                        lab.FontSize = 14;
                        _preview.StatsSp.Children.Add(lab);
                        count++;
                    }
                    _item.StatsCount = count;
                }
            }
            catch
            {
                _preview.StatsSp.Children.Clear();
            }
        }

        private void ResistanceChangedHandler(object sender, EventArgs e)
        {
            _preview.ResistanceSp.Children.Clear();
            try
            {
                foreach (var line in _item.Resistances.GetUserInput())
                {
                    var dt = (DamageType) line.Key;
                    if (line.Value != "0")
                    {
                        var lab = new Label();
                        var amount = int.Parse(line.Value).ToString("+#;-#"); // Validate, add + or -
                        lab.Content = amount + " " + dt.Description + " Resistance";
                        lab.Foreground = Brushes.White;
                        lab.Margin = new Thickness(0, -5, 0, 0);
                        lab.FontSize = 14;
                        _preview.ResistanceSp.Children.Add(lab);
                    }
                }
            }
            catch
            {
                _preview.ResistanceSp.Children.Clear();
            }
        }

        private void AllowedClass_BmspChanged(object sender, EventArgs e)
        {
            _preview.AllowedClassesLb.Text = _item.AllowedClass.ToString();
        }

        private void AllowedRace_BmspChanged(object sender, EventArgs e)
        {
            _preview.AllowedRacesLb.Text = _item.AllowedRace.ToString();
        }

        #endregion

        #region Click event handlers
        private void exportDbBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = Export.Item(_item);
                if (NeedsWeaponExportWindow())
                    OpenWeaponExportWindow(query, WeaponExportWindow.SaveType.Database);
                else if (SaveQuery.CheckDuplicateHandleOverride(Export.C.Item, _item.EntryId))
                    SaveQuery.ToDatabase(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = Export.Item(_item);
                if (NeedsWeaponExportWindow())
                    OpenWeaponExportWindow(query, WeaponExportWindow.SaveType.File);
                else
                    SaveQuery.ToFile("Item " + _item.EntryId, query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void newItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to discard this item and clear the form?",
                "Discard item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _item = new TrinityItem();
                _preview = new ItemPreview(_item);
                PreviewBox.Content = _preview;
                DataContext = null;
                DataContext = _item;
                PrepareItemPage();
            }
        }


        private void FindDisplayIdBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }

        #endregion

    }
}