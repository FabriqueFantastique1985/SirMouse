using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentInteractableOfInterest : PlaceOfInterest
{
    [Header("Instrument Interaction parent")]
    [SerializeField]
    private InstrumentInteractable _instrumentInteractable;


    public override void HideIcon()
    {
        base.HideIcon();

        _instrumentInteractable.ShowInstrumentPopup();
    }
    public override void ShowIcon()
    {
        base.ShowIcon();

        _instrumentInteractable.HideInstrumentPopup();
    }
}
