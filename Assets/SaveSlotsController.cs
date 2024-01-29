using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotsController : MonoBehaviour
{
    [SerializeField]
    private Scrollbar _scrollbar;
    [SerializeField]
    private GameObject _newGameButton;

    [SerializeField]
    private GameObject _slotSelectionCanvas;
    [SerializeField]
    private GameObject _highlightSlotCanvas;

    [SerializeField]
    private Animator _animator;

    private const string MOTION_TIME = "MotionTime";
    private float _targetScrollPosition;

    private float _initValue;
    [SerializeField]
    private float _snapSpeed = 10.0f;
    private float _lerpValue = 0.0f;
    private bool _isSnapping = false;

    [HideInInspector]
    public int AmountSlots;
    [HideInInspector]
    public int SelectedSlot;
    private int _maxAmountSlots = 10;
    private float _scrollScale = 1f / 9;

    [SerializeField]
    private List<GameObject> _slots = new List<GameObject>();

    private void Start()
    {
        SetSelectedSlot();
        UnhighlightSlot();
    }

    private void Update()
    {
        _newGameButton.SetActive(AmountSlots < 10);

        if (Input.GetMouseButtonUp(0))
        {
            _targetScrollPosition = Mathf.Clamp(CalculateNearestPosition(_scrollbar.value), (_maxAmountSlots - AmountSlots) * _scrollScale, 1);
            _initValue = _scrollbar.value;
            _isSnapping = true;
        }

        if (_isSnapping)
        {
            SnapToValue();
        }

        _animator.SetFloat(MOTION_TIME, 1 - _scrollbar.value);
    }

    private float CalculateNearestPosition(float currentValue)
    {
        int nearestIndex = Mathf.RoundToInt(currentValue / _scrollScale);
        return nearestIndex * _scrollScale;
    }

    private void SnapToValue()
    {
        _lerpValue += Time.deltaTime * _snapSpeed;
        _scrollbar.value = Mathf.Lerp(_initValue, _targetScrollPosition, _lerpValue);

        if (_lerpValue > 1)
        {
            SetSelectedSlot();
            _lerpValue = 0;
            _isSnapping = false;
        }
    }

    public void InitializeSlots(List<GameData> gameDataSlots)
    {
        AmountSlots = gameDataSlots.Count;

        for (int i = 0; i < _maxAmountSlots; i++)
        {
            if (i < AmountSlots)
            {
                _slots[i].SetActive(true);
                _slots[i].transform.GetChild(1).GetComponent<Text>().text = gameDataSlots[i].SaveID.ToString();

                // Load and assign the screenshot
                string screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + gameDataSlots[i].SaveID + ".png");
                if (File.Exists(screenshotPath))
                {
                    byte[] imageData = File.ReadAllBytes(screenshotPath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);

                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    _slots[i].GetComponent<Image>().sprite = sprite;
                }
            }
            else
            {
                _slots[i].SetActive(false);
            }
        }
    }


    private void SetSelectedSlot()
    {
        SelectedSlot = (_maxAmountSlots - 1) - Mathf.FloorToInt(_scrollbar.value * 10);
        if (SelectedSlot < 0)
        {
            SelectedSlot = 0;
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].transform.GetChild(0).GetComponent<Image>().raycastTarget = SelectedSlot == i;
        }
    }

    public void HighlightSlot()
    {
        _slotSelectionCanvas.SetActive(false);
        _highlightSlotCanvas.SetActive(true);
        _animator.SetLayerWeight(1, 1);
    }

    public void UnhighlightSlot()
    {
        _slotSelectionCanvas.SetActive(true);
        _highlightSlotCanvas.SetActive(false);
        _animator.SetLayerWeight(1, 0);
    }
}