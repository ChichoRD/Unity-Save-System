using SaveSystem.Saveable;

namespace SaveSystem.SaveRequester
{
    public interface ISaveEventRaiser
    {
        bool Subscribe(IPersistentSaveable persistentSaveable);
        bool Unsubscribe(IPersistentSaveable persistentSaveable);
    }
}