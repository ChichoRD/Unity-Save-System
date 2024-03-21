namespace SaveSystem.Saveable
{
    public readonly struct PersistentSaveable : IPersistentSaveable
    {
        private readonly ISaveable _saveable;
        private readonly object _key;

        public PersistentSaveable(ISaveable saveable, object key)
        {
            _saveable = saveable;
            _key = key;
        }

        public object GetData() => _saveable.GetData();
        public bool TrySetData<T>(T saveData) => _saveable.TrySetData(saveData);
        public object ID => _key;
    }
}