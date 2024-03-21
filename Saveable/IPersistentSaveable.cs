namespace SaveSystem.Saveable
{
    public interface IPersistentSaveable
    {
        object GetData();
        bool TrySetData<T>(T saveData);
        object ID { get; }
    }
}