using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    public delegate void InstrumentControllerDelegate(SlotInstrument instrument);
    public event InstrumentControllerDelegate OnInstrumentUnlocked;

    public static InstrumentController Instance { get; private set; }

    public InstrumentPiece EquipedInstrumentPiece;
    public Type_Instrument EquipedInstrument;

    [Header("Instrument Buttons")]
    [SerializeField]
    private List<SlotInstrument> _slotsInstruments = new List<SlotInstrument>();

    public SlotInstrument ActiveInstrumentSlot;
    public InstrumentPiece ActiveInstrumentPiece;

    public ButtonEquipToggle ButtonEquiping;

    [HideInInspector]
    public InstrumentInteractable InstrumentInteractableMouseIsIn;
    

    private void Awake()
    {
        // Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        for (int i = 0; i < _slotsInstruments.Count; i++)
        {
            if (_slotsInstruments[i].Unlocked == true)
            {
                ActivateInstrument(_slotsInstruments[i]);
                break;
            }
        }

    }



    // called on the gatherable being picked up
    public void UnlockInstrument(Type_Instrument instrumentToUnlock)
    {
        for (int i = 0; i < _slotsInstruments.Count; i++)
        {
            if (_slotsInstruments[i].InstrumentType == instrumentToUnlock)
            {
                SlotInstrument slotInstrumentOfInterest = _slotsInstruments[i];

                slotInstrumentOfInterest.UnlockThisSlot();

                OnInstrumentUnlocked?.Invoke(slotInstrumentOfInterest);

                PageController.Instance.ButtonInstrumentSuper.NotificationObject.SetActive(true);
                PageController.Instance.ButtonBackpackSuper.IhaveNotificationsLeftInstruments = true;
                PageController.Instance.NotifyBackpackSuper();
                break;
            }
        }
    }

    public bool CheckIfFoundInstrument()
    {
        for (int i = 0; i < _slotsInstruments.Count; i++)
        {
            if (_slotsInstruments[i].Unlocked == true)
            {
                return true;
            }
        }
        return false;
    }


    // called on buttons in Backpack 
    public void ActivateInstrument(SlotInstrument slotToActivate)
    {
        ActiveInstrumentSlot = slotToActivate;

        ActiveInstrumentSlot.IsActive = true;
        ActiveInstrumentSlot.GlowActivation.SetActive(true);

        for (int i = 0; i < GameManager.Instance.Player.Character.InstrumentsOnMe.Count; i++)
        {
            if (GameManager.Instance.Player.Character.InstrumentsOnMe[i].InstrumentType == slotToActivate.InstrumentType)
            {
                InstrumentPiece instrumentOfInterest = GameManager.Instance.Player.Character.InstrumentsOnMe[i];
                ActiveInstrumentPiece = instrumentOfInterest;

                break;
            }
        }

        ButtonEquiping.SetInstrumentSprite();
    }
    
    // called on buttons in Backpack
    public void DeactivateInstrument()
    {
        if (ActiveInstrumentSlot != null)
        {
            ActiveInstrumentSlot.GlowActivation.SetActive(false);
            ActiveInstrumentSlot.IsActive = false;
            ActiveInstrumentSlot = null;

            ActiveInstrumentPiece = null;
        }
    }


    // called when the player state InstrumentEquipState is pushed
    public void EquipInstrument(Type_Instrument instrumentToEquip)
    {
        // always unequip first
        if (EquipedInstrument != Type_Instrument.None)
        {
            UnEquipInstrument();
        }
        
        for (int i = 0; i < GameManager.Instance.Player.Character.InstrumentsOnMe.Count; i++)
        {
            if (GameManager.Instance.Player.Character.InstrumentsOnMe[i].InstrumentType == instrumentToEquip)
            {
                InstrumentPiece instrumentOfInterest = GameManager.Instance.Player.Character.InstrumentsOnMe[i];
                ActiveInstrumentPiece = instrumentOfInterest;
                //ActiveInstrumentPiece.gameObject.SetActive(true);

                EquipedInstrument = instrumentToEquip;           
                EquipedInstrumentPiece = GameManager.Instance.Player.Character.InstrumentsOnMe[i];

                EquipedInstrumentPiece.gameObject.SetActive(true);

                break;
            }
        }

        // check if the player is within a trigger of a required instrument...
        if (InstrumentInteractableMouseIsIn != null)
        {
            InstrumentInteractableMouseIsIn.ShowInstrumentPopup();
        }

        // show alternate button
        ButtonEquiping.SetButtonState(false);

        SkinsMouseController.Instance.HideOrShowSwordAndShield(false);
    }
    
    public void UnEquipInstrument()
    {
        if (!EquipedInstrumentPiece)
        {
            return;
        }

        //Debug.Log("Active piece is " + ActiveInstrumentPiece.gameObject.name);
        //ActiveInstrumentPiece.gameObject.SetActive(false);

        EquipedInstrument = Type_Instrument.None;

        EquipedInstrumentPiece.gameObject.SetActive(false);
        EquipedInstrumentPiece = null;

        // show the thinking balloon of any instrument interactable I am in
        if (InstrumentInteractableMouseIsIn != null)
        {
            InstrumentInteractableMouseIsIn.ShowInstrumentThink();
        }

        SkinsMouseController.Instance.HideOrShowSwordAndShield(true);

        // show normal button
        ButtonEquiping.SetButtonState(true);
    }
}
