using SaveSystem.Saveable;
using SaveSystem.SaveService;
using System.Collections.Generic;

namespace SaveSystem.SaveRequester.Handler
{
    internal readonly struct SaveHandler : ISaveHandler
    {
        private readonly IEnumerable<IPersistentSaveable> _persistentSaveables;

        public SaveHandler(IEnumerable<IPersistentSaveable> persistentSaveables)
        {
            _persistentSaveables = persistentSaveables;
        }

        public bool AcceptSaveRequest(ISaveService saveService, string path)
        {
            Dictionary<object, object> data = new Dictionary<object, object>();
            foreach (var persistentSaveable in _persistentSaveables)
                data.Add(persistentSaveable.ID, persistentSaveable.GetData());

            return saveService.Save(data, path);
        }

        public bool AcceptLoadRequest(ISaveService saveService, string path)
        {
            if (!saveService.Load(out Dictionary<object, object> data, path))
                return false;

            bool success = true;
            foreach (var persistentSaveable in _persistentSaveables)
                if (data.TryGetValue(persistentSaveable.ID, out object value))
                    success &= persistentSaveable.TrySetData(value);
            return success;
        }

        public bool AcceptDeleteRequest(ISaveService saveService, string path)
        {
            return saveService.Delete(path);
        }
    }
}