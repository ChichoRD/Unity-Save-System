using RequireAttributes;
using SaveSystem.SaveRequester.SavePath;
using SaveSystem.SaveService;
using UnityEngine;

namespace SaveSystem.SaveRequester.Auxiliar
{
    internal class InitializationSaver : MonoBehaviour
    {
        [RequireInterface(typeof(ISaveRequester), typeof(ScriptableObject))]
        [SerializeField]
        private Object _saveRequesterObject;
        private ISaveRequester SaveRequester => _saveRequesterObject as ISaveRequester;

        [RequireInterface(typeof(ISavePathProvider), typeof(ScriptableObject))]
        [SerializeField]
        private Object _savePathProviderObject;
        private ISavePathProvider SavePathProvider => _savePathProviderObject as ISavePathProvider;

        [SerializeReference]
        private ISaveService _saveService;
#if UNITY_EDITOR
        private string _debugSaveServiceName = string.Empty;
#endif

        [SerializeField]
        private bool _loadOnStart = true;

        [SerializeField]
        private bool _saveOnQuit = true;

        //[SerializeField]
        //private bool _saveOnDisable = true;

        private void Awake()
        {
            SaveRequester.Initialize(_saveService);
        }

        private void Start()
        {
            if (_loadOnStart)
                Load();
        }

        //private void OnDisable()
        //{
        //    if (_saveOnDisable)
        //        Save();
        //}

        private void OnApplicationQuit()
        {
            if (_saveOnQuit)
                Save();
        }

        public void Load() => SaveRequester.Load(SavePathProvider.GetSavePath());
        public void Save() => SaveRequester.Save(SavePathProvider.GetSavePath());
        public void Delete() => SaveRequester.Delete(SavePathProvider.GetSavePath());

        [ContextMenu(nameof(SetJsonSaveService))]
        private void SetJsonSaveService()
        {
            _saveService = new JsonSaveService();
#if UNITY_EDITOR
            _debugSaveServiceName = _saveService.ToString();
#endif
        }

        [ContextMenu(nameof(SetXmlSaveService))]
        private void SetXmlSaveService()
        {
            _saveService = new XmlSaveService();
#if UNITY_EDITOR
            _debugSaveServiceName = _saveService.ToString();
#endif
        }

        [ContextMenu(nameof(SetBinarySaveService))]
        private void SetBinarySaveService()
        {
            _saveService = new BinarySaveService();
#if UNITY_EDITOR
            _debugSaveServiceName = _saveService.ToString();
#endif
        }
    }
}