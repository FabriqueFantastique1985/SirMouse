using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrollPicker : MonoBehaviour
{
    [SerializeField]
    private RectTransform _slotPrefab;
    [SerializeField]
    private RectTransform _scrollPivot;

    private List<RectTransform> _slots = new List<RectTransform>();
    private int _numberOfSlots = 10;
    [SerializeField]
    private float _slotSpacing = 50f;

    private Vector2 _startDragPosition;
    private Vector2 _pivotStartPosition;
    private bool _isDragging = false;
    private float _momentum;
    private Vector2 _lastMousePosition;
    [SerializeField]
    private float _momentumPersistence = 0.95f;

    [SerializeField]
    private float _snapPositionY = 0f;

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        HandleInput();
        ApplyMomentum();
        SnapSlots();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _numberOfSlots; i++)
        {
            // Instantiate the slot
            RectTransform newSlot = Instantiate(_slotPrefab, transform);

            // Position the slot
            float yPos = -i * _slotSpacing; // Calculate the y position
            newSlot.anchoredPosition = new Vector2(0, yPos);

            // Add the slot to the list
            _slots.Add(newSlot);
        }
    }
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse button pressed
            _isDragging = true;
            _startDragPosition = Input.mousePosition;
            _pivotStartPosition = _scrollPivot.anchoredPosition;
            _lastMousePosition = Input.mousePosition;
            _momentum = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Mouse button released
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 dragDistance = currentMousePosition - _startDragPosition;

            // Apply the drag distance to the scroll pivot
            _scrollPivot.anchoredPosition = _pivotStartPosition + new Vector2(0, dragDistance.y);

            // Calculate momentum based on the speed of the mouse movement
            _momentum = (currentMousePosition - _lastMousePosition).y / Time.deltaTime;
            _lastMousePosition = currentMousePosition;
        }
    }

    private void ApplyMomentum()
    {
        if (!_isDragging && _momentum != 0)
        {
            // Apply the momentum to the scroll pivot
            Vector2 newPosition = _scrollPivot.anchoredPosition + new Vector2(0, _momentum * Time.deltaTime);
            _scrollPivot.anchoredPosition = newPosition;

            // Reduce the momentum over time
            _momentum *= _momentumPersistence;

            // Stop the momentum when it's sufficiently small
            if (Mathf.Abs(_momentum) < 0.1f)
            {
                _momentum = 0;
            }
        }
    }

    private void SnapSlots()
    {
        if (!_isDragging && Mathf.Abs(_momentum) < 0.1f && !DOTween.IsTweening(_scrollPivot))
        {
            RectTransform closestSlot = null;
            float closestDistance = float.MaxValue;

            // Find the closest slot to the snap position
            foreach (RectTransform slot in _slots)
            {
                float distance = Mathf.Abs(_snapPositionY - slot.anchoredPosition.y);
                if (distance < closestDistance)
                {
                    closestSlot = slot;
                    closestDistance = distance;
                }
            }

            if (closestSlot != null)
            {
                // Calculate the distance to move the scroll pivot
                float distanceToMove = _snapPositionY - closestSlot.anchoredPosition.y;
                _scrollPivot.DOAnchorPos(_scrollPivot.anchoredPosition + new Vector2(0, distanceToMove), 0.5f);
            }
        }
    }
}