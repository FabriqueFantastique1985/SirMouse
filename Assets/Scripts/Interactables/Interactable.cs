using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IDataPersistence
{
    #region Events

    public delegate void InteractableDelegate();

    public event InteractableDelegate OnInteracted;

    #endregion

    [SerializeField]
    private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    /// <summary>
    /// Balloon used to execute an interaction.
    /// </summary>
    [Header("Balloon components")]  
    public Balloon InteractionBalloon;

    /// <summary>
    /// Balloon used to scroll between the interactions
    /// </summary>
    [SerializeField]
    private Balloon _swapBalloon;

    [Header("Assign proper type if I have a pickup interaction")]
    public Type_Pickup MyPickupType;

    /// <summary>
    /// List of possible interactions with this interactable
    /// </summary>
    [SerializeField]
    private List<Interaction> _interactions = new List<Interaction>();

    protected int _currentInteractionIndex = 0;

    #region Properties

    /// <summary>
    /// Reference to the list of possible Interactions of this Interactable.
    /// </summary>
    public List<Interaction> Interactions
    {
        get => _interactions;
    }

    #endregion
    
    private void Start()
    {
        InteractionBalloon.OnBalloonClicked += OnInteractBalloonClicked;
        InteractionBalloon.gameObject.SetActive(false);

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
        InteractionBalloon.SetSprite(_interactions[0].SpriteObjectInteractionBalloon);
    }
    protected virtual void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        // Execute current interaction
        if (_currentInteractionIndex < 0 || _interactions.Count <= 0)
        {
            Debug.LogError("Tried to execute an interaction that either did not exist or wasn't setup correctly!");
            return;
        }
        _interactions[_currentInteractionIndex].Execute(player);

        OnInteracted?.Invoke();
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
        _currentInteractionIndex = (_currentInteractionIndex + 1) % _interactions.Count;
        InteractionBalloon.SetSprite(_interactions[_currentInteractionIndex].SpriteObjectInteractionBalloon);
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
        // No balloon required when there are no interactions
        if (_interactions.Count <= 0) return;
        
        InteractionBalloon.Show();
        
        // Swap balloon is required if there's more than one interaction
        if (_interactions.Count > 1) _swapBalloon.Show();
    }

    private void HideInteractionBalloon()
    {
        // Nothing to hide if there are no interactions to begin with
        if (_interactions.Count <= 0) return;
        
        InteractionBalloon.Hide();
        
        // Also hide the swapballoon if there's more than one interaction
        if (_interactions.Count > 1) _swapBalloon.Hide();
    }
    
    #endregion
    
    #region Public Functions

    public virtual void HideBalloonBackpack()
    {
        HideInteractionBalloon();
    }

    #endregion

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data)
    {
    }
}
