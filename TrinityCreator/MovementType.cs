using System.Collections.Generic;

namespace TrinityCreator
{
    public class MovementType : IKeyValue
    {
        public MovementType(int id, string description)
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

        public static MovementType[] GetMovementTypes()
        {
            return new MovementType[]
            {
                new MovementType(0, "Stay in one place"),
                new MovementType(2, "Waypoint Movement"),
                new MovementType(1, "Random Movement inside spawn radius"),                
            };
        }
    }
}