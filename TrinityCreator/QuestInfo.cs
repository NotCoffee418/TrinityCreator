using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class QuestInfo : IKeyValue
    {
        public QuestInfo(int id, string  description)
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


        public static QuestInfo[] ListQuestInfo()
        {
            return new QuestInfo[]
            {
                new QuestInfo(0, "None"),
                new QuestInfo(1, "Group"),
                new QuestInfo(21, "Life"),
                new QuestInfo(41, "PvP"),
                new QuestInfo(62, "Raid"),
                new QuestInfo(81, "Dungeon"),
                new QuestInfo(82, "World Event"),
                new QuestInfo(83, "Legendary"),
                new QuestInfo(84, "Escort"),
                new QuestInfo(85, "Heroic"),
                new QuestInfo(88, "Raid (10)"),
                new QuestInfo(89, "Raid (25)")
            };
        }
    }
}
