using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TrinityCreator
{
    public class ItemQuality
    {
        public ItemQuality(int id, string name, Color color)
        {
            Id = id;
            Name = name;
            QualityColor = color; 
        }

        private int _id;
        private string _name;
        private Color _color;


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

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Color QualityColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }

        }

        public static int FindQualityId(string name)
        {
            var found = from q in GetQualityList() where q.Name == name select q.Id;
            return found.First();
        }

        public string FindQualityName(int id)
        {
            var found = from q in GetQualityList() where q.Id == id select q.Name;
            return found.First();
        }

        public override string ToString()
        {
            return Name;
        }

        public static ItemQuality[] GetQualityList()
        {
            return new ItemQuality[] {
                new ItemQuality(0, "Poor", Colors.Gray),
                new ItemQuality(1, "Common", Colors.White),
                new ItemQuality(2, "Uncommon", Colors.Green),
                new ItemQuality(3, "Rare", Colors.Blue),
                new ItemQuality(4, "Epic", Colors.Purple),
                new ItemQuality(5, "Legendary", Colors.Orange),
                new ItemQuality(6, "Artifact", Colors.Red),
                new ItemQuality(7, "Bind to Account", Colors.Gold),
            };
        }
    }
}
