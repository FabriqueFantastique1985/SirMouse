﻿using System.Collections;
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
        BackpackController.BackpackInstance.RemoveItemFromBackpackThroughButton(MyInteractable, MyPickupType, this.gameObject);

        _pageInstance.OpenBagImage(false);
    }
}
