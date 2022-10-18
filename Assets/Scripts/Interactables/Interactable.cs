using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Balloon used to execute an interaction.
    /// </summary>
    [Header("Balloon components")]
    [SerializeField]
    private Balloon _interactionBalloon;

    /// <summary>
    /// Balloon used to scroll between the interactions
    /// </summary>
    [SerializeField]
    private Balloon _swapBalloon;

    [SerializeField]
    private List<Interaction> _interactions = new List<Interaction>();

    protected int _interactionCurrentValue = 0;

    private void Start()
    {
        _interactionBalloon.OnBalloonClicked += OnInteractBalloonClicked;
        _interactionBalloon.gameObject.SetActive(false);

        if (_swapBalloon != null)
        {
            _swapBalloon.OnBalloonClicked += OnInteractSwapBalloonClicked;
            _swapBalloon.gameObject.SetActive(false);
        }

        Initialize();
    }

    #region Virtual Functions

    protected virtual void Initialize()
    {
        // extra method that inheriting classes can use to still use the Start function
        _interactionBalloon.SetSprite(_interactions[0].InteractionSprite);
    }
    protected virtual void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);
    }
    protected virtual void OnInteractSwapBalloonClicked(Balloon sender, Player player)
    {
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);

        // update sprites & int
        AdjustInteraction();
    }

    #endregion

    #region Private Functions


    // current way of adjusting interaction will not always work
    // -> example: interaction_0 & interaction_2 are possible, but interaction_1 not --> this logic would show the wrong sprites of 0 & 1 unless updated !
    private void AdjustInteraction()
    {
        _interactionCurrentValue = (_interactionCurrentValue + 1) % _interactions.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            ShowInteractionBalloon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            HideInteractionBalloon();
        }
    }

    private void ShowInteractionBalloon()
    {
        _interactionBalloon.Show();
        if (_interactions.Count > 1) _swapBalloon.Show();
    }

    private void HideInteractionBalloon()
    {
        _interactionBalloon.Hide();
        if (_interactions.Count > 1) _swapBalloon.Hide();
    }
    
    #endregion
    
    #region Public Functions

    public virtual void HideBalloonBackpack()
    {
        HideInteractionBalloon();
    }

    #endregion

}
