using System;
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
    
    private void Awake()
    {
        base.Awake();
        _dataHandler = new FileDataHandler(Path.Combine(Application.persistentDataPath, "Scenes"), SceneManager.GetActiveScene().name + "_" + fileName);
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
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        
        LoadGame();
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        SaveGame();
    }

    public void NewGame()
    {
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
        foreach (var dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
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
        IEnumerable<IDataPersistence> dataPersistenceObjetcs =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjetcs);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}