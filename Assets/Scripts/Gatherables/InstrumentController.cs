using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    public static InstrumentController Instance { get; private set; }

    public Type_Instrument EquipedInstrument;

    [Header("Instruments on Player")]
    [SerializeField]
    private List<InstrumentPiece> _playerInstruments = new List<InstrumentPiece>();

    [Header("Instrument Buttons")]
    [SerializeField]
    private List<SlotInstrument> _slotsInstruments = new List<SlotInstrument>();

    public SlotInstrument ActiveInstrumentSlot;
    public InstrumentPiece ActiveInstrumentPiece;


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



    // called on the gatherable being picked up
    public void UnlockInstrument(Type_Instrument instrumentToUnlock)
    {
        for (int i = 0; i < _slotsInstruments.Count; i++)
        {
            if (_slotsInstruments[i].InstrumentType == instrumentToUnlock)
            {
                SlotInstrument slotInstrumentOfInterest = _slotsInstruments[i];

                slotInstrumentOfInterest.UnlockThisSlot();
                break;
            }
        }
    }


    // called on buttons in Backpack 
    public void ActivateInstrument(SlotInstrument slotToActivate)
    {
        ActiveInstrumentSlot = slotToActivate;

        ActiveInstrumentSlot.IsActive = true;
        ActiveInstrumentSlot.GlowActivation.SetActive(true);
    }
    public void DeactivateInstrument()
    {
        if (ActiveInstrumentSlot != null)
        {
            ActiveInstrumentSlot.GlowActivation.SetActive(false);
            ActiveInstrumentSlot.IsActive = false;
            ActiveInstrumentSlot = null;
        }
    }


    // called when button is pressed on gameplay UI
    public void EquipInstrument(Type_Instrument instrumentToEquip)
    {
        // always unequip first
        if (EquipedInstrument != Type_Instrument.None)
        {
            UnEquipInstrument();
        }
        
        for (int i = 0; i < _playerInstruments.Count; i++)
        {
            if (_playerInstruments[i].InstrumentType == instrumentToEquip)
            {
                // push state which does the unequip animation -> equip animation
                // (start of the second animation will have interaction event which will pass through the Type_Instrument (?)

                // this could all happen in de animation event (?)
                InstrumentPiece instrumentOfInterest = _playerInstruments[i];
                ActiveInstrumentPiece = instrumentOfInterest;
                ActiveInstrumentPiece.gameObject.SetActive(true);
                EquipedInstrument = instrumentToEquip;

                break;
            }
        }
    }
    public void UnEquipInstrument()
    {
        // called in the animation event (as well)(?) 

        ActiveInstrumentPiece.gameObject.SetActive(false);
        ActiveInstrumentPiece = null;
        EquipedInstrument = Type_Instrument.None;
    }



}
