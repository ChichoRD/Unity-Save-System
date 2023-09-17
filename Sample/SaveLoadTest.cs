using UnityEngine;
using Object = UnityEngine.Object;

public class SaveLoadTest : MonoBehaviour, ISaveable
{
    //[RequireInterface(typeof(ISavePathProvider))]
    //[SerializeField] private Object _savePathProviderObject;
    [SerializeField] private RelativeSavePathProviderObject _savePathProviderObject;
    private ISavePathProvider SavePathProvider => _savePathProviderObject as ISavePathProvider;

    [RequireInterface(typeof(ISaveRequester))]
    [SerializeField] private Object _saveRequesterObject;
    private ISaveRequester SaveRequester => _saveRequesterObject as ISaveRequester;

    [RequireInterface(typeof(ISaveEventRaiser))]
    [SerializeField] private Object _saveEventRaiserObject;
    private ISaveEventRaiser SaveEventRaiser => _saveEventRaiserObject as ISaveEventRaiser;

    [RequireInterface(typeof(IPersistentSaveable))]
    [SerializeField] private Object _persistentSaveableObject;
    private IPersistentSaveable PersistentSaveable => _persistentSaveableObject as IPersistentSaveable;

    [SerializeField] private SaveTestData _saveTestData;

    public object GetSaveData()
    {
        _saveTestData.testTextureData = _saveTestData.testTexture.GetRawTextureData();
        return _saveTestData;
    }

    public bool TrySetSaveData(object saveData)
    {
        if (saveData is not SaveTestData saveTestData) return false;

        _saveTestData = new SaveTestData()
        {
            testInt = saveTestData.testInt,
            testFloat = saveTestData.testFloat,
            testString = saveTestData.testString,
            testTexture = new Texture2D(256, 256),
            testTextureData = saveTestData.testTextureData,
        };

        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();
        _saveTestData.testTexture.LoadRawTextureData(saveTestData.testTextureData);
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
        _savePathProviderObject.SaveService = new JsonSaveService();
        SaveRequester.Initialize(_savePathProviderObject.SaveService);
    }

    [ContextMenu(nameof(TestSubscribe))]
    private void TestSubscribe()
    {
        SaveEventRaiser.Subscribe(PersistentSaveable);
    }

    [ContextMenu(nameof(TestUnsubscribe))]
    private void TestUnsubscribe()
    {
        SaveEventRaiser.Unsubscribe(PersistentSaveable);
    }

    [ContextMenu(nameof(TestSave))]
    private void TestSave()
    {
        SaveRequester.Save(SavePathProvider.GetSavePath());
    }

    [ContextMenu(nameof(TestLoad))]
    private void TestLoad()
    {
        SaveRequester.Load(SavePathProvider.GetSavePath());
    }
}