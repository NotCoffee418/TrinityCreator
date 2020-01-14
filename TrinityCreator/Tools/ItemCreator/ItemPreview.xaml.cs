using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TrinityCreator.Tools.ItemCreator
{
    /// <summary>
    ///     Interaction logic for ItemPreview.xaml
    /// </summary>
    public partial class ItemPreview : UserControl, INotifyPropertyChanged
    {
        private readonly TrinityItem _item;

        public ItemPreview(TrinityItem item)
        {
            InitializeComponent();
            this._item = item;
            DataContext = _item;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void screenshotClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bmp = ConvertToBitmapSource(TakeScreenshot());
            Clipboard.SetImage(bmp);
            MessageBox.Show("Your screenshot has been saved to your clipboard.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void screenshotDiskBtn_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bmp = TakeScreenshot(); // screenshot first so UI doesn't overlap
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save screenshot";
            sfd.FileName = _item.Name;
            sfd.Filter = "Bitmap File|*.bmp";
            if (sfd.ShowDialog() == true)
            {
                using (FileStream fs = File.Open(sfd.FileName, FileMode.OpenOrCreate))
                    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                MessageBox.Show("Your screenshot has been saved to a file", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private Bitmap TakeScreenshot()
        {
            // Get position
            System.Windows.Point pos = itemTooltipGrid.PointToScreen(new System.Windows.Point(0, 0));
            int width = Convert.ToInt32(itemTooltipGrid.ActualWidth);
            int height = Convert.ToInt32(itemTooltipGrid.ActualHeight);

            // Bitmap in right size
            Bitmap Screenshot = new Bitmap(width, height);
            Graphics G = Graphics.FromImage(Screenshot);
            // snip wanted area
            G.CopyFromScreen(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y), 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            return Screenshot;
        }

        public BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}