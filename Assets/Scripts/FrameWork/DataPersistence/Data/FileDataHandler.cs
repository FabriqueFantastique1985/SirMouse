using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Combine is used because we will be working with different OS systems that have different path separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load serialized data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // deserialize data from json into C# object
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);// JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error when loading data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        
        try
        {
            // create directory if it doesn't already exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize data to json
            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented, 
                new JsonSerializerSettings 
                { 
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                });// JsonUtility.ToJson(data, true);
            
            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when saving file: " + fullPath + "\n" + e);
        }
    }

    /// <summary>
    /// Temporary method to clear save files 
    /// </summary>
    public void ClearSaveFiles()
    {
        if (Directory.Exists(_dataDirPath)) 
        { 
            Directory.Delete(_dataDirPath, true);
        }
        Directory.CreateDirectory(_dataDirPath);
    }
}
