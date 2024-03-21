using UnityEngine;

namespace SaveSystem.SaveRequester.SavePath
{
    public abstract class SavePathFlyweight : ScriptableObject, ISavePathService
    {
        public abstract string GetPath();
    }
}