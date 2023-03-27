using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEquipToggle : ButtonBaseNew
{
    [Header("Components")]
    public Button ButtonComponent;
    public Collider ColliderComponent;

    [Header("Reference children")]
    public GameObject ImageInsideButtonState0;
    public GameObject ImageInsideButtonState1;

    public override void ClickedButton()
    {
        base.ClickedButton();

        // additional equip logic
        if (InstrumentController.Instance.ActiveInstrumentPiece == null)
        {
            Debug.Log("not instrument has been activated, so pressing me does nothing");
        }
        else
        {
            if (InstrumentController.Instance.EquipedInstrument == Type_Instrument.None)
            {
                GameManager.Instance.Player.PushState(new InstrumentEquipState(GameManager.Instance.Player, InstrumentController.Instance.ActiveInstrumentPiece.InstrumentType));
            }
            else
            {
                GameManager.Instance.Player.PushState(new InstrumentUnequipState(GameManager.Instance.Player));
            }
        }

    }
}
