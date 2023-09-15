using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
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
        return _saveTestData;
    }

    public bool TrySetSaveData(object saveData)
    {
        if (saveData is not SaveTestData saveTestData) return false;

        _saveTestData = saveTestData;
        return true;
    }

    [ContextMenu(nameof(TestInitialize))]
    private void TestInitialize()
    {
        _savePathProviderObject.SaveService = new XmlSaveService();
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

[XmlInclude(typeof(SaveTestData))]
[Serializable]
public struct SaveTestData
{
    public int testInt;
    public float testFloat;
    public string testString;
}
