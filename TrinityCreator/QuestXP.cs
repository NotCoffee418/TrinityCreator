using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class QuestXp : IKeyValue
    {
        public QuestXp(int id, string description)
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

        public static QuestXp[] GetQuestXP()
        {
            return new QuestXp[]
            {
                new QuestXp(2, "No XP"),
                new QuestXp(4, "Simple delivery"),
                new QuestXp(5, "Long walk delivery"),
                new QuestXp(6, "Simple slaying quest"),
                new QuestXp(7, "Long slaying quest"),
                new QuestXp(8, "Elite & group quests"),
                new QuestXp(9, "End of quest chain (big reward)"),
                new QuestXp(10, "Raid quests"),
            };
        }   
    }
}
