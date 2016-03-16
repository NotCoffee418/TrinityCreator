using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TrinityCreator
{
    /// <summary>
    ///     Interaction logic for ItemPreview.xaml
    /// </summary>
    public partial class ItemPreview : UserControl, INotifyPropertyChanged
    {
        private readonly TrinityItem _item;

        public ItemPreview(TrinityItem _item)
        {
            InitializeComponent();
            this._item = _item;
            DataContext = this._item;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void screenshotClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void screenshotDiskBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }
    }
}