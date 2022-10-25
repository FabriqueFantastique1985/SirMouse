using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionPickup))]
public class InteractionBackpack : Interaction
{
    [SerializeField]
    private InteractionPickup _interactionPickup;

    protected override void SpecificAction(Player player)
    {
        // needs the interactable itself and the type
        BackpackController.BackpackInstance.AddItemToBackpack(this.gameObject, _interactionPickup.TypeOfPickup, _interactionPickup.SpriteRenderPickup, 1);
        Debug.Log($"Added item: {gameObject}, {_interactionPickup.TypeOfPickup} to the backpack.");
    }
}
