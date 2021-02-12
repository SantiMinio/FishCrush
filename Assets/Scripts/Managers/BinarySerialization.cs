using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySerialization
{
    public static string DataPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android) return Application.persistentDataPath;
            else return Application.dataPath;
        }
    }


    public static void Serialize<T>(string name, T data)
    {
        FileStream file = File.Create(DataPath + "/" + name + ".bin");
        var binary = new BinaryFormatter();

        binary.Serialize(file, data);

        file.Close();
    }

    public static T Deserialize<T>(string name)
    {
        if (File.Exists(DataPath + "/" + name + ".bin"))
        {
            FileStream file = File.Open(DataPath + "/" + name + ".bin", FileMode.Open);
            var binary = new BinaryFormatter();

            T data = (T)binary.Deserialize(file);

            file.Close();

            return data;
        }
        else
        {
            UnityEngine.Debug.LogError("No hay archivo creado");
            return default(T);
        }
    }

    public static bool IsFileExist(string path) => File.Exists(DataPath + "/" + path + ".bin");
}
