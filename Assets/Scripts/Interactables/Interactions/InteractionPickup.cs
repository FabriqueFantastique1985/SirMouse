using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPickup : Interaction
{
    public Type_Pickup TypeOfPickup;
    public SpriteRenderer SpriteRenderPickup;

    public override void Execute()
    {
        base.Execute();

        Debug.Log("picking up");
    }
}
