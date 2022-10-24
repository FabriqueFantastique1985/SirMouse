using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBackpack : Interaction
{
    public override void Execute()
    {
        base.Execute();

        // needs the interactable utself and the type
        //BackpackController.BackpackInstance.AddItemToBackpack(this.gameObject, PickupType);
    }
}
