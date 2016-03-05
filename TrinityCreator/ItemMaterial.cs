using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemMaterial : IKeyValue
    {
        public ItemMaterial(int id, string description)
        {
            Id = id;
            Description = description;
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

        internal static ItemMaterial GetConsumable()
        {
            return new ItemMaterial(-1, "Consumable");
        }

        internal static ItemMaterial GetUndefined()
        {
            return new ItemMaterial(0, "Not Defined");
        }

        internal static ItemMaterial GetPlate()
        {
            return new ItemMaterial(6, "Plate");
        }

        internal static ItemMaterial GetChain()
        {
            return new ItemMaterial(5, "Chainmail");
        }

        internal static ItemMaterial GetLeather()
        {
            return new ItemMaterial(8, "Leather");
        }

        internal static ItemMaterial GetCloth()
        {
            return new ItemMaterial(7, "Cloth");
        }
    }
}
