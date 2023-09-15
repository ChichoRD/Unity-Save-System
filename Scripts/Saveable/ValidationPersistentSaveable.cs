using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class ValidationPersistentSaveable : MonoBehaviour, IPersistentSaveable
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
}
