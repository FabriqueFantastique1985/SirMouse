using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Balloon components")]
    [SerializeField]
    protected InteractBalloon _balloon;
    [SerializeField]
    private Collider _balloonTrigger;
    [SerializeField]
    protected Animator _balloonAnimator;

    protected const string _animFloat = "Balloon_Floaty";
    protected const string _animPop = "Balloon_Pop";

    [Header("Swap Balloon components")]
    [SerializeField]
    protected InteractSwapBalloon _swapBalloon;
    [SerializeField]
    private Collider _swapBalloonTrigger;
    [SerializeField]
    protected Animator _swapBalloonAnimator;

    protected const string _animSwapPop = "SwapBalloon_Pop";

    protected int _interactionCurrentValue;
    protected int _interactionTotal;   // this is set in initialize (sprites should be linked to the currentvalue)
    protected int _interactionPossibleTotal = 2; // this is normally set in the OnTriggerEnter...(as it requires the info whether player has x pickup)

    [Header("Balloon sprite parents")]
    [SerializeField]
    private List<GameObject> _spriteParents = new List<GameObject>();


    private void Start()
    {
        _balloon.OnBalloonClicked += OnInteractBalloonClicked;
        _balloon.gameObject.SetActive(false);

        if (_swapBalloon != null)
        {
            _swapBalloon.OnSwapBalloonClicked += OnInteractSwapBalloonClicked;
            _swapBalloon.gameObject.SetActive(false);
        }

        InitializeThings();
    }




    #region Virtual Functions

    protected virtual void InitializeThings()
    {
        // extra method that inheriting classes can use to still use the Start function
    }
    protected virtual void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);

        _balloonAnimator.Play(_animPop);
        StartCoroutine(DisableBalloon());
    }
    protected virtual void OnInteractSwapBalloonClicked(InteractSwapBalloon sender, Player player)
    {
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);

        _swapBalloonAnimator.Play(_animSwapPop);
        // update sprites & int
        AdjustInteraction();
    }


    
    protected virtual void ShowBalloon(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            _balloon.gameObject.SetActive(true);
            _balloonAnimator.Play(_animFloat);

            if (_interactionPossibleTotal > 1) 
            {
                // show swap balloon
                _swapBalloon.gameObject.SetActive(true);
            }
        }
    }
    protected virtual void HideBalloon(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            _balloon.gameObject.SetActive(false);

            if (_swapBalloon != null)
            {
                // show swap balloon
                _swapBalloon.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Private Functions

    private IEnumerator DisableBalloon()
    {
        // disable the collider -> wait a bit -> disable the gameobject + enable the collider
        _balloonTrigger.enabled = false;

        yield return new WaitForSeconds(0.35f); // should be the length of the animation "Pop"

        _balloon.gameObject.SetActive(false);
        _balloonTrigger.enabled = true;
    }
    // current way of adjusting interaction will not always work
    // -> example: interaction_0 & interaction_2 are possible, but interaction_1 not --> this logic would show the wrong sprites of 0 & 1 unless updated !
    private void AdjustInteraction()
    {
        _interactionCurrentValue += 1;  

        if (_interactionCurrentValue >= _interactionPossibleTotal)
        {
            _interactionCurrentValue = 0;
        }

        for (int i = 0; i < _interactionPossibleTotal; i++)
        {
            _spriteParents[i].SetActive(false);
        }
        _spriteParents[_interactionCurrentValue].SetActive(true);
    }

    #endregion

    #region Public Functions

    public virtual void HideBalloonBackpack()
    {
        _balloon.gameObject.SetActive(false);

        if (_swapBalloon != null)
        {
            // show swap balloon
            _swapBalloon.gameObject.SetActive(false);
        }    
    }

    #endregion



    private void OnTriggerEnter(Collider other)
    {
        ShowBalloon(other);
    }


    private void OnTriggerExit(Collider other)
    {
        HideBalloon(other);
    }
}
