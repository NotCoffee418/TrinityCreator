using System;
using System.Collections.Generic;

namespace TrinityCreator.Data
{
    public class TrainerData : IKeyValue
    {
        public TrainerData(int id, string description, int trainerType, int trainerSpell, int trainerClass, int trainerRace)
        {
            Id = id;
            Description = description;
            TrainerType = trainerType;
            TrainerSpell = trainerSpell;
            TrainerClass = trainerClass;
            TrainerRace = trainerRace;
        }

        public int TrainerType { get; set; }
        public int TrainerSpell { get; set; }
        public int TrainerClass { get; set; }
        public int TrainerRace { get; set; } // mount trainer


        public override string ToString()
        {
            return Description;
        }

        public static TrainerData[] GetTrainerData()
        {
            return new TrainerData[]
            {
                new TrainerData(0, "Not a trainer", 0, 0, 0, 0),
                new TrainerData(1, "Profession Trainer", 2, 0, 0, 0),
                new TrainerData(2, "Pet Trainer for hunters", 3, 0, 3, 0), // hunter only
                new TrainerData(4, "Warrior Trainer", 0, 0, 1, 0),
                new TrainerData(5, "Paladin Trainer", 0, 0, 2, 0),
                new TrainerData(6, "Hunter Trainer", 0, 0, 3, 0),
                new TrainerData(7, "Rogue Trainer", 0, 0, 4, 0),
                new TrainerData(8, "Priest Trainer", 0, 0, 5, 0),
                new TrainerData(9, "Death Knight Trainer", 0, 0, 6, 0),
                new TrainerData(10, "Shaman Trainer", 0, 0, 7, 0),
                new TrainerData(11, "Mage Trainer", 0, 0, 8, 0),
                new TrainerData(12, "Warlock Trainer", 0, 0, 9, 0),
                new TrainerData(13, "Monk (5.x) Trainer", 0, 0, 10, 0),
                new TrainerData(14, "Druid Trainer", 0, 0, 11, 0),
                new TrainerData(16, "Human Mount Trainer", 1, 0, 0, 1),
                new TrainerData(17, "Orc Mount Trainer", 1, 0, 0, 2),
                new TrainerData(18, "Dwarf Mount Trainer", 1, 0, 0, 3),
                new TrainerData(19, "Night Elf Mount Trainer", 1, 0, 0, 4),
                new TrainerData(20, "Undead Mount Trainer", 1, 0, 0, 5),
                new TrainerData(21, "Tauren Mount Trainer", 1, 0, 0, 6),
                new TrainerData(22, "Gnome Mount Trainer", 1, 0, 0, 7),
                new TrainerData(23, "Troll Mount Trainer", 1, 0, 0, 8),
                new TrainerData(24, "Blood Elf Mount Trainer", 1, 0, 0, 10),
                new TrainerData(25, "Draenei Mount Trainer", 1, 0, 0, 11),
                new TrainerData(26, "Goblin Mount Trainer", 1, 0, 0, 9),
                new TrainerData(27, "Worgen Mount Trainer", 1, 0, 0, 22),
            };
        }
    }
}