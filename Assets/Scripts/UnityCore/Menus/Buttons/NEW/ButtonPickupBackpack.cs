using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPickupBackpack : ButtonPaging
{
    public GameObject MyInteractable;
    public Type_Pickup MyPickupType;

    public Image MyImage;

    protected override void TurnOnPage()
    {
        BackpackController.BackpackInstance.RemoveItemFromBackpack(MyInteractable, MyPickupType, this.gameObject);
    }
}
