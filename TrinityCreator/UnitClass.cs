using System.Collections.Generic;

namespace TrinityCreator
{
    public class UnitClass : IKeyValue
    {
        public UnitClass(int id, string description)
        {
            Id = id;
            Description = description;
        }


        public int Id { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }

        public static UnitClass[] GetUnitClasses()
        {
            return new UnitClass[]
            {
                new UnitClass(1, "Warrior (Health only)"),
                new UnitClass(2, "Paladin (Health & mana)"),
                new UnitClass(8, "Mage (Less health & more mana)"),
            };
        }
    }
}