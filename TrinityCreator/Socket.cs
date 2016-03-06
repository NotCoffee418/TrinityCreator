using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TrinityCreator
{
    public class Socket : IKeyValue
    {
        private string _description;
        private int _id;
        private BitmapImage _socketImage;

        public Socket(int id, string description, BitmapImage socketImage)
        {
            _id = id;
            _description = description;
            _socketImage = socketImage;
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public BitmapImage SocketImage
        {
            get
            {
                return _socketImage;
            }
            set
            {
                _socketImage = value;
            }
        } 

        public override string ToString()
        {
            return Description;
        }

        public static Socket[] GetSocketList()
        {
            return new Socket[]
            {
                new Socket(1, "Meta",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator;component/Resources/metasocket.png", UriKind.Absolute))),
                new Socket(2, "Red",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator;component/Resources/redsocket.png", UriKind.Absolute))),
                new Socket(4, "Yellow",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator;component/Resources/yellowsocket.png", UriKind.Absolute))),
                new Socket(8, "Blue",
                    new BitmapImage(new Uri("pack://application:,,,/TrinityCreator;component/Resources/bluesocket.png", UriKind.Absolute)))
            };
        }
    }
}
