using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace TrinityCreator.Data
{
    public class ItemQuality : IKeyValue, INotifyPropertyChanged
    {
        private Color _color;

        public ItemQuality(int id, string name, Color color)
        {
            Id = id;
            Description = name;
            QualityColor = color;
        }

        public Color QualityColor
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged("QualityColor");
            }
        }

        public int Id
        {
            get { return base.Id; }
            set
            {
                base.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Description
        {
            get { return base.Description; }
            set
            {
                base.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        public override string ToString()
        {
            return Description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public static int FindQualityId(string name)
        {
            var found = from q in GetQualityList() where q.Description == name select q.Id;
            return found.First();
        }

        public string FindQualityName(int id)
        {
            var found = from q in GetQualityList() where q.Id == id select q.Description;
            return found.First();
        }

        public static ItemQuality[] GetQualityList()
        {
            return new[]
            {
                new ItemQuality(0, "Poor", Color.FromRgb(157, 157, 157)),
                new ItemQuality(1, "Common", Color.FromRgb(255, 255, 255)),
                new ItemQuality(2, "Uncommon", Color.FromRgb(30, 255, 0)),
                new ItemQuality(3, "Rare", Color.FromRgb(0, 112, 221)),
                new ItemQuality(4, "Epic", Color.FromRgb(163, 53, 238)),
                new ItemQuality(5, "Legendary", Color.FromRgb(255, 128, 0)),
                new ItemQuality(6, "Artifact", Colors.Red),
                new ItemQuality(7, "Bind to Account", Color.FromRgb(229, 204, 128))
            };
        }
    }
}