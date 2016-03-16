namespace TrinityCreator
{
    public class ItemBonding : IKeyValue
    {
        public ItemBonding(int id, string name)
        {
            Id = id;
            Description = name;
        }


        public int Id { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }

        public static ItemBonding[] GetItemBondingList()
        {
            return new[]
            {
                new ItemBonding(0, "No bounds"),
                new ItemBonding(1, "Binds when picked up"),
                new ItemBonding(2, "Binds when equipped"),
                new ItemBonding(3, "Binds when used"),
                new ItemBonding(4, "Quest item")
            };
        }
    }
}