#if UNITY_EDITOR
using SaveSystem.Saveable;
using SaveSystem.SaveRequester.Batch;
using SaveSystem.SaveRequester.SavePath;
using UnityEngine;

namespace SaveSystem.Sample
{
    internal class SaveLoadTest : MonoBehaviour, ISaveable<SaveTestData>, IPersistentSaveable
    {
        [SerializeField]
        private SavePathFlyweight _savePathService;
        [SerializeField]
        private SaveRequester.SaveRequester _saveRequester;
        [SerializeField]
        private SubscribableSaveHandler _saveHandler;
        private PersistentSaveable _persistentSaveable;
        [SerializeField]
        private string _key;

        [SerializeField]
        private SaveTestData _saveTestData;
        public object ID => _persistentSaveable.ID;

        public SaveTestData GetData()
        {
            _saveTestData.testTextureData = _saveTestData.testTexture.GetRawTextureData();
            return _saveTestData;
        }

        public bool TrySetData(SaveTestData saveData)
        {
            _saveTestData = new SaveTestData()
            {
                testInt = saveData.testInt,
                testFloat = saveData.testFloat,
                testString = saveData.testString,
                testTexture = new Texture2D(256, 256),
                testTextureData = saveData.testTextureData,
            };

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            _saveTestData.testTexture.LoadRawTextureData(saveData.testTextureData);
            _saveTestData.testTexture.Apply();
            //stopwatch.Stop();
            //UnityEngine.Debug.Log($"Texture load time: {stopwatch.Elapsed.TotalMilliseconds}ms");

            /*
             * Benchmarking results:
             * PNG Encoding/Decoding: 159kB, 2ms
             * Raw Texture Data Encoding/Decoding: 456kB, 0.6ms
             */

            return true;
        }

        [ContextMenu(nameof(TestInitialize))]
        private void TestInitialize()
        {
            //SaveRequester.Initialize(new JsonSaveService());
            //SaveRequester.Initialize(new XmlSaveService(typeof(SaveTestData)));
            //SaveRequester.Initialize(new BinarySaveService());
        }

        [ContextMenu(nameof(TestSubscribe))]
        private void TestSubscribe()
        {
            _saveHandler.Subscribe(this);
        }

        [ContextMenu(nameof(TestUnsubscribe))]
        private void TestUnsubscribe()
        {
            _saveHandler.Unsubscribe(this);
        }

        [ContextMenu(nameof(TestSave))]
        private void TestSave()
        {
            _saveRequester.Save(_saveHandler, _savePathService.GetPath());
        }

        [ContextMenu(nameof(TestLoad))]
        private void TestLoad()
        {
            _saveRequester.Load(_saveHandler, _savePathService.GetPath());
        }

        private void OnValidate()
        {
            _persistentSaveable = new PersistentSaveable(this, _key);
        }

        object IPersistentSaveable.GetData() => _persistentSaveable.GetData();
        bool IPersistentSaveable.TrySetData<T>(T saveData) => _persistentSaveable.TrySetData(saveData);
    }
}
#endif