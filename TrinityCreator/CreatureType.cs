using System.Collections.Generic;

namespace TrinityCreator
{
    public class CreatureType : IKeyValue
    {
        public CreatureType(int id, string description)
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

        public static CreatureType[] GetCreatureTypes()
        {
            return new CreatureType[]
            {
                new CreatureType(0, "None"),
                new CreatureType(1, "Beast"),
                new CreatureType(2, "Dragonkin"),
                new CreatureType(3, "Demon"),
                new CreatureType(4, "Elemental"),
                new CreatureType(5, "Giant"),
                new CreatureType(6, "Undead"),
                new CreatureType(7, "Humanoid"),
                new CreatureType(8, "Critter"),
                new CreatureType(9, "Mechanical"),
                new CreatureType(10, "Not specified"),
                new CreatureType(11, "Totem"),
                new CreatureType(12, "Non-Combat Pet"),
                new CreatureType(13, "Gas Cloud"),
            };
        }
    }
}