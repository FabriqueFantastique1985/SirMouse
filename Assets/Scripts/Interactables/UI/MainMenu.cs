using System;
using System.Threading.Tasks;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneField _toLoadScene;
    
    [SerializeField]
    private Button _newGameButton;

    [SerializeField]
    private Button _continueButton;

    private void Awake()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
    }

    private void ContinueGame()
    {
        Loader.Instance.LoadScene(_toLoadScene);
    }

    private async void StartNewGame()
    {
        await Task.Run(DataPersistenceManager.Instance.ClearGame);
        Loader.Instance.LoadScene(_toLoadScene);
        TutorialTracker.Instance.ResetTutorialStates();
    }
}
