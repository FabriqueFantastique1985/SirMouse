using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPickup : Interaction
{
    public Type_Pickup TypeOfPickup;
    public SpriteRenderer SpriteRenderPickup;

    protected override void SpecificAction(Player player)
    {
        Debug.Log("picking up");
    }
}
