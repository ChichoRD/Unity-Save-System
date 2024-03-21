using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace SaveSystem.SaveService
{
    [Serializable]
    internal class XmlSaveService : ISaveService, IFileExtensionService
    {
        private readonly IEnumerable<Type> _knownTypes = new Type[0];
        public string GetFileExtension() => ".json";

        public XmlSaveService(params Type[] knownTypes)
        {
            _knownTypes = knownTypes;
        }

        public XmlSaveService(IEnumerable<Type> knownTypes)
        {
            _knownTypes = knownTypes;
        }

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

            var serializer = new DataContractSerializer(typeof(T), _knownTypes);
            using var streamReader = new StreamReader(path);
            using var xmlReader = new XmlTextReader(streamReader);

            data = (T)serializer.ReadObject(xmlReader);

            xmlReader.Close();
            streamReader.Close();

            return true;
        }

        public bool Save<T>(T data, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var serializer = new DataContractSerializer(typeof(T), _knownTypes);

            using var streamWriter = new StreamWriter(path);
            using var xmlWriter = new XmlTextWriter(streamWriter);

            serializer.WriteObject(xmlWriter, data);

            xmlWriter.Close();
            streamWriter.Close();

            return true;
        }
    }
}