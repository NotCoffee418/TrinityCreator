using System.Collections.Generic;

namespace TrinityCreator
{
    public class InhabitType : IKeyValue
    {
        public InhabitType(int id, string description)
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

        public static InhabitType[] GetMovementTypes()
        {
            return new InhabitType[]
            {
                new InhabitType(1, "Ground"),
                new InhabitType(2, "Water"),
                new InhabitType(3, "Flying"),
            };
        }
    }
}