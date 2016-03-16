namespace TrinityCreator
{
    public class Stat : IKeyValue
    {
        public Stat(int id, string description)
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

        internal static Stat[] GetStatList()
        {
            return new[]
            {
                new Stat(0, "Mana"),
                new Stat(1, "Health"),
                new Stat(3, "Agility"),
                new Stat(4, "Strength"),
                new Stat(5, "Intellect"),
                new Stat(6, "Spirit"),
                new Stat(7, "Stamina"),
                new Stat(12, "Defense skill"),
                new Stat(13, "Dodge"),
                new Stat(14, "Parry"),
                new Stat(15, "Block"),
                new Stat(31, "Hit"),
                new Stat(16, "Melee hit"),
                new Stat(17, "Ranged hit"),
                new Stat(18, "Spell hit"),
                new Stat(32, "Crit Rating"),
                new Stat(19, "Melee crit"),
                new Stat(20, "Ranged crit"),
                new Stat(21, "Spell crit"),
                new Stat(36, "Haste"),
                new Stat(28, "Melee haste"),
                new Stat(29, "Ranged haste"),
                new Stat(30, "Spell haste"),
                new Stat(35, "Resilience"),
                new Stat(37, "Expertise"),
                new Stat(38, "Attack power"),
                new Stat(39, "Ranged attach power"),
                new Stat(41, "Healing"),
                new Stat(42, "Spell damage"),
                new Stat(43, "Mana regeneration"),
                new Stat(44, "Armor penetration"),
                new Stat(45, "Spell power"),
                new Stat(46, "Health regeneration"),
                new Stat(47, "Spell penetration"),
                new Stat(48, "Block value")
            };
        }
    }
}