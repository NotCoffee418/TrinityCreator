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
using TrinityCreator.Shared.Helpers;
using TrinityCreator.Shared.Profiles;

namespace TrinityCreator.Shared.Tools.VendorCreator
{
    /// <summary>
    /// Interaction logic for VendorEntryControl.xaml
    /// </summary>
    public partial class VendorEntryControl : UserControl, ICreator
    {
        public VendorEntryControl()
        {
            InitializeComponent();
            CustomDisplayFields = Profile.Active.GetCustomDisplayFields(Export.C.Vendor);
            Profile.ActiveProfileChangedEvent += Profile_ActiveProfileChangedEvent;
            UiHelper.PrepareCustomDisplayFields(customDisplayFieldGb, this);
        }



        public Export.C ExportType { get; } = Export.C.Creature;
        public List<CustomDisplayField> CustomDisplayFields { get; set; }
            = new List<CustomDisplayField>();

        private void Profile_ActiveProfileChangedEvent(object sender, EventArgs e)
        {
            UiHelper.PrepareCustomDisplayFields(customDisplayFieldGb, this);
        }

        public event EventHandler RemoveRequestEvent;

        private void removeMeBtn_Click(object sender, RoutedEventArgs e)
        {
            Profile.ActiveProfileChangedEvent -= Profile_ActiveProfileChangedEvent;
            RemoveRequestEvent(this, e);
        }

        private void itemLookupBtn_Click(object sender, RoutedEventArgs e)
        {
            Global.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }
    }
}
