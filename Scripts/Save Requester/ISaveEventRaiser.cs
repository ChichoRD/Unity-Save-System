public interface ISaveEventRaiser
{
    bool Subscribe(IPersistentSaveable persistentSaveable);
    bool Unsubscribe(IPersistentSaveable persistentSaveable);
}