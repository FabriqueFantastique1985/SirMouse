using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstrumentInteractableData
{
    public bool Finished;
}

[RequireComponent(typeof(ID))]
public class InstrumentInteractable : MonoBehaviour, IDataPersistence
{
    public delegate void InteractableDelegate();
    public event InteractableDelegate OnInteracted;


    [Header("Instrument I Need")]
    [SerializeField]
    private Type_Instrument InstrumentRequired;

    [Header("Specific Interaction I Do")]
    [SerializeField]
    private InteractionInstrument _instrumentInteraction;

    [Header("trigger to enter")]
    [SerializeField]
    private InstrumentInteractableOfInterest _placeOfInterest;

    [HideInInspector]
    public bool Finished;

    [Header("Balloon Thinking")]
    [SerializeField]
    private GameObject _thinkingBalloon;
    //[SerializeField]
    //private Animator _thinkingBalloonAnimator;
    [SerializeField]
    private GameObject _spriteThinkingToSpawnInstrumentIn;

    [Header("Balloon Interaction")]
    [SerializeField]
    private Balloon _interactionBalloon;
    [SerializeField]
    private Animator _interactionBalloonAnimator;
    [SerializeField]
    private GameObject _spriteInteractionToSpawnInstrumentIn;

    private GameObject _currentlyActiveBalloon;

    [Header("Instrument prefabs to spawn in balloons")]
    [SerializeField]
    private List<GameObject> _prefabsToSpawnInBalloonsThinking = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _prefabsToSpawnInBalloonsInteraction = new List<GameObject>();

    [SerializeField]
    private ID _id;

    private void Start()
    {
        // subscribe event
        _interactionBalloon.OnBalloonClicked += OnInteractBalloonClicked;

        // create the correct visual in the bubbles
        CreateVisualInBalloons();
        // disable balloons on start
        DisableBalloonsOnStart();

        // if Im finished, disable my colliders
        if (Finished == true)
        {
            _placeOfInterest.HideIconPermanently();
            _instrumentInteraction.HideInteraction();
        }
    }


    protected virtual void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        if (_instrumentInteraction != null)
        {
            _instrumentInteraction.Execute(player);
        }

        // play animation to hide balloon
        _interactionBalloonAnimator.Play("Clicked");
        // hide the icon
        _placeOfInterest.HideIconPermanently();
        // set finished bool
        Finished = true;

        OnInteracted?.Invoke();
        Debug.Log("Interacted with: " + sender.gameObject.name + " by player:" + player.gameObject.name);

        DataPersistenceManager.Instance.SaveGame();
    }



    // called in InstrumentInteractionOfInterest
    public void ShowInstrumentPopup()
    {
        // check if player has desired instrument equiped
        if (InstrumentController.Instance.EquipedInstrument == InstrumentRequired)
        {
            // bubble click
            _interactionBalloon.gameObject.SetActive(true);
            _currentlyActiveBalloon = _interactionBalloon.gameObject;
        }
        else
        {
            // bubble transparent
            _thinkingBalloon.SetActive(true);
            _currentlyActiveBalloon = _thinkingBalloon;
        }
    }
    public void HideInstrumentPopup()
    {
        if (_currentlyActiveBalloon != null)
        {
            _currentlyActiveBalloon.SetActive(false);
        }     
        _currentlyActiveBalloon = null;
    }



    private void CreateVisualInBalloons()
    {
        if (InstrumentRequired != Type_Instrument.None)
        {
            GameObject chosenInstrumentThinking = _prefabsToSpawnInBalloonsThinking[((int)InstrumentRequired) - 1];
            GameObject chosenInstrumentInteraction = _prefabsToSpawnInBalloonsInteraction[((int)InstrumentRequired) - 1];

            Instantiate(chosenInstrumentThinking, _spriteThinkingToSpawnInstrumentIn.transform);
            Instantiate(chosenInstrumentInteraction, _spriteInteractionToSpawnInstrumentIn.transform);
        }
    }
    private void DisableBalloonsOnStart()
    {
        _thinkingBalloon.SetActive(false);
        _interactionBalloon.gameObject.SetActive(false);

        _currentlyActiveBalloon = null;
    }




    public void LoadData(GameData data)
    {
        if (data.InstrumentInteractionSavedData.ContainsKey(_id))
        {
            // get the correct key...
            var instrumentInteractionData = data.InstrumentInteractionSavedData[_id];

            // get the value 
            Finished = instrumentInteractionData.Finished;

            // if Im finished, disable my colliders
            if (Finished == true)
            {
                _placeOfInterest.HideIconPermanently();
                _instrumentInteraction.HideInteraction();
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (_id == string.Empty)
        {
            Debug.LogWarning("No id yet made! Please generate one!");
            return;
        }

        InstrumentInteractableData instrumentInteractionData = new InstrumentInteractableData();

        // assign current bool, to the data bool
        instrumentInteractionData.Finished = Finished;
        
        // update the data using correct key
        data.InstrumentInteractionSavedData[_id] = instrumentInteractionData;
    }
}
