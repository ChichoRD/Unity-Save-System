using RequireAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaveSystem.Saveable
{
    internal class TagPersistentSaveable : MonoBehaviour, IPersistentSaveable
    {
        [SerializeField][TextArea] private string _saveTag = string.Empty;
        [SerializeField] private bool _useHash;
        [SerializeField][HideInInspector] private string _previousSaveTag = string.Empty;
        [SerializeField][HideInInspector] private int _tagHash = 0;

        [RequireInterface(typeof(ISaveable))]
        [SerializeField] private Object _saveableObject;
        public ISaveable Saveable => _saveableObject as ISaveable;

        public string ID => _useHash ? _tagHash.ToString() : _saveTag;

        private void OnValidate()
        {
            if (_previousSaveTag == _saveTag) return;
            _previousSaveTag = _saveTag;
            _tagHash = _saveTag.GetStableHash();
        }

        public object GetSaveData() => Saveable?.GetSaveData();
        public bool TrySetSaveData(object saveData) => Saveable != null && Saveable.TrySetSaveData(saveData);
    }
}