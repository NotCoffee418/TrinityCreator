namespace TrinityCreator
{
    internal interface IKeyValue
    {
        int Id { get; set; }

        string Description { get; set; }

        string ToString();
    }
}