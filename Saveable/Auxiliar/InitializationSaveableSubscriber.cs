using RequireAttributes;
using SaveSystem.SaveRequester;
using UnityEngine;

namespace SaveSystem.Saveable.Auxiliar
{
    internal class InitializationSaveableSubscriber : MonoBehaviour
    {
        [RequireInterface(typeof(ISaveEventRaiser), typeof(ScriptableObject))]
        [SerializeField]
        private Object _saveEventRaiserObject;
        private ISaveEventRaiser SaveEventRaiser => _saveEventRaiserObject as ISaveEventRaiser;

        [RequireInterface(typeof(IPersistentSaveable))]
        [SerializeField]
        private Object _persistentSaveableObject;
        private IPersistentSaveable PersistentSaveable => _persistentSaveableObject as IPersistentSaveable;

        [SerializeField]
        private bool _subscribeOnEnable = true;

        [SerializeField]
        private bool _unsubscribeOnDisable = true;

        //[SerializeField]
        //private bool _unsubscribeOnDestroy = true;

        private void OnEnable() => _ = !_subscribeOnEnable || SaveEventRaiser.Subscribe(PersistentSaveable);

        private void OnDisable() => _ = !_unsubscribeOnDisable || SaveEventRaiser.Unsubscribe(PersistentSaveable);

        //private void OnDestroy() => _ = !_unsubscribeOnDestroy || SaveEventRaiser.Unsubscribe(PersistentSaveable);
    }
}