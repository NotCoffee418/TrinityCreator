using System.Collections.Generic;

namespace TrinityCreator
{
    public class AI : IKeyValue
    {
        public AI(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public int Id { get; set; } // only used by template, value is string
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }

        public static AI[] GetCreatureAI()
        {
            return new AI[]
            {
                new AI(0, "NullAI"),
                new AI(1, "AggressorAI"),
                new AI(2, "ReactorAI"),
                new AI(3, "GuardAI"),
                new AI(4, "PetAI"),
                new AI(5, "TotemAI"),
                new AI(6, "EventAI"),
                new AI(7, "SmartAI"),
            };
        }
    }
}