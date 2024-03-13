using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField]
    private CanvasScaler _canvasScaler;
    private float _canvasRatio;
    [SerializeField]
    private int _minWidth = 1050;
    [SerializeField]
    private int _maxWidth = 1340;

    [Header("Rect Transform")]
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private float _minScale = 1;
    [SerializeField]
    private float _maxScale = 1.6f;

    [Header("Settings")]
    [SerializeField]
    private float _minRatio = 1.3f;
    [SerializeField]
    private float _maxRatio = 2.3f;
    [SerializeField]
    private bool _update = false;

    private void Awake()
    {
        CalculateReferenceResolution();
    }

    private void Update()
    {
        if(_update)
        {
            CalculateReferenceResolution();
        }
    }

    private void CalculateReferenceResolution()
    {
        _canvasRatio = (float)Screen.width / Screen.height;
        float lerpValue = Mathf.Clamp(_canvasRatio, _minRatio, _maxRatio) - _minRatio;

        if (_canvasScaler != null)
        {
            _canvasScaler.referenceResolution = new Vector2(Mathf.Lerp(_minWidth, _maxWidth, lerpValue), _canvasScaler.referenceResolution.y);
        }

        if (_rectTransform != null)
        {
            float scaleValue = Mathf.Lerp(_minScale, _maxScale, lerpValue);
            _rectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }
}