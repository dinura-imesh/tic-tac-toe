using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using RSG;
using Scripts;

public class SaveSytem : MonoBehaviour
{
    private static SaveSytem instance;
    
    private List<String> pathList = new List<string>();

    private string extensionType = "technoob";
    
    private void Awake()
    {
        #region SINGLETON

        if (instance == null)
            instance = this;
        else if(instance!= this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        #endregion
    }

    /// <summary>
    /// Saves the class passed to this method
    /// </summary>
    /// <param name="saveData">Class which is extended by SaveData class</param>
    /// <param name="_datapacketId">This should be unique for each class you save</param>
    public static void SaveGame(SaveData saveData , string _datapacketId)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath;
        
        if (!instance.pathList.Contains(_datapacketId))
            instance.pathList.Add(_datapacketId);
        
        path = Application.persistentDataPath + "/" + _datapacketId + instance.extensionType;
        
        FileStream stream;
            
        if (File.Exists(path))
            stream= new FileStream(path, FileMode.Open);
        
        else
            stream = new FileStream(path, FileMode.CreateNew);
        
        binaryFormatter.Serialize(stream , saveData);

        stream.Close();
            
        int x = instance.pathList.Count;
        
        String[] paths = new string[x];
        
        for (int i = 0; i < x; i++)
        {
            paths[i] = instance.pathList[i];
        }

        path = Application.persistentDataPath + "/settings.technoob";
        
        if (File.Exists(path))
            stream= new FileStream(path, FileMode.Open);
        
        else
            stream = new FileStream(path, FileMode.CreateNew);
        
        
        binaryFormatter.Serialize(stream , paths);
        
        stream.Close();
     }

    /// <summary>
    /// Returns saved class for the unique string id
    /// </summary>
    /// <param name="_dataPacketId">Unique string id for the saved class</param>
    /// <returns></returns>
    public static SaveData GetSaveData(string _dataPacketId)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + _dataPacketId + instance.extensionType;
        
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData _data = binaryFormatter.Deserialize(stream) as SaveData;
            stream.Close();
            return _data;
        }
        else
        {
            Debug.LogError("Data packet " + _dataPacketId + " not found!  - SaveSystem");
            return null;
        }
        
        
    }

    #region INITIALIZATION

    
    public static IPromise InitSaveSystem()
    {
        Promise saveSystemInitPromise = new Promise();
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/settings.technoob" ;
        
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            String[] paths =  binaryFormatter.Deserialize(stream) as string[];

            for (int i = 0; i < paths.Length; i++)
            {
                instance.pathList.Add(paths[i]);
            }
            stream.Close();
        }
       Debug.Log("Init Save System");
        
       saveSystemInitPromise.Resolve();
        return saveSystemInitPromise;
    }
    
    #endregion
}
