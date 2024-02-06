using DG.Tweening;
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
    private GameObject _scrollView;

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

    [SerializeField]
    private Sprite _brokenSlotAlpha;

    private void Awake()
    {
        _newGameButton.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        StartCoroutine(ShowNewGameButton());
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

                // Load and assign the screenshot
                string screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + gameDataSlots[i].SaveID + ".png");
                if (File.Exists(screenshotPath))
                {
                    byte[] imageData = File.ReadAllBytes(screenshotPath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);

                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    _slots[i].GetComponent<Image>().sprite = sprite;
                    _slots[i].transform.Find("Screenshot").GetComponent<Image>().sprite = sprite;
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
            _slots[i].transform.Find("Select_Button").gameObject.SetActive(SelectedSlot == i);
        }
    }

    private IEnumerator LerpAnimatorLayerWeight(int layerIndex, float duration, bool lerpIn)
    {
        float startValue = _animator.GetLayerWeight(layerIndex);
        float targetValue = lerpIn ? 1 : 0;
        float time = 0;

        while (time < duration)
        {
            _animator.SetLayerWeight(layerIndex, Mathf.Lerp(startValue, targetValue, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        _animator.SetLayerWeight(layerIndex, targetValue);
    }

    public void HighlightSlot()
    {
        _scrollView.SetActive(false);
        //_slotSelectionCanvas.SetActive(false);
        //_highlightSlotCanvas.SetActive(true);
        _slots[SelectedSlot].transform.Find("Play_Button").gameObject.SetActive(true);
        _animator.SetLayerWeight(2, 0);
        StartCoroutine(LerpAnimatorLayerWeight(1, 0.2f, true));
    }

    public void UnhighlightSlot()
    {
        _scrollView.SetActive(true);
        //_slotSelectionCanvas.SetActive(true);
        //_highlightSlotCanvas.SetActive(false);
        _slots[SelectedSlot].transform.Find("Play_Button").gameObject.SetActive(false);
        StartCoroutine(LerpAnimatorLayerWeight(1, 0.2f, false));
    }

    private IEnumerator ShowNewGameButton()
    {
        yield return new WaitForSeconds(1);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_newGameButton.transform.DOScale(1.5f, 0.3f)).Append(_newGameButton.transform.DOScale(1, 0.1f));
    }

    public void ButtonPop(Transform button)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(button.DOScale(1.5f, 0.1f)).Append(button.DOScale(1, 0.2f));
    }

    public IEnumerator ButtonHideCoroutine(Transform button)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(button.DOScale(1.2f, 0.1f)).Append(button.DOScale(0, 0.1f));
        yield return new WaitForSeconds(0.2f);
        button.gameObject.SetActive(false);
    }

    public void ButtonHide(Transform button)
    {
        StartCoroutine(ButtonHideCoroutine(button));
    }

    public IEnumerator ButtonShowCoroutine(Transform button)
    {
        button.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(button.DOScale(1.5f, 0.1f)).Append(button.DOScale(1, 0.2f));
        yield return new WaitForSeconds(0.3f);
    }

    public void ButtonShow(Transform button)
    {
        StartCoroutine(ButtonShowCoroutine(button));
    }

    public void BrokenSlotVisual()
    {
        _slots[SelectedSlot].GetComponent<Image>().sprite = _brokenSlotAlpha;
        _slots[SelectedSlot].transform.Find("Screenshot").GetComponent<Image>().color = new Color(1.0f, 0.5f, 0.5f);
        _slots[SelectedSlot].transform.Find("Background").gameObject.SetActive(false);
    }

    public void ReverseBrokenSlotVisual()
    {
        _slots[SelectedSlot].GetComponent<Image>().sprite = null;
        _slots[SelectedSlot].transform.Find("Screenshot").GetComponent<Image>().color = Color.white;
        _slots[SelectedSlot].transform.Find("Background").gameObject.SetActive(true);
    }
}