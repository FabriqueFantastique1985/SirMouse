using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private List<Image> _screenshotImages = new List<Image>();

    [SerializeField]
    private GameObject _defaultSlot;
    [SerializeField]
    private GameObject _brokenSlot;
    [SerializeField]
    private RectTransform _brokenSlotLeft;
    [SerializeField]
    private RectTransform _brokenSlotRight;

    private void Awake()
    {
        SetDefaultState();
    }

    public void SetDefaultState()
    {
        _defaultSlot.SetActive(true);
        _brokenSlot.SetActive(false);
        _brokenSlotLeft.anchoredPosition = new Vector2(0, _brokenSlotLeft.anchoredPosition.y);
        _brokenSlotRight.anchoredPosition = new Vector2(0, _brokenSlotRight.anchoredPosition.y);
    }

    public void SetScreenshotImages(Sprite sprite)
    {
        for (int i = 0; i < _screenshotImages.Count; i++)
        {
            _screenshotImages[i].sprite = sprite;
        }
    }

    public void SetBrokenState()
    {
        _defaultSlot.SetActive(false);
        _brokenSlot.SetActive(true);
        _brokenSlotLeft.DOAnchorPosX(- 20f, 0.3f, true);
        _brokenSlotRight.DOAnchorPosX(20f, 0.3f, true);
    }
}