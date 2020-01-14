using System.Collections.Generic;

namespace TrinityCreator.Data
{
    public class DamageType : IKeyValue
    {
        public DamageType(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }

        public static DamageType[] GetDamageTypes(bool magicOnly = false)
        {
            var list = new List<DamageType>();
            if (!magicOnly)
                list.Add(new DamageType(0, "Physical"));

            list.AddRange(new[]
            {
                new DamageType(1, "Holy"),
                new DamageType(2, "Fire"),
                new DamageType(3, "Nature"),
                new DamageType(4, "Frost"),
                new DamageType(5, "Shadow"),
                new DamageType(6, "Arcane")
            });

            return list.ToArray();
        }
    }
}