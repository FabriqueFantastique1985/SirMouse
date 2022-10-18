﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Balloon : MonoBehaviour
{
    #region Events

    public delegate void BalloonDelegate(Balloon sender, Player player);

    /// <summary>
    /// Gets invoked when clicked on.
    /// </summary>
    public event BalloonDelegate OnBalloonClicked;
    
    #endregion

    #region EditorFields

    [SerializeField]
    private Collider _balloonTrigger;

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
    private Sprite _spriteInBalloon;
    //[SerializeField]
    //private GameObject _spriteInBalloonParent;

    #endregion

    #region Fields



    #endregion

    #region Methods

    public void Click(Player player)
    {
        if (_balloonAnimator != null) _balloonAnimator.Play(_animPop);
        if (_disableOnClick) StartCoroutine(DisableBalloon());
        OnBalloonClicked?.Invoke(this, player);
    }
    
    private IEnumerator DisableBalloon()
    {
        // disable the collider -> wait a bit -> disable the gameobject + enable the collider
        _balloonTrigger.enabled = false;

        yield return new WaitForSeconds(0.35f); // should be the length of the animation "Pop"

        gameObject.SetActive(false);
        _balloonTrigger.enabled = true;
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        if (_balloonAnimator != null) _balloonAnimator.Play(_animFloat);

        //  if (_interactionPossibleTotal > 1) 
        //  {
        //      // show swap balloon
        //      _swapBalloon.gameObject.SetActive(true);
        //  }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);

        // if (_swapBalloon != null)
        // {
        //     // show swap balloon
        //     _swapBalloon.gameObject.SetActive(false);
        // }
    }



    public void SetSprite(Sprite newSprite)
    {
        _spriteInBalloon = newSprite;
    }
    //public void SetSprite(GameObject newSpriteParent)
    //{
    //    _spriteInBalloonParent = newSpriteParent;
    //    // this method will see change
    //}

    #endregion

}
