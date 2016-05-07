using System.Collections.Generic;

namespace TrinityCreator
{
    public class CreatureRank : IKeyValue
    {
        public CreatureRank(int id, string description)
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

        public static CreatureRank[] GetCreatureRanks()
        {
            return new CreatureRank[]
            {
                new CreatureRank(0, "Normal"),
                new CreatureRank(1, "Elite"),
                new CreatureRank(2, "Rare Elite"),
                new CreatureRank(3, "Boss"),
                new CreatureRank(4, "Rare"),
            };
        }
    }
}