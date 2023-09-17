using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = SAVE_REQUESTER_NAME, menuName = SAVER_REQUESTER_PATH + SAVE_REQUESTER_NAME)]
public class SaveRequesterObject : ScriptableObject, ISaveRequester, ISaveEventRaiser
{
    private const string SAVE_REQUESTER_NAME = "Save Requester";
    private const string SAVER_REQUESTER_PATH = "Save System/";

    private ISaveService _saveService;
    private List<IPersistentSaveable> _persistentSaveables;

    public bool Initialized { get; private set; }
    public bool Delete(string path) => _saveService.Delete(path);

    public bool Load(string path)
    {
        if (!_saveService.Load(out Dictionary<string, object> data, path)) return false;

        foreach (var persistentSaveable in _persistentSaveables)
        {
            if (persistentSaveable == null || !data.TryGetValue(persistentSaveable.ID, out var value)) continue;
            persistentSaveable.Saveable.TrySetSaveData(value);
        }
        return true;
    }

    public bool Save(string path)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (var persistentSaveable in _persistentSaveables)
        {
            if (persistentSaveable == null || persistentSaveable.Saveable == null) continue;
            data.Add(persistentSaveable.ID, persistentSaveable.Saveable.GetSaveData());
        }

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
        return Initialized = true;
    }
}