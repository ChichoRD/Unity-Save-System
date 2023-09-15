using System.IO;
using System.Xml.Serialization;

public class XmlSaveService : ISaveService
{
    public string PreferredFileExtension { get; } = ".xml";
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

        var serializer = new XmlSerializer(typeof(T));
        using var stream = new FileStream(path, FileMode.Open);
        data = (T)serializer.Deserialize(stream);
        stream.Close();

        return true;
    }

    public bool Save<T>(T data, string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();

        return true;
    }
}
