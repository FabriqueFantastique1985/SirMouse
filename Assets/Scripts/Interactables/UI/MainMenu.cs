using System;
using System.Collections;
using System.Threading.Tasks;
using UnityCore.Audio;
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

    [Header("Button references")]
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _newGameDoubleCheck;
    [SerializeField]
    private Button _exitDoubleCheck;
    [SerializeField]
    private Button _newGameButton;

    [Header("other references")]
    [SerializeField]
    private GameObject _panelDoubleCheck;

    [Header("animation strings")]
    [SerializeField]
    private string _buttonIdleAnim;
    [SerializeField]
    private string _buttonClickedAnim;

    [Header("Audio")]
    [SerializeField]
    private float _delayOST;
    [SerializeField]
    private AudioSource _sourceOST;
    [SerializeField]
    private AudioSource _sourceFX;
    [SerializeField]
    private AudioSource _sourceLogoPopup;

    private string _toLoadSceneName = "";


    [Header("calculated wait time for buttons to start appearing")]
    [SerializeField]
    private float _waitTimeForButtonsToPopIn;

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
        _newGameDoubleCheck.onClick.AddListener(EnterNewGameDoubleCheck);
        _exitDoubleCheck.onClick.AddListener(ExitNewGameDoubleCheck);

        //_continueButton.interactable = DataPersistenceManager.Instance.HasGameData;
        _continueButton.gameObject.SetActive(DataPersistenceManager.Instance.HasGameData);

        // start a coroutine to show the buttons...
        StartCoroutine(ShowButtons());

        StartCoroutine(PlaySound(_delayOST, _sourceOST));
        StartCoroutine(PlaySound(0.8f, _sourceLogoPopup));
    }

    private void ContinueGame()
    {
        PlaySoundEffect();

        if (_toLoadSceneName == String.Empty) _toLoadSceneName = _toLoadScene;
        Loader.Instance.LoadScene(_toLoadSceneName);
    }

    private void EnterNewGameDoubleCheck()
    {
        // open up extra panel with 2 buttons if save data is present
        if (DataPersistenceManager.Instance.HasGameData)
        {
            PlaySoundEffect();

            _panelDoubleCheck.SetActive(true);
            StartCoroutine(PulseButton(_exitDoubleCheck.GetComponent<Animation>()));
            StartCoroutine(PulseButton(_newGameButton.GetComponent<Animation>()));
        }
        else
        {
            StartNewGame();
        }
    }
    private void ExitNewGameDoubleCheck()
    {
        PlaySoundEffect();

        // close the panel
        _panelDoubleCheck.SetActive(false);
    }

    private async void StartNewGame()
    {
        PlaySoundEffect();

        await Task.Run(DataPersistenceManager.Instance.ClearGame);
        DataPersistenceManager.Instance.NewGame();
        Loader.Instance.LoadScene(_toLoadScene);
    }


    private void PlaySoundEffect()
    {
        _sourceFX.Play();
    }

    private IEnumerator ShowButtons()
    {
        // wait time should be equal to all animation durations leading up to showing the buttons
        yield return new WaitForSeconds(_waitTimeForButtonsToPopIn);

        // first show continue, then new game
        if (DataPersistenceManager.Instance.HasGameData == true)
        {
            _continueButton.gameObject.SetActive(true);
            

            yield return new WaitForSeconds(1f);
        }
        _newGameDoubleCheck.gameObject.SetActive(true);

        StartCoroutine(PulseButton(_continueButton.GetComponent<Animation>()));
        StartCoroutine(PulseButton(_newGameDoubleCheck.GetComponent<Animation>()));
    }
    private IEnumerator PulseButton(Animation animationComponentButton)
    {
        yield return new WaitForSeconds(0.25f);

        animationComponentButton.Play(_buttonIdleAnim);
    }
    private IEnumerator PlaySound(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);

        source.Play();
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
