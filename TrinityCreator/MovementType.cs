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
                new MovementType(2, "Waypoint Movement"), // should be default in case user decides to set waypoints
                new MovementType(1, "Random Movement inside spawn radius"),
                new MovementType(0, "Stay in one place"),
            };
        }
    }
}