using SaveSystem.Saveable;
using SaveSystem.SaveRequester.Handler;

namespace SaveSystem.SaveRequester.Batch
{
    public abstract class SubscribableSaveHandler : SaveHandler, ISaveBatch, ISaveHandler
    {
        public abstract bool Subscribe<TSaveable>(TSaveable persistentSaveable)
            where TSaveable : IPersistentSaveable;

        public abstract bool Unsubscribe<TSaveable>(TSaveable persistentSaveable)
            where TSaveable : IPersistentSaveable;
    }
}