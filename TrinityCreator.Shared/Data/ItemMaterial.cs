namespace TrinityCreator.Shared.Data
{
    public class ItemMaterial : IKeyValue
    {
        public ItemMaterial(int id, string description)
            : base(id, description) { }

        public static ItemMaterial GetConsumable()
        {
            return new ItemMaterial(-1, "Consumable");
        }

        public static ItemMaterial GetUndefined()
        {
            return new ItemMaterial(1, "Not Defined");
        }

        public static ItemMaterial GetPlate()
        {
            return new ItemMaterial(6, "Plate");
        }

        public static ItemMaterial GetChain()
        {
            return new ItemMaterial(5, "Chainmail");
        }

        public static ItemMaterial GetLeather()
        {
            return new ItemMaterial(8, "Leather");
        }

        public static ItemMaterial GetCloth()
        {
            return new ItemMaterial(7, "Cloth");
        }
    }
}