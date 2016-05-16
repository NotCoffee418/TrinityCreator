using System.Collections.Generic;

namespace TrinityCreator
{
    public class CreatureFamily : IKeyValue
    {
        public CreatureFamily(int id, string description)
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

        public static CreatureFamily[] GetCreatureFamilies()
        {
            return new CreatureFamily[]
            {
                new CreatureFamily(0, "None"),
                new CreatureFamily(1, "Wolf"),
                new CreatureFamily(2, "Cat"),
                new CreatureFamily(3, "Spider"),
                new CreatureFamily(4, "Bear"),
                new CreatureFamily(5, "Boar"),
                new CreatureFamily(6, "Crocolisk"),
                new CreatureFamily(7, "Carrion Bird"),
                new CreatureFamily(8, "Crab"),
                new CreatureFamily(9, "Gorilla"),
                new CreatureFamily(11, "Raptor"),
                new CreatureFamily(12, "Tallstrider"),
                new CreatureFamily(15, "Felhunter"),
                new CreatureFamily(16, "Voidwalker"),
                new CreatureFamily(17, "Succubus"),
                new CreatureFamily(19, "Doomguard"),
                new CreatureFamily(20, "Scorpid"),
                new CreatureFamily(21, "Turtle"),
                new CreatureFamily(23, "Imp"),
                new CreatureFamily(24, "Bat"),
                new CreatureFamily(25, "Hyena"),
                new CreatureFamily(26, "Owl"),
                new CreatureFamily(27, "Wind Serpent"),
                new CreatureFamily(28, "Remote Control"),
                new CreatureFamily(29, "Felguard"),
                new CreatureFamily(30, "Dragonhawk"),
                new CreatureFamily(31, "Ravager"),
                new CreatureFamily(32, "Warp Stalker"),
                new CreatureFamily(33, "Sporebat"),
                new CreatureFamily(34, "Nether Ray"),
                new CreatureFamily(35, "Serpent"),
                new CreatureFamily(37, "Moth"),
                new CreatureFamily(38, "Chimaera"),
                new CreatureFamily(39, "Devilsaur"),
                new CreatureFamily(40, "Ghoul"),
                new CreatureFamily(41, "Silithid"),
                new CreatureFamily(42, "Worm"),
                new CreatureFamily(43, "Rhino"),
                new CreatureFamily(44, "Wasp"),
                new CreatureFamily(45, "Core Hound"),
                new CreatureFamily(46, "Spirit Beast"),
            };
        }
    }
}