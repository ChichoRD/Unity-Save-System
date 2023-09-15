using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;

[Obsolete]
public static class SaveUtils
{
    public static readonly string s_defaultSaveDataSubFolder = "SaveData/";
    public static readonly string s_defaultImageSubFolder = "Images/";

    public static readonly string s_defaultDataPath = Application.persistentDataPath;
    public static readonly string s_defaultImagePath = $"{s_defaultDataPath}/{s_defaultImageSubFolder}";
    public static readonly string s_defaultSaveDataPath = $"{s_defaultDataPath}/{s_defaultSaveDataSubFolder}";
    public static string StreamPath(string fileName, string path = null) => $"{path ?? s_defaultDataPath}/{fileName}";

    //TODO - Async Methods

    public static bool Save<T>(T dataToSave, string fileName, string path = null)
    {
        path ??= s_defaultSaveDataPath;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileName, path), FileMode.Create);
        serializer.Serialize(stream, dataToSave);
        stream.Close();

        return true;
    }

    public static bool Load<T>(string fileName, string path, out T data)
    {
        path ??= s_defaultSaveDataPath;
        data = default;

        if (!File.Exists(StreamPath(fileName, path))) return false;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileName, path), FileMode.Open);
        data = (T)serializer.Deserialize(stream);
        stream.Close();

        return true;
    }

    public static bool Delete(string fileName, string path = null)
    {
        path ??= s_defaultSaveDataPath;
        if (!File.Exists(StreamPath(fileName, path))) return false;

        File.Delete(StreamPath(fileName, path));
        return true;
    }

    public static bool SaveImage(Texture2D texture, string fileName, string path = null)
    {
        path ??= s_defaultSaveDataPath;

        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(StreamPath(fileName, path), bytes);
        return true;
    }

    public static bool SaveImage(RenderTexture texture, string fileName, string path = null) => SaveImage(ToTexture2D(texture), fileName, path);

    public static bool LoadImage(string fileName, string path, out Texture2D texture)
    {
        path ??= s_defaultImagePath;
        texture = new Texture2D(2, 2);
        if (!File.Exists(StreamPath(fileName, path))) return false;

        var bytes = File.ReadAllBytes(StreamPath(fileName, path));
        texture.LoadImage(bytes);

        return true;
    }

    public static Texture2D ToTexture2D(RenderTexture renderTexture, TextureFormat textureFormat = TextureFormat.RGBA32)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public static RenderTexture ToRenderTexture(Texture2D texture2D)
    {
        RenderTexture renderTexture = new RenderTexture(texture2D.width, texture2D.height, 0);
        RenderTexture.active = renderTexture;
        Graphics.Blit(texture2D, renderTexture);
        return renderTexture;
    }
}