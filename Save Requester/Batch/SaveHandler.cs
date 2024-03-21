using SaveSystem.SaveRequester.Handler;
using SaveSystem.SaveService;
using UnityEngine;

namespace SaveSystem.SaveRequester.Batch
{
    public abstract class SaveHandler : ScriptableObject, ISaveHandler
    {
        bool ISaveHandler.AcceptDeleteRequest(ISaveService saveService, string path) =>
            AcceptDeleteRequest(saveService, path);
        protected abstract bool AcceptDeleteRequest(ISaveService saveService, string path);

        bool ISaveHandler.AcceptLoadRequest(ISaveService saveService, string path) =>
            AcceptLoadRequest(saveService, path);
        protected abstract bool AcceptLoadRequest(ISaveService saveService, string path);

        bool ISaveHandler.AcceptSaveRequest(ISaveService saveService, string path) =>
            AcceptSaveRequest(saveService, path);
        protected abstract bool AcceptSaveRequest(ISaveService saveService, string path);
    }
}