namespace SaveSystem.Saveable
{
    public interface IWriteOnlySaveable
    {
        bool TrySetData<T>(T saveData);
    }

    public interface IWriteOnlySaveable<T> : IWriteOnlySaveable
    {
        bool IWriteOnlySaveable.TrySetData<U>(U saveData) =>
            saveData is T t && TrySetData(t);
        bool TrySetData(T saveData);
    }
}