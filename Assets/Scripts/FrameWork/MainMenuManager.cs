using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField]
    private ScrollPicker _scrollPicker;
    [SerializeField]
    private RectTransform _slotPrefab;

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

    private DataPersistenceManager _dataPersistenceManager;

    private enum States
    {
        none,
        scrolling,
        highlighted,
        deleting
    }

    private States _mainMenuState;

    private void Awake()
    {
        InitializeMenu();
    }

    private void FixedUpdate()
    {
        switch (_mainMenuState)
        {
            case States.none:
                break;
            case States.scrolling:
                VisualizePlayButton();
                break;
            case States.highlighted:
                break;
        }
    }

    private void InitializeMenu()
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

    private void AddButtonPressed()
    {
        PopAnimation(_addButton.transform);
        AddSaveSlot();
    }

    private void BackButtonPressed()
    {
        PopAnimation(_backButton.transform);

        switch (_mainMenuState)
        {
            case States.highlighted:
                _scrollPicker.UnhighlightSlot();
                HideAnimation(_backButton.transform);
                HideAnimation(_deleteButton_R.transform);
                ShowAnimation(_addButton.transform);
                ReappearAnimation(_playButton.transform, Vector2.zero, 1f);
                _mainMenuState = States.scrolling;
                break;
            case States.deleting:
                ShowAnimation(_playButton.transform, 1.5f);
                ShowAnimation(_deleteButton_R.transform);
                HideAnimation(_deleteButton_C.transform);
                _mainMenuState = States.highlighted;
                break;
        }
    }

    private void PlayButtonPressed()
    {
        PopAnimation(_playButton.transform);

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
                _dataPersistenceManager.LoadGame(_scrollPicker.SelectedSlotIndex);
                break;
        }
    }

    private void DeleteButtonPressed()
    {

        switch (_mainMenuState)
        {
            case States.highlighted:
                HideAnimation(_playButton.transform);
                HideAnimation(_deleteButton_R.transform);
                ShowAnimation(_deleteButton_C.transform);
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
                ShowAnimation(_addButton.transform);
                ReappearAnimation(_playButton.transform, Vector2.zero, 1f);
                _mainMenuState = States.scrolling;
                break;
        }
    }

    public void StartMainMenu()
    {
        // Show Buttons
        ShowAnimation(_addButton.transform);
        ShowAnimation(_playButton.transform);

        // Scroll picker
        float positionX = _scrollPicker.transform.position.x;
        _scrollPicker.transform.position += Vector3.left * 2000;
        _scrollPicker.gameObject.SetActive(true);
        _scrollPicker.transform.DOMoveX(positionX, 0.5f, true);

        _mainMenuState = States.scrolling;
    }

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
        DOTween.Complete(transform);
        transform.DOScale(0, 0.1f).OnComplete(() => transform.gameObject.SetActive(false));
    }

    private void ReappearAnimation(Transform transform, Vector2 position, float endScale)
    {
        DOTween.Complete(transform);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(0f, 0.1f)).Append(transform.DOLocalMove(new Vector3(position.x, position.y, 0), 0)).Append(transform.DOScale(endScale, 0.2f));
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
            Image screenshotImage = _scrollPicker.Slots[index].Find("Screenshot").GetComponent<Image>();
            screenshotImage.sprite = sprite;
        }
    }

    public void AddSaveSlot()
    {
        _dataPersistenceManager.CreateNewSaveSlot();
        _scrollPicker.CreateSlots(_slotPrefab, 1);
        InitializeSaveSlot(_scrollPicker.Slots.Count - 1);
        _scrollPicker.SnapToSlot(_scrollPicker.Slots.Count - 1);
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
}