namespace SaveSystem.Saveable
{
    public interface IPersistentSaveable
    {
        ISaveable Saveable { get; }
        string ID { get; }
    }
}