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
}
