using UnityEngine;

namespace SaveSystem.SaveService.Factory
{
    public abstract class SaveServiceFactory : ScriptableObject
    {
        public abstract ISaveService Create();
    }
}