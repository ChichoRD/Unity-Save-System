namespace SaveSystem.Saveable
{
    public interface IPersistentSaveable : ISaveable
    {
        string ID { get; }
    }
}