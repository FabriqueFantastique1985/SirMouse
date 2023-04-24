using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-200)]
public class DataPersistenceManager : MonoBehaviourSingleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [Header("Debugging")]
    [SerializeField]
    private bool _initializeDataIfNull = false;
    
    #region Fields

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    #endregion

    #region Properties
    public bool HasGameData => _gameData != null;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _dataHandler = new FileDataHandler(Path.Combine(Application.persistentDataPath), fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "MainMenu")
        {
            for (int i = DontDestroyOnLoadManager.DontDestroyOnLoadObjects.Count - 1; i >= 0; i--)
            {
                var dontDestroyOnLoadObject = DontDestroyOnLoadManager.DontDestroyOnLoadObjects[i];
                
                // Dont destroy the data persistence manager
                if (dontDestroyOnLoadObject == this.gameObject) continue;
                
                Destroy(dontDestroyOnLoadObject);
                DontDestroyOnLoadManager.DontDestroyOnLoadObjects.RemoveAt(i);
            }
        }
        
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        if (_gameData != null) _gameData.lastActiveScene = arg0.name;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        SaveGame();
    }

    public void NewGame()
    {
        DeleteGameData();
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();
        
        #if UNITY_EDITOR
        if (_gameData == null && _initializeDataIfNull)
        {
            NewGame();
        }
        #endif
        
        if (_gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded");
            return;
        }

        foreach (var dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            Debug.LogWarning("GameData is null. A New Game needs to be started before data can be loaded");
            return;

        }

        _dataPersistenceObjects.RemoveAll(x => !(x as UnityEngine.Object));

        foreach (var datapersistenceobj in _dataPersistenceObjects)
        {
            datapersistenceobj.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    /// <summary>
    /// Temporary method to clear all save files
    /// </summary>
    public void ClearGame()
    {
        _dataHandler.ClearSaveFiles();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
       /*IEnumerable<IDataPersistence> dataPersistenceObjects =
          FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();//  FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

       return new List<IDataPersistence>(dataPersistenceObjects);*/

       //get all root objects in the scene
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            
            List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                var dataPersistenceObject = rootObjects[i].GetComponentsInChildren<IDataPersistence>(true).ToList();
                dataPersistenceObjects.AddRange(dataPersistenceObject);
            }  
            
            //get all rootobjects in dont destroy on load
              var dontDestroyOnLoadRootObjects = DontDestroyOnLoadManager.DontDestroyOnLoadObjects;
              for (int i = 0; i < dontDestroyOnLoadRootObjects.Count; i++)
              {
                  var dataPersistenceObject = dontDestroyOnLoadRootObjects[i].GetComponentsInChildren<IDataPersistence>(true).ToList();
                  dataPersistenceObjects.AddRange(dataPersistenceObject);
              }  
              
            return dataPersistenceObjects;
       //  Scene currentScene = SceneManager.GetActiveScene();
       //  IEnumerable<IDataPersistence> dataPersistenceObjects =
       //      currentScene.GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<IDataPersistence>(true));
       //  return dataPersistenceObjects.ToList();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    private void DeleteGameData()
    {
        _dataHandler.ClearSaveFiles();
        _dataPersistenceObjects.Clear();
    }
}