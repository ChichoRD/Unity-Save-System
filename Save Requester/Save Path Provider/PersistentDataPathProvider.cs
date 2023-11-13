using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    public readonly struct PersistentDataPathProvider : ISavePathProvider
    {
        public readonly string GetSavePath() => Application.persistentDataPath;
    }
}