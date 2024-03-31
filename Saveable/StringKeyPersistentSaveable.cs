using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SaveSystem.Saveable
{
    internal class StringKeyPersistentSaveable : MonoBehaviour, IPersistentSaveable
    {
        [SerializeField]
        [TextArea]
        private string _key = string.Empty;
        [SerializeField]
        private bool _hashKey;
        private string _keyHash;

        private ISaveable _saveable;
        public object ID => _hashKey ? _keyHash : _key;

        private void Awake()
        {
            if (_hashKey)
            {
                UTF8Encoding encoding = new UTF8Encoding();
                _keyHash = encoding.GetString(SHA1.Create().ComputeHash(encoding.GetBytes(_key)));
            }

            _saveable = GetComponent<ISaveable>() ?? ISaveable.Null;
        }

        public object GetData() => _saveable.GetData();
        public bool TrySetData<T>(T saveData) => _saveable.TrySetData(saveData);
    }
}