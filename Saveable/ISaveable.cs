namespace SaveSystem.Saveable
{
    public interface ISaveable
    {
        object GetData();
        bool TrySetData<T>(T saveData);

        static NullSaveable Null => NullSaveable.Instance;
        readonly struct NullSaveable : ISaveable
        {
            public static readonly NullSaveable Instance = new NullSaveable();
            public object GetData() => new object();
            public bool TrySetData<T>(T saveData) => false;
        }
    }

    public interface ISaveable<T> : ISaveable
    {
        object ISaveable.GetData() => GetData();
        bool ISaveable.TrySetData<U>(U saveData) =>
            saveData is T t && TrySetData(t);

        new T GetData();
        bool TrySetData(T saveData);
    }
}