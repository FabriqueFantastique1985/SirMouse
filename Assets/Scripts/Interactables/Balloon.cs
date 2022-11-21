using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Balloon : MonoBehaviour, IClickable
{
    #region Events

    public delegate void BalloonDelegate(Balloon sender, Player player);

    /// <summary>
    /// Gets invoked when clicked on.
    /// </summary>
    public event BalloonDelegate OnBalloonClicked;
    
    #endregion

    #region EditorFields

    public Collider BalloonTrigger;

    [SerializeField]
    protected Animator _balloonAnimator;

    [SerializeField]
    private bool _disableOnClick = false;

    [Header("BalloonAnimations")]
    [SerializeField]
    private string _animFloat = "Balloon_Floaty";
    
    [SerializeField]
    private string _animPop = "Balloon_Pop";

    [Header("BalloonSprite")]
    [SerializeField]
    private AudioElement _soundEffectClick;

    [Header("BalloonSprite")]
    [SerializeField]
    private Image _objectImageRenderer;

    
    #endregion

    #region Fields

    private Rect _imageOriginalRect;

    private GameObject _currentSpriteObject;

    #endregion

    #region Methods

    private void Awake()
    {
      if (_objectImageRenderer != null) _imageOriginalRect = _objectImageRenderer.rectTransform.rect;
    }

    public void Click(Player player)
    {
        if (_balloonAnimator != null) _balloonAnimator.Play(_animPop);
        if (_disableOnClick) StartCoroutine(DisableBalloon());
        AudioController.Instance.PlayAudio(_soundEffectClick);
        OnBalloonClicked?.Invoke(this, player);
    }
    
    private IEnumerator DisableBalloon()
    {
        // disable the collider -> wait a bit -> disable the gameobject + enable the collider
        BalloonTrigger.enabled = false;

        yield return new WaitForSeconds(0.35f); // should be the length of the animation "Pop"

        gameObject.SetActive(false);
        BalloonTrigger.enabled = true;
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        if (_balloonAnimator != null) _balloonAnimator.Play(_animFloat);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSprite(Sprite newSprite)  // methodology for when the balloon is a UI/canvas object
    {
        var oldSprite = _objectImageRenderer.sprite;

        float aspectRatio = newSprite.rect.width / newSprite.rect.height;
        float newImageRendererWidth = _imageOriginalRect.width;
        float newImageRendererHeight = _imageOriginalRect.height;
        // first check if the new sprite is square shaped. If so, skip readjusting the image.

        if (Mathf.Approximately(aspectRatio, 1.0f) == false)
        {
           // first decide if my image is wider than its height or taller than its width
           bool isImageWider = aspectRatio > 1.0f;

           newImageRendererWidth = isImageWider ? _imageOriginalRect.width : _imageOriginalRect.width * aspectRatio;
           newImageRendererHeight = isImageWider ? _imageOriginalRect.height * (1 / aspectRatio) : _imageOriginalRect.height;
        }
        
        // either reset or apply new renderer width and height
        _objectImageRenderer.rectTransform.sizeDelta = new Vector2(newImageRendererWidth, newImageRendererHeight);
        
        // Apply new sprite
        _objectImageRenderer.sprite = newSprite;
    }
    public void SetSprite(GameObject newSpriteObject) // methodology for when the balloon is SpriteRender
    {
        if (_currentSpriteObject != null) _currentSpriteObject.SetActive(false);
        _currentSpriteObject = newSpriteObject;
        _currentSpriteObject.SetActive(true);
    } 

    #endregion
}
