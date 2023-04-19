using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPickupBackpack : ButtonPaging
{
    public Interactable MyInteractable;
    public Type_Pickup MyPickupType;

    public Image MyImage;

    protected override void TurnOnPage()
    {
        BackpackController.Instance.RemoveItemFromBackpackThroughButton(MyInteractable, MyPickupType, this);

        _pageInstance.OpenBagImage(false);
    }
}
