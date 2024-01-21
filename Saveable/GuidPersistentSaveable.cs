using RequireAttributes;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaveSystem.Saveable
{
    internal class GuidPersistentSaveable : MonoBehaviour, IPersistentSaveable
    {
        [SerializeField][HideInInspector] private string _id = string.Empty;
        public string ID => _id;

        [RequireInterface(typeof(ISaveable))]
        [SerializeField] private Object _saveableObject;
        public ISaveable Saveable => _saveableObject as ISaveable;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
                _id = Guid.NewGuid().ToString();
        }

        public object GetSaveData() => Saveable?.GetSaveData();
        public bool TrySetSaveData(object saveData) => Saveable != null && Saveable.TrySetSaveData(saveData);
    }
}