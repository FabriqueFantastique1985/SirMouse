using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-200)]
public class DataPersistenceManager : MonoBehaviourSingleton<DataPersistenceManager>
{
    //// OLD
    //[Header("File Storage Config")]
    //[SerializeField] private string fileName;

    [Header("Debugging")]
    [SerializeField]
    private bool _initializeDataIfNull = false;
    
    #region Fields

    private GameData _gameData;
    public List<GameData> GameDataSlots = new List<GameData>();
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    #endregion

    #region Properties
    public bool HasGameData => _gameData != null;

    #endregion

    //// OLD
    //protected override void Awake()
    //{
    //    base.Awake();
    //    _dataHandler = new FileDataHandler(Path.Combine(Application.persistentDataPath), fileName);
    //}

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

        ////OLD
        //LoadGame();

        if (arg0.name != "MainMenu")
        {
            SetDataToObjects();
            StartCoroutine(SaveScreenshot());
        }

        if (_gameData != null)
        {
            _gameData.lastActiveScene = arg0.name;
        }
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        SaveGame();
    }

    public void NewGame()
    {
        //// OLD
        //DeleteGameData();
        //_gameData = new GameData();



        // NEW

        GameData newGameData = new GameData();

        // Assign the SaveID to the GameData based on the amount of data slots
        newGameData.SaveID = GenerateUniqueSaveID();

        // Add the new GameData to the list of data slots
        GameDataSlots.Add(newGameData);

        // Assign the new GameData to _gameData field because this is the GameData that's currently active
        _gameData = newGameData;

        // Initilize the data handler
        InitializeDataHandler(_gameData.SaveID);

        // Save new GameData to file
        SaveGame();

        Loader.Instance.LoadScene("IntroCutscene");
    }

    public void CreateNewSaveSlot()
    {
        GameData newGameData = new GameData();

        // Assign the SaveID to the GameData based on the amount of data slots
        newGameData.SaveID = GenerateUniqueSaveID();

        // Add the new GameData to the list of data slots
        GameDataSlots.Add(newGameData);

        // Assign the new GameData to _gameData field because this is the GameData that's currently active
        _gameData = newGameData;

        // Initilize the data handler
        InitializeDataHandler(_gameData.SaveID);

        // Save new GameData to file
        SaveGame();

        LoadAllSaveSlots();
    }

    public void LoadGame(int slotIndex)
    {
        _gameData = GameDataSlots[slotIndex];
        InitializeDataHandler( _gameData.SaveID);
        _gameData = _dataHandler.Load();
        
        if (_gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded");
            return;
        }

        if(_gameData.lastActiveScene == null)
        {
            Loader.Instance.LoadScene("IntroCutscene");
        }
        else
        {
            Loader.Instance.LoadScene(_gameData.lastActiveScene);
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

    private void InitializeDataHandler(string saveID)
    {
        _dataHandler = new FileDataHandler(Path.Combine(Application.persistentDataPath), "SaveSlot_" + saveID + ".json");

        Debug.Log(Path.Combine(Application.persistentDataPath) + "SaveSlot_" + saveID + ".json");
    }

    private string GenerateUniqueSaveID()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmss");
    }



    public void LoadAllSaveSlots()
    {
        // Find all save files in the directory
        string[] saveFiles = System.IO.Directory.GetFiles(Application.persistentDataPath, "SaveSlot_*.json");

        // Clear the existing game data slots list
        GameDataSlots.Clear();

        foreach (string file in saveFiles)
        {
            // Extract the SaveID from the file name
            string fileName = Path.GetFileName(file);
            string saveID = ExtractSaveIDFromFileName(fileName);

            // Initialize the data handler from this save slot
            InitializeDataHandler(saveID);

            // Load the game data
            GameData loadedGameData = _dataHandler.Load();

            if (loadedGameData != null)
            {
                // Add the loaded game data to the list
                GameDataSlots.Add(loadedGameData);

                // Print out the details (or handle them as needed)
                Debug.Log("Loaded save slot: " + saveID);
            }
        }
    }

    private string ExtractSaveIDFromFileName(string fileName)
    {
        return fileName.Replace("SaveSlot_", "").Replace(".json", "");
    }

    private void SetDataToObjects()
    {
        foreach (var dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void DeleteSlot(int slotIndex)
    {
        InitializeDataHandler(GameDataSlots[slotIndex].SaveID);
        _dataHandler.DeleteSaveFile();
        _dataPersistenceObjects.Clear();
    }

    public IEnumerator SaveScreenshot()
    {
        yield return new WaitForSeconds(5);
        string fileName = "screenshot" + _gameData.SaveID + ".png";
        string path = System.IO.Path.Combine(Application.persistentDataPath, fileName);

        // Capture the screenshot and save it to the specified path
        //ScreenCapture.CaptureScreenshot(path);
        ScreenCapture.CaptureScreenshot(fileName);

        // Optional: Log the path for debugging purposes
        Debug.Log("Screenshot saved to: " + path);
    }
}