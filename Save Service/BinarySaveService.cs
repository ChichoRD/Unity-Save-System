using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveSystem.SaveService
{
    [Serializable]
    internal class BinarySaveService : ISaveService, IFileExtensionService
    {
        public string GetFileExtension() => ".bin";

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

            var serializer = new BinaryFormatter()
            {
                TypeFormat = FormatterTypeStyle.TypesWhenNeeded,
            };

            using var streamReader = new FileStream(path, FileMode.Open);
            using var binaryReader = new BinaryReader(streamReader);

            data = (T)serializer.Deserialize(binaryReader.BaseStream);

            binaryReader.Close();
            streamReader.Close();

            return true;
        }

        public bool Save<T>(T data, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var serializer = new BinaryFormatter()
            {
                TypeFormat = FormatterTypeStyle.TypesWhenNeeded,
            };

            using var streamWriter = new FileStream(path, FileMode.Create);
            using var binaryWriter = new BinaryWriter(streamWriter);

            serializer.Serialize(binaryWriter.BaseStream, data);

            binaryWriter.Close();
            streamWriter.Close();

            return true;
        }
    }
}