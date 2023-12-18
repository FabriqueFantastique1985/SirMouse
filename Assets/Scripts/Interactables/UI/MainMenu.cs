using System;
using System.Collections;
using System.Threading.Tasks;
using UnityCore.Audio;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;
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
    [SerializeField]
    private GameObject _panelClothing;

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

    [Header("Language Toggle")]
    [SerializeField]
    private GameObject _panelLanguageToggle;
    [SerializeField]
    private ButtonLanguageToggle _buttonLanguageScript;
    [SerializeField]
    private Button _buttonLanguage;
    [SerializeField]
    private LanguageReferences _languageRefs;

    private bool _currentlyInEnglish;


    private string _toLoadSceneName = "";


    [Header("calculated wait time for buttons to start appearing")]
    [SerializeField]
    private float _waitTimeForButtonsToPopIn;


    [Header("New spashscreen system")]
    // Spash screens
    [SerializeField]
    private PlayableDirector _splashScreen_SM_FF;

    [SerializeField]
    private VideoPlayer _VAFLeaderVideoPlayer;
    [SerializeField]
    private GameObject _VAFLeaderCanvas;
    [SerializeField]
    private RawImage _VAFLeaderRawImage;

    [SerializeField]
    private PlayableDirector _VAFPancarte;

    private IEnumerator _splashScreenCoroutine;

    [SerializeField]
    private Animation _redBackgroundAnimation;
    [SerializeField]
    private Animation _foreGroundAnimation;

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
        _newGameDoubleCheck.onClick.AddListener(EnterNewGameDoubleCheck);
        _exitDoubleCheck.onClick.AddListener(ExitNewGameDoubleCheck);
        _buttonLanguage.onClick.AddListener(SwapLanguage);

        //_continueButton.interactable = DataPersistenceManager.Instance.HasGameData;
        _continueButton.gameObject.SetActive(DataPersistenceManager.Instance.HasGameData);

        //// start a coroutine to show the buttons...
        //StartCoroutine(ShowButtons());

        //StartCoroutine(PlaySound(_delayOST, _sourceOST));
        //StartCoroutine(PlaySound(0.8f, _sourceLogoPopup));
        _VAFLeaderCanvas.SetActive(false);
        _VAFLeaderRawImage.color = Color.black;
        PlaySpashScreens();
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


    private void SwapLanguage()
    {
        _currentlyInEnglish = !_currentlyInEnglish;

        for (int i = 0; i < _languageRefs.ObjectsInEnglish.Count; i++)
        {
            _languageRefs.ObjectsInEnglish[i].SetActive(_currentlyInEnglish);
        }
        for (int i = 0; i < _languageRefs.ObjectsInDutch.Count; i++)
        {
            _languageRefs.ObjectsInDutch[i].SetActive(!_currentlyInEnglish);
        }

        _buttonLanguageScript.SwapLanguageVisual(_currentlyInEnglish);

        _buttonLanguage.GetComponent<Animation>().Play("L3");
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

        // activate language panel
        _panelLanguageToggle.SetActive(true);

        // start showing the clothing pieces 1 by 1
        _panelClothing.SetActive(true);
    }
    private IEnumerator PulseButton(Animation animationComponentButton)
    {
        yield return new WaitForSeconds(2f);

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

    private void PlaySpashScreens()
    {
        _splashScreenCoroutine = SplashScreenCoroutine();
        StartCoroutine(_splashScreenCoroutine);
    }

    IEnumerator SplashScreenCoroutine()
    {
        _splashScreen_SM_FF.Play();

        yield return new WaitForSeconds((float)_splashScreen_SM_FF.duration);
        _VAFLeaderCanvas.SetActive(true);
        _VAFLeaderVideoPlayer.Play();

        yield return new WaitForSeconds(0.1f);
        _VAFLeaderRawImage.color = Color.white;

        yield return new WaitForSeconds((float)_VAFLeaderVideoPlayer.length);
        _VAFLeaderCanvas.SetActive(false);
        _VAFPancarte.Play();
        
        yield return new WaitForSeconds((float)_VAFPancarte.duration);
        ShowMenu();
    }

    public void SkipSequence()
    {
        _VAFLeaderCanvas.SetActive(false);
        _VAFLeaderVideoPlayer.Stop();
        StopCoroutine(_splashScreenCoroutine);
        ShowMenu();
    }

    private IEnumerator ShowMenuBackground()
    {
        _redBackgroundAnimation.Play();
        yield return new WaitForSeconds(1.5f);
        _foreGroundAnimation.Play();

    }

    private void ShowMenu()
    {
        StartCoroutine(ShowMenuBackground());
        StartCoroutine(ShowButtons());

        StartCoroutine(PlaySound(_delayOST, _sourceOST));
        StartCoroutine(PlaySound(0.8f, _sourceLogoPopup));
    }
}