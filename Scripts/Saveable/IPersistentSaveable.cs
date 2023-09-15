public interface IPersistentSaveable
{
    string ID { get; }
    ISaveable Saveable { get; }
}