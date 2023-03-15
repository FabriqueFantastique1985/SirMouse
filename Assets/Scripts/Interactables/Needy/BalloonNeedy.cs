using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public ListNeedyBalloons Needy_Sprites_Wrap;

    [SerializeField]
    private float _timeLimitSwapSpriteBalloon = 2f;

    #endregion



    #region Fields

    private GameObject _currentlyShowingSpriteBalloon;

    #endregion

    private bool _iHaveMoreThan1SpriteBalloonNeedy;
    private float _timer;
    private int _indexSpriteBalloonNeedy;


    #region Unity Methods

    private void Start()
    {
        if (Needy_Sprites_Wrap.NeedyBalloons.Count > 1)
        {
            _iHaveMoreThan1SpriteBalloonNeedy = true;
        }
        else
        {
            _iHaveMoreThan1SpriteBalloonNeedy = false;
        }

        InitializeSprite();

        this.enabled = false;
    }

    private void Update()
    {
        // cycle through my sprite at intervals
        _timer += Time.deltaTime;
        if (_timer >= _timeLimitSwapSpriteBalloon)
        {
            _timer = 0;

            _indexSpriteBalloonNeedy += 1;
            if (_indexSpriteBalloonNeedy >= Needy_Sprites_Wrap.NeedyBalloons.Count)
            {
                _indexSpriteBalloonNeedy = 0;
            }

            SetSprite(Needy_Sprites_Wrap.NeedyBalloons[_indexSpriteBalloonNeedy].gameObject);
        }
    }

    #endregion




    #region Methods

    public void Show()
    {
        if (_iHaveMoreThan1SpriteBalloonNeedy == true)
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
        if (_currentlyShowingSpriteBalloon != null)
        {
            _currentlyShowingSpriteBalloon.SetActive(false);
        }
        
        _currentlyShowingSpriteBalloon = newSpriteObject;
        _currentlyShowingSpriteBalloon.SetActive(true);
    }
    public void InitializeSprite()
    {
        // disable all other needy ballons on start
        _indexSpriteBalloonNeedy = 0;
        _currentlyShowingSpriteBalloon = Needy_Sprites_Wrap.NeedyBalloons[_indexSpriteBalloonNeedy].gameObject;

        // disable all balloons
        for (int i = 0; i < Needy_Sprites_Wrap.NeedyBalloons.Count; i++)
        {
            Needy_Sprites_Wrap.NeedyBalloons[i].gameObject.SetActive(false);
        }

        // enable first balloon
        SetSprite(Needy_Sprites_Wrap.NeedyBalloons[_indexSpriteBalloonNeedy].gameObject);
    }



    public void UpdatePossibleBalloonsToShow(List<ListNeedyObjectsInMe> needyBalloons, int index)
    {
        // (needyBalloons.remove(needyBalloons[i]))
        needyBalloons.Remove(needyBalloons[index]);
        // reset timer and index -> continue cycling through needyBallons that have not been finished
        _timer = 0;
        _indexSpriteBalloonNeedy = 0;
        if (Needy_Sprites_Wrap.NeedyBalloons.Count > 0)
        {
            SetSprite(Needy_Sprites_Wrap.NeedyBalloons[_indexSpriteBalloonNeedy].gameObject);
        }

        // If this lists.count == 0, goal achieved (does nothing special, Debug.Log("Completed a Needy Interactabled")
        if (needyBalloons.Count == 0)
        {
            Debug.Log("You have succesfully finished a Needy interactable");
        }
    }  
    public bool CheckIfOneBalloonHasAllItems(ListNeedyObjectsInMe balloonOfInterest)
    {
        for (int i = 0; i < balloonOfInterest.NeedyObjects.Count; i++)
        {
            if (balloonOfInterest.NeedyObjects[i].Delivered == false)
            {
                return false;
            }
        }
        balloonOfInterest.CompletedMe = true;
        return true;
    }
    public void UpdateOneRequiredNeedyPickup(Type_Pickup pickupDelivered)
    {
        var needyBalloons = Needy_Sprites_Wrap.NeedyBalloons;

        bool foundNeedyBalloonToCheck = false;
        for (int i = 0; i < needyBalloons.Count; i++)
        {
            // find the balloon of interest...
            if (needyBalloons[i].PickupsNeeded == pickupDelivered)
            {
                foundNeedyBalloonToCheck = true;

                // loop over the Needy objects, get a needy object which has not been delivered yet
                bool foundNeedyToActivate = false;
                for (int j = 0; j < needyBalloons[i].NeedyObjects.Count; j++)
                {
                    if (needyBalloons[i].NeedyObjects[j].Delivered == false)
                    {
                        foundNeedyToActivate = true;

                        needyBalloons[i].NeedyObjects[j].SpriteFull.SetActive(true);
                        needyBalloons[i].NeedyObjects[j].Delivered = true;

                        // check if this delivered everything...
                        if (CheckIfOneBalloonHasAllItems(needyBalloons[i]) == true)
                        {
                            // If I have deivered everything for this one balloon, remove it from possible balloons to show
                            UpdatePossibleBalloonsToShow(needyBalloons , i);
                        }

                        break;
                    }
                }
                if (foundNeedyToActivate == false)
                {
                    Debug.Log("Could noy find a Needy Object that was not yet delivered, doing nothing here. Pherhaps check Delivered bool ?");
                }
                break;
            }
        }
        if (foundNeedyBalloonToCheck == false)
        {
            Debug.Log("Could not find a Needy Balloon to check for. Perhaps check if you assigned a proper Type to the needy Balloons ?");
        }
    }
    public void UpdateOneRequiredTouchable()
    {
        var needyBalloons = Needy_Sprites_Wrap.NeedyBalloons;

        // get the 1 balloon of interest...(always is 1 balloon for touchables currently -> index 0 needyBlaloons)
        // update 1 of the nondelivered object within it...
        for (int i = 0; i < needyBalloons[0].NeedyObjects.Count; i++)
        {
            if (needyBalloons[0].NeedyObjects[i].Delivered == false)
            {
                needyBalloons[0].NeedyObjects[i].Delivered = true;
                needyBalloons[0].NeedyObjects[i].SpriteFull.SetActive(true);

                break;
            }
        }
    }

    public void ResetMyNeedyObjects()
    {
        // get all componentsChildren of NeedyBalloons, add all of them to a the NeedyBalloons list again
        // go through all my balloons, and all of their NeedyObjects , reset bools balloon and needyobject, reset spriteFull
        Needy_Sprites_Wrap.NeedyBalloons = Needy_Sprites_Wrap.GetComponentsInChildren<ListNeedyObjectsInMe>().ToList();
        for (int i = 0; i < Needy_Sprites_Wrap.NeedyBalloons.Count; i++)
        {
            var balloonToReset = Needy_Sprites_Wrap.NeedyBalloons[i];
            for (int j = 0; j < balloonToReset.NeedyObjects.Count; j++)
            {
                var needyToReset = balloonToReset.NeedyObjects[j];
                needyToReset.SpriteFull.SetActive(false);
                needyToReset.Delivered = false;
            }
            balloonToReset.CompletedMe = false;
        }
    }

    #endregion
}
