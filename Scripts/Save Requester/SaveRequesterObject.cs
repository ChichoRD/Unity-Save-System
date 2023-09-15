using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = SAVE_REQUESTER_NAME, menuName = SAVER_REQUESTER_PATH + SAVE_REQUESTER_NAME)]
public class SaveRequesterObject : ScriptableObject, ISaveRequester, ISaveEventRaiser
{
    private const string SAVE_REQUESTER_NAME = "Save Requester";
    private const string SAVER_REQUESTER_PATH = "Save System/";

    private ISaveService _saveService;
    private List<IPersistentSaveable> _persistentSaveables;

    public class SaveDictionary : SerializableDictionary<string, object> { }

    public bool Delete(string path) => _saveService.Delete(path);

    public bool Load(string path)
    {
        if (!_saveService.Load(out SaveDictionary data, path)) return false;

        foreach (var persistentSaveable in _persistentSaveables)
        {
            if (!data.TryGetValue(persistentSaveable.ID, out var value)) continue;
            persistentSaveable.Saveable.TrySetSaveData(value);
        }
        return true;
    }

    public bool Save(string path)
    {
        SaveDictionary data = new SaveDictionary();
        foreach (var persistentSaveable in _persistentSaveables)
            data.Add(persistentSaveable.ID, persistentSaveable.Saveable.GetSaveData());

        return _saveService.Save(data, path);
    }

    public bool Subscribe(IPersistentSaveable persistentSaveable)
    {
        if (_persistentSaveables.Contains(persistentSaveable)) return false;
        _persistentSaveables.Add(persistentSaveable);
        return true;
    }

    public bool Unsubscribe(IPersistentSaveable persistentSaveable) => _persistentSaveables.Remove(persistentSaveable);

    public bool Initialize(ISaveService saveService)
    {
        _saveService = saveService;
        _persistentSaveables = new List<IPersistentSaveable>();
        return true;
    }
}