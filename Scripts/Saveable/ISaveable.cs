public interface ISaveable
{
    object GetSaveData();
    bool TrySetSaveData(object saveData);
}