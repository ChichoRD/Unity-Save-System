using SaveSystem.SaveRequester.Batch;
using UnityEngine;

namespace SaveSystem.Saveable.Auxiliar
{
    internal class InitializationSaveableSubscriber : MonoBehaviour
    {
        [SerializeField]
        private SubscribableSaveHandler _batchSaveHandler;
        private ISaveBatch _saveBatch;
        private IPersistentSaveable _persistentSaveable;

        [SerializeField]
        private bool _subscribeOnEnable = true;

        [SerializeField]
        private bool _unsubscribeOnDisable = true;

        [SerializeField]
        private bool _unsubscribeOnDestroy = false;

        private void Awake()
        {
            _saveBatch = GetComponentInChildren<ISaveBatch>() ?? _batchSaveHandler;
            _persistentSaveable = GetComponentInChildren<IPersistentSaveable>();
        }

        private void OnEnable() => _ = _subscribeOnEnable && _saveBatch.Subscribe(_persistentSaveable);

        private void OnDisable() => _ = _unsubscribeOnDisable && _saveBatch.Unsubscribe(_persistentSaveable);

        private void OnDestroy() => _ = _unsubscribeOnDestroy && _saveBatch.Unsubscribe(_persistentSaveable);
    }
}