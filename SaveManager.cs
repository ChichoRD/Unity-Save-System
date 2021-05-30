using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Reflection;

public enum DocType
{
    PlayerData,
    GeneralData,
    CustomizationData,
}

public static class SaveManager
{
    public static PLayerData pLayerData = new PlayerData();
    public static GeneralData generalData = new GeneralData();
    public static CustomizationData customizationData = new CustomizationData();

    private static readonly string myFileNameExtension = ".xml";

    public static void Save<T>(T dataToSave, string fileName)
    {
        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(dataPath + "/" + fileName + myFileNameExtension, FileMode.Create);
        serializer.Serialize(stream, dataToSave);
        stream.Close();

        Debug.Log("Saved");

    }

    public static void Save<T>(T dataToSave, DocType fileType)
    {
        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(dataPath + "/" + GetFileName(fileType) + myFileNameExtension, FileMode.Create);
        serializer.Serialize(stream, dataToSave);
        stream.Close();

        Debug.Log("Saved");

    }

    public static void Load<T>(out T dataToLoad, string fileName) where T : class, new()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + fileName + myFileNameExtension))
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(dataPath + "/" + fileName + myFileNameExtension, FileMode.Open);
            dataToLoad = serializer.Deserialize(stream) as T;
            stream.Close();

            Debug.Log("Loaded");
        }
        else
        {
            dataToLoad =  new T();
        }
    }

    public static void Load<T>(out T dataToLoad, DocType fileType) where T : class, new()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + GetFileName(fileType) + myFileNameExtension))
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(dataPath + "/" + GetFileName(fileType) + myFileNameExtension, FileMode.Open);
            dataToLoad = serializer.Deserialize(stream) as T;
            stream.Close();

            Debug.Log("Loaded");
        }
        else
        {
            dataToLoad = new T();
        }
    }

    public static T Load<T>(string fileName) where T : class, new()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + fileName + myFileNameExtension))
        {
            T data;

            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(dataPath + "/" + fileName + myFileNameExtension, FileMode.Open);
            data = serializer.Deserialize(stream) as T;
            stream.Close();

            Debug.Log("Loaded");

            return data;
        }
        else
        {
            return new T();
        }
    }

    public static T Load<T>(DocType fileType) where T : class, new()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + GetFileName(fileType) + myFileNameExtension))
        {
            T data;

            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(dataPath + "/" + GetFileName(fileType) + myFileNameExtension, FileMode.Open);
            data = serializer.Deserialize(stream) as T;
            stream.Close();

            Debug.Log("Loaded");

            return data;
        }
        else
        {
            return new T();
        }
    }

    public static void DeleteSavedData<T>(string fileName) where T : class
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + fileName + myFileNameExtension))
        {
            File.Delete(dataPath + "/" + fileName + myFileNameExtension);

            Debug.Log("Deleted data");
        }
    }

    public static void DeleteSavedData<T>(DocType fileType) where T : class
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + GetFileName(fileType) + myFileNameExtension))
        {
            File.Delete(dataPath + "/" + GetFileName(fileType) + myFileNameExtension);

            Debug.Log("Deleted data");
        }
    }

    public static void LoadAndSave<T>(out T dataToLoadAndSave, DocType docType, Action codeBetween = null) where T : class, new()
    {
        dataToLoadAndSave = Load<T>(docType);

        codeBetween?.Invoke();

        Save(dataToLoadAndSave, docType);
    }

    public static T SaveAndLoad<T>(T dataToLoadAndSave, DocType docType, Action codeBetween = null) where T : class, new()
    {
        Save(dataToLoadAndSave, docType);

        codeBetween?.Invoke();

        return Load<T>(docType);
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
}

[System.Serializable]
public sealed class GeneralData
{

}

[System.Serializable]
public sealed class PLayerData
{

}

[System.Serializable]
public sealed class CustomizationData
{

}
