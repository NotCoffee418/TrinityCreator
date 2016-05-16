using System.Collections.Generic;

namespace TrinityCreator
{
    public class TrainerData
    {
        public TrainerData(string description, int trainerType, int trainerSpell, int trainerClass, int trainerRace)
        {
            Description = description;
        }

        public string Description { get; set; }
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
                new TrainerData("Not a trainer", 0, 0, 0, 0),
                new TrainerData("Profession Trainer", 2, 0, 0, 0),
                new TrainerData("Pet Trainer for hunters", 3, 0, 3, 0), // hunter only
                new TrainerData("Any class Trainer", 0, 0, 0, 0), // untested
                new TrainerData("Warrior Trainer", 0, 0, 1, 0),
                new TrainerData("Paladin Trainer", 0, 0, 2, 0),
                new TrainerData("Hunter Trainer", 0, 0, 3, 0),
                new TrainerData("Rogue Trainer", 0, 0, 4, 0),
                new TrainerData("Priest Trainer", 0, 0, 5, 0),
                new TrainerData("Death Knight Trainer", 0, 0, 6, 0),
                new TrainerData("Shaman Trainer", 0, 0, 7, 0),
                new TrainerData("Mage Trainer", 0, 0, 8, 0),
                new TrainerData("Warlock Trainer", 0, 0, 9, 0),
                new TrainerData("Monk (5.x) Trainer", 0, 0, 10, 0),
                new TrainerData("Druid Trainer", 0, 0, 11, 0),
                new TrainerData("Any Race Mount Trainer", 1, 0, 0, 0), // untested
                new TrainerData("Human Mount Trainer", 1, 0, 0, 1),
                new TrainerData("Orc Mount Trainer", 1, 0, 0, 2),
                new TrainerData("Dwarf Mount Trainer", 1, 0, 0, 3),
                new TrainerData("Night Elf Mount Trainer", 1, 0, 0, 4),
                new TrainerData("Undead Mount Trainer", 1, 0, 0, 5),
                new TrainerData("Tauren Mount Trainer", 1, 0, 0, 6),
                new TrainerData("Gnome Mount Trainer", 1, 0, 0, 7),
                new TrainerData("Troll Mount Trainer", 1, 0, 0, 8),
                new TrainerData("Blood Elf Mount Trainer", 1, 0, 0, 10),
                new TrainerData("Draenei Mount Trainer", 1, 0, 0, 11),
                new TrainerData("Goblin Mount Trainer", 1, 0, 0, 9),
                new TrainerData("Worgen Mount Trainer", 1, 0, 0, 22),
            };
        }
    }
}