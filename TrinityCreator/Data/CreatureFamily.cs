using System.Collections.Generic;

namespace TrinityCreator.Data
{
    public class CreatureFamily : IKeyValue
    {
        public CreatureFamily(int id, string description, int petSpellDataId = 0)
        {
            Id = id;
            Description = description;
            PetSpellDataId = petSpellDataId;
        }
        public int PetSpellDataId { get; internal set; }
        public override string ToString()
        {
            return Description;
        }

        public static CreatureFamily[] GetCreatureFamilies()
        {
            return new CreatureFamily[]
            {
                new CreatureFamily(0, "None"),
                new CreatureFamily(1, "Wolf", 5938),
                new CreatureFamily(2, "Cat", 5828),
                new CreatureFamily(3, "Spider", 5880),
                new CreatureFamily(4, "Bear", 5797),
                new CreatureFamily(5, "Boar", 9081),
                new CreatureFamily(6, "Crocolisk", 5847),
                new CreatureFamily(7, "Carrion Bird", 8311), 
                new CreatureFamily(8, "Crab", 5835),
                new CreatureFamily(9, "Gorilla", 9055),
                new CreatureFamily(11, "Raptor", 5830),
                new CreatureFamily(12, "Tallstrider", 5830),
                new CreatureFamily(29, "Felguard"),
                new CreatureFamily(20, "Scorpid", 5864),
                new CreatureFamily(21, "Turtle", 13466),
                new CreatureFamily(24, "Bat", 5786),
                new CreatureFamily(25, "Hyena", 8280),
                new CreatureFamily(26, "Owl", 5859),
                new CreatureFamily(27, "Wind Serpent", 8905),
                new CreatureFamily(30, "Dragonhawk", 10391),
                new CreatureFamily(31, "Ravager", 5949),
                new CreatureFamily(32, "Warp Stalker", 10525),
                new CreatureFamily(33, "Sporebat", 12957),
                new CreatureFamily(34, "Nether Ray", 12887),
                new CreatureFamily(35, "Serpent", 8905),
                new CreatureFamily(37, "Moth", 13099),
                new CreatureFamily(41, "Silithid", 8849),
                new CreatureFamily(42, "Worm", 12947),
                new CreatureFamily(44, "Wasp", 5874),
                new CreatureFamily(43, "Rhino"), // not tamable by default
                new CreatureFamily(38, "Chimaera"), // none with pet spell ID in db
                new CreatureFamily(39, "Devilsaur"), // none with pet spell ID in db
                new CreatureFamily(45, "Core Hound"), // none with pet spell ID in db
                new CreatureFamily(46, "Spirit Beast"), // needs to be set manually
                new CreatureFamily(23, "Imp"),
                new CreatureFamily(15, "Felhunter"),
                new CreatureFamily(16, "Voidwalker"),
                new CreatureFamily(17, "Succubus"),
                new CreatureFamily(19, "Doomguard"),
                new CreatureFamily(40, "Ghoul"),
                new CreatureFamily(28, "Remote Control"),
            };
        }
    }
}