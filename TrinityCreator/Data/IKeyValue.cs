namespace TrinityCreator.Data
{
    public abstract class IKeyValue
    {
        public IKeyValue() { }
        public IKeyValue(int id, string description)
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
    }
}