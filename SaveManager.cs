/*
Copyright(c) 2021 Chicho Studio

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Reflection;
using UnityEngine.Rendering.Universal;
using ShadowQuality = UnityEngine.ShadowQuality;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

public enum DocType
{
    PlayerData,
    GeneralData,
    CustomizationData,
}

public static class SaveManager
{
    private static readonly List<object> unsavedData = new();

    private static PLayerData pLayerData = new();
    private static GeneralData generalData = new();
    private static CustomizationData customizationData = new();

    private static readonly string s_fileExtension = ".xml";
    private static readonly string s_imageFileExtension = ".png";
    private static readonly string s_overrideWarning = "Warning: Tried to load data with unsaved changes";

    public static Action<object> OnSaved { get; set; }
    public static Action<object> OnLoaded { get; set; }
    public static Action OnDeleted { get; set; }

    public static PLayerData PLayerData
    {
        get => pLayerData;
        set
        {
            pLayerData = value;
            unsavedData.Add(value);
        }
    }

    public static GeneralData GeneralData
    {
        get => generalData;
        set
        {
            generalData = value;
            unsavedData.Add(value);
        }
    }

    public static CustomizationData CustomizationData
    {
        get => customizationData;
        set
        {
            customizationData = value;
            unsavedData.Add(value);
        }
    }


    private static readonly string DataPath = Application.persistentDataPath;
    private static string StreamPath(string fileName, string dataPath) => dataPath + "/" + fileName + s_fileExtension;
    private static string ImageStreamPath(string fileName, string dataPath) => dataPath + "/" + fileName + s_imageFileExtension;
    private static string StreamPath(DocType fileType, string dataPath) => dataPath + "/" + GetFileName(fileType) + s_fileExtension;

    //TODO - Async Methods
    
    public static void Save<T>(T dataToSave, string fileName)
    {
        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileName, DataPath), FileMode.Create);
        serializer.Serialize(stream, dataToSave);
        stream.Close();

        OnSaved?.Invoke(dataToSave);
        unsavedData.Remove(dataToSave);

        Debug.Log("Saved");
    }

    public static void Save<T>(T dataToSave, DocType fileType)
    {
        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileType, DataPath), FileMode.Create);
        serializer.Serialize(stream, dataToSave);
        stream.Close();

        OnSaved?.Invoke(dataToSave);
        unsavedData.Remove(dataToSave);

        Debug.Log("Saved");
    }

    public static void SaveImage(Texture2D texture, string fileName)
    {
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(ImageStreamPath(fileName, DataPath), bytes);

        OnSaved?.Invoke(texture);
        unsavedData.Remove(texture);

        Debug.Log("Saved Image");
    }

    public static void SaveImage(RenderTexture texture, string fileName) => SaveImage(ToTexture2D(texture), fileName);

    public static T Load<T>(string fileName) where T : class, new()
    {
        T data = new T();
        if (!File.Exists(StreamPath(fileName, DataPath))) return data;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileName, DataPath), FileMode.Open);
        data = serializer.Deserialize(stream) as T;
        stream.Close();

        if (unsavedData.Exists(d => d is T))
        {
            Debug.LogWarning(s_overrideWarning);
            var unsaved = unsavedData.Find(d => d is T);
            return SaveAndLoad((T)unsaved, fileName);
        }

        OnLoaded?.Invoke(data);
        Debug.Log("Loaded");
        return data;
    }


    public static T Load<T>(DocType fileType) where T : class, new()
    {
        T data = new T();
        if (!File.Exists(StreamPath(fileType, DataPath))) return data;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(StreamPath(fileType, DataPath), FileMode.Open);
        data = serializer.Deserialize(stream) as T;
        stream.Dispose();

        if (unsavedData.Exists(d => d is T))
        {
            Debug.LogWarning(s_overrideWarning);
            var unsaved = unsavedData.Find(d => d is T);
            return SaveAndLoad((T)unsaved, fileType);
        }

        OnLoaded?.Invoke(data);
        Debug.Log("Loaded");
        return data;
    }

    public static Texture2D LoadImage(string fileName)
    {
        if (!File.Exists(ImageStreamPath(fileName, DataPath))) return null;

        var bytes = File.ReadAllBytes(ImageStreamPath(fileName, DataPath));
        var texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        OnLoaded?.Invoke(texture);
        Debug.Log("Loaded Image");

        return texture;
    }

    public static Texture2D ToTexture2D(RenderTexture renderTexture)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
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

    public static void DeleteSavedData(string fileName)
    {
        if (File.Exists(StreamPath(fileName, DataPath)))
        {
            File.Delete(StreamPath(fileName, DataPath));

            OnDeleted?.Invoke();
            Debug.Log("Deleted data");
        }
    }

    public static void DeleteSavedData(DocType fileType)
    {
        if (File.Exists(StreamPath(fileType, DataPath)))
        {
            File.Delete(StreamPath(fileType, DataPath));

            OnDeleted?.Invoke();
            Debug.Log("Deleted data");
        }
    }

    public static void LoadAndSave<T>(ref T dataToLoadAndSave, DocType docType, Action<T> codeBetween = null) where T : class, new()
    {
        dataToLoadAndSave = Load<T>(docType);

        codeBetween?.Invoke(dataToLoadAndSave);

        Save(dataToLoadAndSave, docType);
    }

    public static T SaveAndLoad<T>(T dataToLoadAndSave, DocType docType, Action codeBetween = null) where T : class, new()
    {
        Save(dataToLoadAndSave, docType);

        codeBetween?.Invoke();

        return Load<T>(docType);
    }

    public static void LoadAndSave<T>(ref T dataToLoadAndSave, string fileName, Action<T> codeBetween = null) where T : class, new()
    {
        dataToLoadAndSave = Load<T>(fileName);

        codeBetween?.Invoke(dataToLoadAndSave);

        Save(dataToLoadAndSave, fileName);
    }

    public static T SaveAndLoad<T>(T dataToLoadAndSave, string fileName, Action codeBetween = null) where T : class, new()
    {
        Save(dataToLoadAndSave, fileName);

        codeBetween?.Invoke();

        return Load<T>(fileName);
    }

    public static T GetFieldValue<T>(object obj, string fieldName)
    {
        if (obj == null)
            throw new ArgumentNullException("obj");

        var field = obj.GetType().GetField(fieldName, BindingFlags.Public |
                                                      BindingFlags.NonPublic |
                                                      BindingFlags.Instance);

        if (field == null)
            throw new ArgumentException("fieldName", "No such field was found.");

        if (!typeof(T).IsAssignableFrom(field.FieldType))
            throw new InvalidOperationException("Field type and requested type are not compatible.");

        return (T)field.GetValue(obj);
    }

    private static string GetFileName(DocType docType)
    {
        return docType switch
        {
            DocType.PlayerData => nameof(PLayerData),
            DocType.GeneralData => nameof(GeneralData),
            DocType.CustomizationData => nameof(CustomizationData),
            _ => nameof(GeneralData)
        };
    }

    public static void LoadAllData()
    {
        CustomizationData = Load<CustomizationData>(DocType.CustomizationData) ?? new();
        PLayerData = Load<PLayerData>(DocType.PlayerData) ?? new();
        GeneralData = Load<GeneralData>(DocType.GeneralData) ?? new();
    }

    public static void SaveAllData()
    {
        Save(CustomizationData, DocType.CustomizationData);
        Save(PLayerData, DocType.PlayerData);
        Save(GeneralData, DocType.GeneralData);

        unsavedData.Clear();
    }

}

[Serializable]
public sealed class GeneralData
{
    
}

[Serializable]
public sealed class PLayerData
{
    
}

[Serializable]
public sealed class CustomizationData
{
    // Video Settings
    public Vector2Int screenResolution = new(0, 0);
    public FullScreenMode screenMode = FullScreenMode.ExclusiveFullScreen;
    public int frameRate = -1;
    public byte vSyncCount = 1;

    //Audio Settings
    public float rawMasterVolume = 1f;
    public float rawSFXVolume = 1f;
    public float rawMusicVolume = 1f;
    public bool enableTextSpeechSound = true;

    //General Settings
    public float cameraShake = 1f;
    public float fieldOfView = 90f;
    public float sensitivity = 1f;
    public bool visualTips = true;

    //Graphics Settings
    public byte qualityLevel = 2;
    public bool postProcessing = true;
    public ShadowQuality shadowQuality = ShadowQuality.All;
    public ShadowResolution shadowResolution = ShadowResolution._2048;
    public byte shadowCascadesCount = 4;
    public float shadowDistance = 100f;
    public MsaaQuality msaaQuality = MsaaQuality._2x;
    public float renderScale = 1f;
}