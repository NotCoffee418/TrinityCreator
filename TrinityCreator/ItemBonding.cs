using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemBonding : IKeyValue
    {
        public ItemBonding(int id, string name)
        {
            Id = id;
            Description = name;
        }

        private int _id;
        private string _description;


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

        public override string ToString()
        {
            return Description;
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
