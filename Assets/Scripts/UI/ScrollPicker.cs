using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.U2D.Path;
using UnityEngine.UI;

public class ScrollPicker : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private RectTransform _slotPrefab;
    [SerializeField]
    private RectTransform _scrollPivot;

    [Header("Slots")]
    public List<RectTransform> Slots = new List<RectTransform>();
    [SerializeField]
    private int _numberOfSlots = 10;
    [SerializeField]
    private float _slotSpacing = 50f;
    public int SelectedSlotIndex = 0;

    // Scrolling
    private Vector2 _startDragPosition;
    private Vector2 _pivotStartPosition;
    private bool _mouseButtonDown = false;
    private bool _isScrolling = false;
    public bool IsScrolling
    {
        get { return _isScrolling; }
        private set { _isScrolling = value; }
    }
    private float _scrollingThreshold = 3f;
    private float _scrollingDistance = 0;

    [Header("Settings")]
    [SerializeField]
    private bool _createOnAwake = false;

    // Momentum
    private float _momentum;
    [SerializeField]
    private float _minMomentumSpeed = 500f;
    [SerializeField]
    private float _maxMomentumSpeed = 700f;
    private Vector2 _lastMousePosition;
    [SerializeField]
    private float _momentumPersistence = 0.95f;

    // Snapping
    [SerializeField]
    private float _snapSpeed = 5f;
    private float _snapPositionY = 0f;
    private float _snapLerpValue = 0f;
    private float _initLerpPositionY = 0f;
    private bool _isSnapping = false;
    private bool _isSnappingNewSlot = false;

    // Highlighting
    private float[] _scaleLerpValues;
    [SerializeField]
    private float _selectScale = 1.5f;
    public bool SlotHighlighted = false;
    [SerializeField]
    private float _highlightScale = 2f;
    private float _initPivotPositionX;

    private void Awake()
    {
        if(_createOnAwake)
        {
            CreateSlots(_slotPrefab, _numberOfSlots);
        }
    }

    void Update()
    {
        if(_numberOfSlots > 1 && !SlotHighlighted)
        {
            HandleInput();
            ApplyMomentum();
            SlotSnapping();
            ScaleSlots();
        }
    }

    public void CreateSlots(RectTransform slotPrefab, int numberOfSlots)
    {
        _numberOfSlots += numberOfSlots;

        _scaleLerpValues = new float[_numberOfSlots];


        for (int i = 0; i < numberOfSlots; i++)
        {
            // Instantiate the slot
            RectTransform newSlot = Instantiate(slotPrefab, _scrollPivot);

            // Position the slot
            float yPos = -Slots.Count * _slotSpacing;
            newSlot.anchoredPosition = new Vector2(0, yPos);

            // Add the slot to the list
            Slots.Add(newSlot);
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse button pressed
            _mouseButtonDown = true;
            _startDragPosition = Input.mousePosition;
            _pivotStartPosition = _scrollPivot.anchoredPosition;
            _lastMousePosition = Input.mousePosition;
            _momentum = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Mouse button released
            _mouseButtonDown = false;
        }

        if (_mouseButtonDown)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 dragDistance = currentMousePosition - _startDragPosition;

            // Apply the drag distance to the scroll pivot
            _scrollPivot.anchoredPosition = _pivotStartPosition + new Vector2(0, dragDistance.y);

            // Calculate momentum based on the speed of the mouse movement
            _momentum = (currentMousePosition - _lastMousePosition).y / Time.deltaTime;
            _lastMousePosition = currentMousePosition;

        }

        // Check if scrolling based on threshold
        _scrollingDistance = Mathf.Abs(_scrollPivot.anchoredPosition.y - _initLerpPositionY);
        _isScrolling = _scrollingDistance > _scrollingThreshold;
    }

    private void ApplyMomentum()
    {
        if (!_mouseButtonDown)
        {
            if(_momentum != 0)
            {
                // Apply the momentum to the scroll pivot
                Vector2 newPosition = _scrollPivot.anchoredPosition + new Vector2(0, _momentum * Time.deltaTime);
                _scrollPivot.anchoredPosition = newPosition;

                // Reduce the momentum over time
                _momentum *= _momentumPersistence;

                // Stop the momentum when it's sufficiently small
                if (Mathf.Abs(_momentum) < _minMomentumSpeed)
                {
                    _momentum = 0;
                }

                if(Mathf.Abs(_momentum) > _maxMomentumSpeed)
                {
                    _momentum = _momentum / Mathf.Abs(_momentum) * _maxMomentumSpeed;
                }
            }
            else
            {
                _isSnapping = true;
                _initLerpPositionY = _scrollPivot.anchoredPosition.y;
            }

        }
    }

    private void SlotSnapping()
    {
        if(!_isSnappingNewSlot)
        {
            if(_isSnapping)
            {
                // Snap closest slot in position
                _snapLerpValue += Time.deltaTime * _snapSpeed;
                _scrollPivot.anchoredPosition = new Vector2(_scrollPivot.anchoredPosition.x, Mathf.Lerp(_initLerpPositionY, _snapPositionY, _snapLerpValue));

                if (_snapLerpValue > 1)
                {
                    _scrollPivot.anchoredPosition = new Vector2(_scrollPivot.anchoredPosition.x, _snapPositionY);
                    _snapLerpValue = 0;
                    _isSnapping = false;
                }
            }
            else
            {
                // Find the closest slot to the snap position
                float closestDistance = float.MaxValue;
                float maxPositionY = Slots[0].anchoredPosition.y;
                int closestSlotIndex = 0;

                foreach (RectTransform slot in Slots)
                {
                    float distance = Mathf.Abs(Mathf.Abs(_scrollPivot.anchoredPosition.y) - Mathf.Abs(slot.anchoredPosition.y));

                    if (distance < closestDistance)
                    {
                        SelectedSlotIndex = closestSlotIndex;
                        _snapPositionY = slot.anchoredPosition.y * -1;
                        closestDistance = distance;
                    }

                    closestSlotIndex++;
                }

                if (_scrollPivot.anchoredPosition.y < maxPositionY)
                {
                    SelectedSlotIndex = 0;
                    _snapPositionY = maxPositionY;
                }
            }

        }
    }

    public void SnapToSlot(int slotIndex)
    {
        _isSnappingNewSlot = true;
        _snapPositionY = -Slots[slotIndex].anchoredPosition.y;
        _scrollPivot.DOAnchorPosY(_snapPositionY, 0.2f, true).OnComplete(() => _isSnappingNewSlot = false);
        SelectedSlotIndex = slotIndex;
    }

    private void ScaleSlots()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            float distance = Mathf.Abs(Mathf.Abs(_scrollPivot.anchoredPosition.y) - Mathf.Abs(Slots[i].anchoredPosition.y));
            _scaleLerpValues[i] = 1 - Mathf.Clamp(distance / _slotSpacing, 0, 1);
            Slots[i].localScale = Vector3.one * Mathf.Lerp(1f, _selectScale, _scaleLerpValues[i]);
        }
    }

    public void HighlightSlot()
    {
        SlotHighlighted = true;
        _initPivotPositionX = _scrollPivot.anchoredPosition.x;
        _scrollPivot.DOAnchorPosX(0, 0.3f, true);
        Slots[SelectedSlotIndex].DOScale(_highlightScale, 0.3f);
    }

    public void UnhighlightSlot()
    {
        SlotHighlighted = false;
        _scrollPivot.DOAnchorPosX(_initPivotPositionX, 0.3f, true);
        Slots[SelectedSlotIndex].DOScale(_selectScale, 0.3f);
    }

    private void ReorganizeSlots()
    {
        float yPos = 0;

        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].anchoredPosition = new Vector2(0, yPos);
            yPos -= _slotSpacing;
        }
    }

    public void DeleteSlot()
    {
        Destroy(Slots[SelectedSlotIndex].gameObject);
        Slots.RemoveAt(SelectedSlotIndex);
        _numberOfSlots = Slots.Count;
        ReorganizeSlots();
        SelectedSlotIndex = (int)Mathf.Clamp(SelectedSlotIndex - 1, 0, _numberOfSlots -1);
        UnhighlightSlot();
        SnapToSlot(SelectedSlotIndex);
    }
}