using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

public class BalloonNeedy : MonoBehaviour
{
    #region EditorFields

    [SerializeField]
    protected Animator _balloonAnimator;

    [Header("BalloonAnimations")]
    [SerializeField]
    private string _animFloat = "Balloon_Floaty";

    [Header("Needy Balloon sprites")]
    public List<GameObject> NeedySpriteObjects = new List<GameObject>();

    [SerializeField]
    private float _timeLimitSwapSprite = 2f;

    #endregion



    #region Fields

    private GameObject _currentSpriteObject;

    #endregion

    private bool _iHaveMoreThan1SpriteNeedy;
    private float _timer;
    private int _indexSpritesNeedy;


    #region Unity Methods

    private void Start()
    {
        if (NeedySpriteObjects.Count > 1)
        {
            _iHaveMoreThan1SpriteNeedy = true;
        }
        else
        {
            _iHaveMoreThan1SpriteNeedy = false;
        }

        _indexSpritesNeedy = NeedySpriteObjects.Count - 1;
        _currentSpriteObject = NeedySpriteObjects[_indexSpritesNeedy];

        this.enabled = false;
    }

    private void Update()
    {
        // cycle through my sprite at intervals
        _timer += Time.deltaTime;
        if (_timer >= _timeLimitSwapSprite)
        {
            _timer = 0;

            _indexSpritesNeedy += 1;
            if (_indexSpritesNeedy >= NeedySpriteObjects.Count)
            {
                _indexSpritesNeedy = 0;
            }

            SetSprite(NeedySpriteObjects[_indexSpritesNeedy]);
        }
    }

    #endregion




    #region Methods

    public void Show()
    {
        if (_iHaveMoreThan1SpriteNeedy == true)
        {
            this.enabled = true;
        }

        gameObject.SetActive(true);

        if (_balloonAnimator != null)
        {
            _balloonAnimator.Play(_animFloat);
        }       
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        this.enabled = false;
    }



    public void SetSprite(GameObject newSpriteObject) // methodology for when the balloon is SpriteRender
    {
        if (_currentSpriteObject != null)
        {
            _currentSpriteObject.SetActive(false);
        }
        
        _currentSpriteObject = newSpriteObject;
        _currentSpriteObject.SetActive(true);
    }

    #endregion
}
