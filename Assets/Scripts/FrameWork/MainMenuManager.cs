using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Start Up")]
    [SerializeField]
    private StartUpSequence _startUpSequence;
    [SerializeField]
    private Animation _titleLogo;
    [SerializeField]
    private bool _playSequence = true;
    
    [Header("Slots")]
    [SerializeField]
    private ScrollPicker _scrollPicker;
    [SerializeField]
    private RectTransform _slotPrefab;
    [SerializeField]
    private int _maxAmountSlots = 10;

    [Header("Buttons")]
    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _deleteButton_R;
    [SerializeField]
    private Button _deleteButton_C;

    [Header("Audio")]
    [SerializeField]
    private AudioSource _OST;
    [SerializeField]
    private AudioSource _tada;

    [SerializeField]
    private AudioElement _popSound;
    [SerializeField]
    private AudioElement _wooshSound;
    [SerializeField]
    private AudioElement _rulerTwang;
    [SerializeField]
    private AudioElement _vaseBroken;

    [Header("Other")]
    [SerializeField]
    private Volume _postProcessingVolume;

    private DataPersistenceManager _dataPersistenceManager;


    private enum States
    {
        none,
        titleScreen,
        scrolling,
        highlighted,
        deleting
    }

    private States _mainMenuState;

    private void Awake()
    {
        _titleLogo.gameObject.SetActive(false);
        InitializeMainMenu();
    }

    private void Start()
    {
        if (_playSequence)
        {
            StartUpSequence startUpSequence = Instantiate(_startUpSequence);
            if (_startUpSequence != null)
            {
                startUpSequence.OnSequenceComplete += StartUpSequenceComplete;
            }
        }
        else
        {
            StartMainMenu();
        }
    }

    private void StartUpSequenceComplete()
    {
        ShowTitleScreen();
    }

    private void FixedUpdate()
    {
        switch (_mainMenuState)
        {
            case States.none:
                break;
            case States.titleScreen:
                if (Input.GetMouseButtonDown(0))
                {
                    _titleLogo.GetComponent<RectTransform>().DOAnchorPosX(1500, 0.3f).OnComplete(() => Destroy(_titleLogo.gameObject));
                    StartMainMenu();
                }
                break;
            case States.scrolling:
                VisualizePlayButton();
                break;
            case States.highlighted:
                break;
        }
    }

    #region MainMenu
    private void InitializeMainMenu()
    {
        // Initialize buttons
        _addButton.onClick.AddListener(AddButtonPressed);
        _backButton.onClick.AddListener(BackButtonPressed);
        _playButton.onClick.AddListener(PlayButtonPressed);
        _deleteButton_R.onClick.AddListener(DeleteButtonPressed);
        _deleteButton_C.onClick.AddListener(DeleteButtonPressed);

        // Hide all UI elements
        _scrollPicker.gameObject.SetActive(false);
        _addButton.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
        _deleteButton_R.gameObject.SetActive(false);
        _deleteButton_C.gameObject.SetActive(false);

        // Load all data
        _dataPersistenceManager = DataPersistenceManager.Instance;
        _dataPersistenceManager.LoadAllSaveSlots();

        // CreateSlots
        _scrollPicker.CreateSlots(_slotPrefab, _dataPersistenceManager.GameDataSlots.Count);
        InitializeAllSaveSlots();
    }

    private void ShowTitleScreen()
    {
        _mainMenuState = States.titleScreen;

        _tada.Play();
        _titleLogo.gameObject.SetActive(true);
        _titleLogo.Play();
    }

    private void StartMainMenu()
    {
        _mainMenuState = States.scrolling;
        _OST.Play();

        // Show Buttons
        if (_scrollPicker.Slots.Count < _maxAmountSlots)
        {
            ShowAnimation(_addButton.transform);
        }
        ShowAnimation(_playButton.transform);

        // Scroll picker
        float positionX = _scrollPicker.transform.position.x;
        _scrollPicker.transform.position += Vector3.left * 2000;
        _scrollPicker.gameObject.SetActive(true);
        _scrollPicker.transform.DOMoveX(positionX, 0.5f, true);

    }

    private void InitializeAllSaveSlots()
    {
        for (int i = 0; i < _scrollPicker.Slots.Count; i++)
        {
            InitializeSaveSlot(i);
        }
    }

    private void InitializeSaveSlot(int index)
    {
        // Load and assign the screenshot
        string screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + _dataPersistenceManager.GameDataSlots[index].SaveID + ".png");

        if (File.Exists(screenshotPath))
        {
            byte[] imageData = File.ReadAllBytes(screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _scrollPicker.Slots[index].GetComponent<Slot>().SetScreenshotImages(sprite);
        }
    }

    public IEnumerator AddSaveSlot()
    {
        _mainMenuState = States.none;
        HideAnimation(_addButton.transform);
        HideAnimation(_playButton.transform);
        _dataPersistenceManager.CreateNewSaveSlot();
        _scrollPicker.CreateSlots(_slotPrefab, 1);
        InitializeSaveSlot(_scrollPicker.Slots.Count - 1);
        _scrollPicker.SnapToSlot(_scrollPicker.Slots.Count - 1);

        yield return new WaitForSeconds(1);
        if (_scrollPicker.Slots.Count < _maxAmountSlots)
        {
            ShowAnimation(_addButton.transform);
        }
        ShowAnimation(_playButton.transform);
        _mainMenuState = States.scrolling;
    }

    private void VisualizePlayButton()
    {
        if (!_scrollPicker.IsScrolling && !_playButton.gameObject.activeSelf)
        {
            ShowAnimation(_playButton.transform);
        }

        if (_scrollPicker.IsScrolling && _playButton.gameObject.activeSelf)
        {
            HideAnimation(_playButton.transform);
        }
    }
    #endregion MainMenu

    #region Buttons
    private void AddButtonPressed()
    {
        AudioController.Instance.PlayAudio(_popSound);
        StartCoroutine(AddSaveSlot());
    }

    private void BackButtonPressed()
    {
        AudioController.Instance.PlayAudio(_rulerTwang);

        switch (_mainMenuState)
        {
            case States.highlighted:
                _scrollPicker.UnhighlightSlot();
                HideAnimation(_backButton.transform);
                HideAnimation(_deleteButton_R.transform);
                if (_scrollPicker.Slots.Count < _maxAmountSlots)
                {
                    ShowAnimation(_addButton.transform);
                }
                ReappearAnimation(_playButton.transform, Vector2.zero, 1f);
                _mainMenuState = States.scrolling;
                break;
            case States.deleting:
                PopAnimation(_backButton.transform);
                ShowAnimation(_playButton.transform, 1.5f);
                ShowAnimation(_deleteButton_R.transform);
                HideAnimation(_deleteButton_C.transform);
                _scrollPicker.Slots[_scrollPicker.SelectedSlotIndex].GetComponent<Slot>().SetDefaultState();
                _postProcessingVolume.enabled = false;
                _mainMenuState = States.highlighted;
                break;
        }
    }

    private void PlayButtonPressed()
    {
        AudioController.Instance.PlayAudio(_popSound);
        switch (_mainMenuState)
        {
            case States.scrolling:
                _scrollPicker.HighlightSlot();
                HideAnimation(_addButton.transform);
                ShowAnimation(_backButton.transform);
                ShowAnimation(_deleteButton_R.transform);
                ReappearAnimation(_playButton.transform, new Vector2(90f, 0f), 1.5f);
                _mainMenuState = States.highlighted;
                break;
            case States.highlighted:
                PopAnimation(_playButton.transform);
                _dataPersistenceManager.LoadGame(_scrollPicker.SelectedSlotIndex);
                break;
        }
    }

    private void DeleteButtonPressed()
    {
        AudioController.Instance.PlayAudio(_vaseBroken);
        switch (_mainMenuState)
        {
            case States.highlighted:
                HideAnimation(_playButton.transform);
                HideAnimation(_deleteButton_R.transform);
                ShowAnimation(_deleteButton_C.transform);
                _scrollPicker.Slots[_scrollPicker.SelectedSlotIndex].GetComponent<Slot>().SetBrokenState();
                _postProcessingVolume.enabled = true;
                _mainMenuState = States.deleting;
                break;
            case States.deleting:
                // Delete slot
                _dataPersistenceManager.DeleteSlot(_scrollPicker.SelectedSlotIndex);
                _scrollPicker.DeleteSlot();

                // Reset menu
                _dataPersistenceManager.LoadAllSaveSlots();
                HideAnimation(_backButton.transform);
                HideAnimation(_deleteButton_C.transform);
                if (_scrollPicker.Slots.Count < _maxAmountSlots)
                {
                    ShowAnimation(_addButton.transform);
                }
                ReappearAnimation(_playButton.transform, Vector2.zero, 1f);
                _postProcessingVolume.enabled = false;
                _mainMenuState = States.scrolling;
                break;
        }
    }
    #endregion Buttons

    #region UIAnimations
    private void PopAnimation(Transform transform)
    {
        DOTween.Complete(transform);
        transform.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(1.5f, 0.1f)).Append(transform.DOScale(1, 0.2f));
    }

    private void ShowAnimation(Transform transform)
    {
        DOTween.Complete(transform);
        transform.gameObject.SetActive(true);
        transform.DOScale(1, 0.1f);
    }

    private void ShowAnimation(Transform transform, float scale)
    {
        DOTween.Complete(transform);
        transform.gameObject.SetActive(true);
        transform.DOScale(scale, 0.1f);
    }

    private void HideAnimation(Transform transform)
    {
        AudioController.Instance.PlayAudio(_wooshSound);
        DOTween.Complete(transform);
        transform.DOScale(0, 0.1f).OnComplete(() => transform.gameObject.SetActive(false));
    }

    private void ReappearAnimation(Transform transform, Vector2 position, float endScale)
    {
        DOTween.Complete(transform);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(0f, 0.1f)).Append(transform.DOLocalMove(new Vector3(position.x, position.y, 0), 0)).Append(transform.DOScale(endScale, 0.2f));
    }
    #endregion UIAnimations
}