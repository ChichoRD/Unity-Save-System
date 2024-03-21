using SaveSystem.Saveable;

namespace SaveSystem.SaveRequester.Batch
{
    public interface ISaveBatch
    {
        bool Subscribe<TSaveable>(TSaveable persistentSaveable)
            where TSaveable : IPersistentSaveable;
        bool Unsubscribe<TSaveable>(TSaveable persistentSaveable)
            where TSaveable : IPersistentSaveable;
    }
}