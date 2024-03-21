using UnityEngine;

namespace SaveSystem.SaveService.Factory
{
    [CreateAssetMenu(fileName = "BinarySaveServiceFactory", menuName = "Save System/Save Service/Binary Factory")]
    internal class BinarySaveServiceFactory : SaveServiceFactory
    {
        public override ISaveService Create() => new BinarySaveService();
        public BinarySaveService CreateService() => new BinarySaveService();
    }
}