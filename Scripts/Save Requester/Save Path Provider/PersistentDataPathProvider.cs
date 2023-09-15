using UnityEngine;

public readonly struct PersistentDataPathProvider : ISavePathProvider
{
    public readonly string GetSavePath() => Application.persistentDataPath;
}
