using System.IO;
using System.Runtime.Serialization.Json;

public class JsonSaveService : ISaveService
{
    public string PreferredFileExtension { get; } = ".json";
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

        var serializer = new DataContractJsonSerializer(typeof(T));
        using var stream = new FileStream(path, FileMode.Open);
        data = (T)serializer.ReadObject(stream);
        stream.Close();

        return true;
    }

    public bool Save<T>(T data, string path)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));
        using var stream = new FileStream(path, FileMode.Create);
        serializer.WriteObject(stream, data);
        stream.Close();

        return true;
    }
}