using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    class ItemBonding
    {
        public ItemBonding(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private int _id;
        private string _name;


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

        public override string ToString()
        {
            return Name;
        }

        public static ItemBonding[] GetItemBondingList()
        {
            return new ItemBonding[] {
                new ItemBonding(0, "No bounds"),
                new ItemBonding(1, "Binds when picked up"),
                new ItemBonding(2, "Binds when equipped"),
                new ItemBonding(3, "Binds when used"),
                new ItemBonding(4, "Quest item"),
            };
        }
    }
}
