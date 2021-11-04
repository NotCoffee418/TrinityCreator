using System;
using System.Windows.Media.Imaging;

namespace TrinityCreator.Shared.Data
{
    public class Socket : IKeyValue
    {
        public Socket(int id, string description, BitmapImage socketImage)
        {
            Id = id;
            Description = description;
            SocketImage = socketImage;
        }

        public BitmapImage SocketImage { get; set; }

        public override string ToString()
        {
            return Description;
        }

        public static Socket[] GetSocketList()
        {
            return new[]
            {
                new Socket(1, "Meta",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator.Shared;component/Resources/metasocket.png",
                        UriKind.Absolute))),
                new Socket(2, "Red",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator.Shared;component/Resources/redsocket.png",
                        UriKind.Absolute))),
                new Socket(4, "Yellow",
                    new BitmapImage(new Uri(
                        "pack://application:,,,/TrinityCreator.Shared;component/Resources/yellowsocket.png", UriKind.Absolute))),
                new Socket(8, "Blue",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator.Shared;component/Resources/bluesocket.png",
                        UriKind.Absolute)))
            };
        }
    }
}