using SaveSystem.Saveable;
using SaveSystem.SaveRequester.Handler;
using SaveSystem.SaveService;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.SaveRequester.Batch
{
    [CreateAssetMenu(fileName = "BatchSaveHandler", menuName = "Save System/Save Requester/Batch Save Handler")]
    internal class BatchSaveHandler : SubscribableSaveHandler, ISaveBatch, ISaveHandler
    {
        private readonly HashSet<IPersistentSaveable> _persistentSaveables = new HashSet<IPersistentSaveable>();

        public override bool Subscribe<TSaveable>(TSaveable persistentSaveable)
        {
            _persistentSaveables.Add(persistentSaveable);
            return true;
        }

        public override bool Unsubscribe<TSaveable>(TSaveable persistentSaveable)
        {
            return _persistentSaveables.Remove(persistentSaveable);
        }

        protected override bool AcceptDeleteRequest(ISaveService saveService, string path) =>
            new Handler.SaveHandler(_persistentSaveables).AcceptDeleteRequest(saveService, path);

        protected override bool AcceptLoadRequest(ISaveService saveService, string path) =>
            new Handler.SaveHandler(_persistentSaveables).AcceptLoadRequest(saveService, path);

        protected override bool AcceptSaveRequest(ISaveService saveService, string path) =>
            new Handler.SaveHandler(_persistentSaveables).AcceptSaveRequest(saveService, path);

        private void OnEnable()
        {
            _persistentSaveables.Clear();
        }
    }
}