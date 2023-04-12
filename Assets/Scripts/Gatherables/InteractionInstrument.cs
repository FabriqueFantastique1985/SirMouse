using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrument : Interaction
{
    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        Debug.Log("INTERACTED WOOHOO");
    }


    // method to hide any objects that shouldn't be there anymore (like a chest)
    public virtual void HideInteraction()
    {

    }
}
