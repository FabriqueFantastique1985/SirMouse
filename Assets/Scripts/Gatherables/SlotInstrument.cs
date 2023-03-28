using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class SlotInstrument : ButtonBaseNew
{
    [SerializeField]
    private Collider _buttonCollider;

    [Header("Instrument related")]
    public Type_Instrument InstrumentType;
    public bool Unlocked;
    public bool IsActive;

    [Header("Button Component")]
    public Button ButtonComponent;

    [Header("References Children")]
    public GameObject GlowActivation;
    public GameObject SpriteFull;
    

    public override void ClickedButton()
    {
        base.ClickedButton();

        // de-activate other ones
        InstrumentController.Instance.DeactivateInstrument();

        // activate this one
        InstrumentController.Instance.ActivateInstrument(this);

        // equip this one
        GameManager.Instance.Player.PushState(new InstrumentEquipState(GameManager.Instance.Player, InstrumentType));

        // turn off the current pages (maybe not this ?)
        PageController.Instance.TurnPageOff(PageType.BackpackInstruments);
        PageController.Instance.TurnPageOff(PageType.BackpackButtons);
        PageController.Instance.ShowGameplayHUD(true);
    }

    public void UnlockThisSlot()
    {
        Unlocked = true;
        SpriteFull.SetActive(true);
        _buttonCollider.enabled = true;
        ButtonComponent.enabled = true;
    }
}
