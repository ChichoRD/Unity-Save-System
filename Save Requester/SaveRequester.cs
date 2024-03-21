using SaveSystem.SaveRequester.Handler;
using SaveSystem.SaveService.Factory;
using UnityEngine;

namespace SaveSystem.SaveRequester
{
    [CreateAssetMenu(fileName = "SaveRequester", menuName = "Save System/Save Requester/Save Requester")]
    internal class SaveRequester : ScriptableObject, ISaveRequester
    {
        [SerializeField]
        private SaveServiceFactory _saveServiceFactory;

        public bool Delete(ISaveHandler saveHandler, string path) =>
            saveHandler.AcceptDeleteRequest(_saveServiceFactory.Create(), path);

        public bool Load(ISaveHandler saveHandler, string path) =>
            saveHandler.AcceptLoadRequest(_saveServiceFactory.Create(), path);

        public bool Save(ISaveHandler saveHandler, string path) =>
            saveHandler.AcceptSaveRequest(_saveServiceFactory.Create(), path);
    }
}