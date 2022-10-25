using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionPickup))]
public class InteractionBackpack : Interaction
{
    [SerializeField]
    private InteractionPickup _interactionPickup;

    public override void Execute()
    {
        base.Execute();

        // needs the interactable utself and the type
        BackpackController.BackpackInstance.AddItemToBackpack(this.gameObject, _interactionPickup.TypeOfPickup, _interactionPickup.SpriteRenderPickup, 1);

        Debug.Log("backpacking");
    }
}
