using System;
using System.IO;
using Newtonsoft.Json;

namespace SaveSystem.SaveService
{
    [Serializable]
    internal class JsonSaveService : ISaveService, IFileExtensionService
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonSaveService()
        {
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };
        }

        public JsonSaveService(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public string GetFileExtension() => ".json";
        public bool Delete(string path)
        {
            if (!File.Exists(path)) return false;

            File.Delete(path);
            return true;
        }

        public bool Load<T>(out T data, string path)
        {
            data = default;
            if (!File.Exists(path)) return false;

            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            using var streamReader = new StreamReader(path);
            using var jsonReader = new JsonTextReader(streamReader);

            data = serializer.Deserialize<T>(jsonReader);

            jsonReader.Close();
            streamReader.Close();

            return true;
        }

        public bool Save<T>(T data, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            using var streamWriter = new StreamWriter(path);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            serializer.Serialize(jsonWriter, data);

            jsonWriter.Close();
            streamWriter.Close();

            return true;
        }
    }
}