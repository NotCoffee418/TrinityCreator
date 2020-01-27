namespace TrinityCreator.Data
{
    public class ItemMaterial : IKeyValue
    {
        public ItemMaterial(int id, string description)
            : base(id, description) { }

        internal static ItemMaterial GetConsumable()
        {
            return new ItemMaterial(-1, "Consumable");
        }

        internal static ItemMaterial GetUndefined()
        {
            return new ItemMaterial(1, "Not Defined");
        }

        internal static ItemMaterial GetPlate()
        {
            return new ItemMaterial(6, "Plate");
        }

        internal static ItemMaterial GetChain()
        {
            return new ItemMaterial(5, "Chainmail");
        }

        internal static ItemMaterial GetLeather()
        {
            return new ItemMaterial(8, "Leather");
        }

        internal static ItemMaterial GetCloth()
        {
            return new ItemMaterial(7, "Cloth");
        }
    }
}