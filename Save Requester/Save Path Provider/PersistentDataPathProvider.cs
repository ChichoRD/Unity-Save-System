using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    internal readonly struct PersistentDataPathProvider : ISavePathProvider
    {
        public readonly string GetSavePath() => Application.persistentDataPath;
    }
}