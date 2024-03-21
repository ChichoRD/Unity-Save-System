namespace SaveSystem.Saveable
{
    public interface IReadOnlySaveable
    {
        object GetData();
    }

    public interface IReadOnlySaveable<T> : IReadOnlySaveable
    {
        object IReadOnlySaveable.GetData() => GetData();
        new T GetData();
    }
}