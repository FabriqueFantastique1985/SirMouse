using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInstrument : ButtonBaseNew
{
    [SerializeField]
    private Collider _buttonCollider;

    [Header("Instrument related")]
    public Type_Instrument InstrumentType;
    public bool Unlocked;
    public bool IsActive;

    public GameObject SpriteFull;
    public GameObject GlowActivation;

    public override void ClickedButton()
    {
        base.ClickedButton();

        // de-activate other ones
        InstrumentController.Instance.DeactivateInstrument();

        // activate this one
        InstrumentController.Instance.ActivateInstrument(this);

        // equip this one
        InstrumentController.Instance.EquipInstrument(InstrumentType);

        // turn off the current pages (maybe not this ?)
    }

    public void UnlockThisSlot()
    {
        Unlocked = true;
        SpriteFull.SetActive(true);
        _buttonCollider.enabled = true;      
    }
}
