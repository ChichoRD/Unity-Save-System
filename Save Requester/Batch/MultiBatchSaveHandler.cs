using SaveSystem.SaveRequester.Handler;
using SaveSystem.SaveService;
using UnityEngine;

namespace SaveSystem.SaveRequester.Batch
{
    [CreateAssetMenu(fileName = "MultiBatchSaveHandler", menuName = "Save System/Save Requester/Batch/Multi Batch Save Handler")]
    internal class MultiBatchSaveHandler : SaveHandler, ISaveHandler
    {
        [SerializeField]
        private SubscribableSaveHandler[] _saveHandlers;

        protected override bool AcceptDeleteRequest(ISaveService saveService, string path)
        {
            bool success = true;
            foreach (ISaveHandler saveHandler in _saveHandlers)
                success &= saveHandler.AcceptDeleteRequest(saveService, path);
            return success;
        }

        protected override bool AcceptLoadRequest(ISaveService saveService, string path)
        {
            bool success = true;
            foreach (ISaveHandler saveHandler in _saveHandlers)
                success &= saveHandler.AcceptLoadRequest(saveService, path);
            return success;
        }

        protected override bool AcceptSaveRequest(ISaveService saveService, string path)
        {
            bool success = true;
            foreach (ISaveHandler saveHandler in _saveHandlers)
                success &= saveHandler.AcceptSaveRequest(saveService, path);
            return success;
        }
    }
}