using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class QuestSort : IKeyValue
    {
        public QuestSort(int id, string  description)
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


        public static QuestInfo[] ListQuestSort()
        {
            return new QuestInfo[]
            {
                new QuestInfo(-0, "Zone ID"), // toggle a textbox for zone input
                new QuestInfo(-61, "Warlock"),
                new QuestInfo(-82, "Shaman"),
                new QuestInfo(-161, "Mage"),
                new QuestInfo(-261, "Hunter"),
                new QuestInfo(-263, "Druid"),
                new QuestInfo(-81, "Warrior"),
                new QuestInfo(-141, "Paladin"),
                new QuestInfo(-162, "Rogue"),
                new QuestInfo(-262, "Priest"),
                new QuestInfo(-372, "Death Knight"),
                new QuestInfo(-24, "Herbalism"),
                new QuestInfo(-101, "Fishing"),
                new QuestInfo(-121, "Blacksmithing"),
                new QuestInfo(-181, "Alchemy"),
                new QuestInfo(-182, "Leatherworking"),
                new QuestInfo(-201, "Engeneering"),
                new QuestInfo(-264, "Tailoring"),
                new QuestInfo(-304, "Cooking"),
                new QuestInfo(-324, "First Aid"),
                new QuestInfo(-762, "Riding"),
                new QuestInfo(-371, "Inscription"),
                new QuestInfo(-373, "Jewelcrafting"),
                new QuestInfo(-1, "Epic"),
                new QuestInfo(-22, "Seasonal"),
                new QuestInfo(-25, "Battlegrounds"),
                new QuestInfo(-221, "Treasure map"),
                new QuestInfo(-241, "Tournament"),
                new QuestInfo(-284, "Special"),
                new QuestInfo(-344, "Legendary"),
                new QuestInfo(-364, "Darkmoon Faire"),
                new QuestInfo(-365, "Ahn'Qiraj War"),
                new QuestInfo(-366, "Lunar Festival"),
                new QuestInfo(-367, "Reputation"),
                new QuestInfo(-368, "Invasion"),
                new QuestInfo(-369, "Midsummer"),
                new QuestInfo(-370, "Brewfest"),
                new QuestInfo(-374, "Noblegarden"),
                new QuestInfo(-375, "Pilgrim's Bounty"),
                new QuestInfo(-376, "Love is in the Air"),
            };
        }
    }
}
