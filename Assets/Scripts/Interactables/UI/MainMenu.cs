using System;
using System.Threading.Tasks;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private SceneField _toLoadScene;

    [SerializeField]
    private SceneField _mainMenuScene;
    
    [SerializeField]
    private Button _newGameButton;

    [SerializeField]
    private Button _continueButton;

    private string _toLoadSceneName = "";

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
        
        _continueButton.interactable = DataPersistenceManager.Instance.HasGameData;
    }

    private void ContinueGame()
    {
        if (_toLoadSceneName == String.Empty) _toLoadSceneName = _toLoadScene;
        Loader.Instance.LoadScene(_toLoadSceneName);
    }

    private async void StartNewGame()
    {
        await Task.Run(DataPersistenceManager.Instance.ClearGame);
        DataPersistenceManager.Instance.NewGame();
        Loader.Instance.LoadScene(_toLoadScene);
    }

    public void LoadData(GameData data)
    {
        if (data.lastActiveScene == _mainMenuScene) return;
        _toLoadSceneName = data.lastActiveScene;
    }

    public void SaveData(ref GameData data)
    {
    }
}
