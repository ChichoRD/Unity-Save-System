using Newtonsoft.Json;
using UnityEditor.PackageManager;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SaveSystem.SaveService.Factory
{
    [CreateAssetMenu(fileName = "JsonSaveServiceFactory", menuName = "Save System/Save Service/Json Factory")]
    internal class JsonSaveServiceFactory : SaveServiceFactory
    {
        private bool _hasJsonSerializerSettings = false;
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();

        public override ISaveService Create() => CreateService();
        public JsonSaveService CreateService() =>
            _hasJsonSerializerSettings
            ? new JsonSaveService(_jsonSerializerSettings)
            : new JsonSaveService();

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        internal static void SubscribeToEvent()
        {
            Client.Add("com.unity.nuget.newtonsoft-json");

            /* new PackageInfo
             * {
             *      name = "Newtonsoft.Json",
             *      source = PackageSource.Embedded,
             *      version = "12.0.301"
             *  }
             */
        }
#endif

        public JsonSaveServiceFactory WithJsonSerializerSettings(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings;
            _hasJsonSerializerSettings = true;
            return this;
        }

        public JsonSaveServiceFactory WithoutJsonSerializerSettings()
        {
            _hasJsonSerializerSettings = false;
            return this;
        }

        private void OnEnable()
        {
            _hasJsonSerializerSettings = false;
            _jsonSerializerSettings = new JsonSerializerSettings();
        }
    }
}