using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupInteraction))]
public class InteractionBackpack : Interaction
{
    [Header("Other")]

    [SerializeField]
    private Interactable _interactableComponent;

    [SerializeField]
    private PickupInteraction _interactionPickup;

    [SerializeField]
    private float _scaleImage = 1;

    protected override void SpecificAction(Player player)
    {
        // needs the interactable itself and the type
        BackpackController.Instance.AddItemToBackpackFromFloor(_interactableComponent, this.gameObject, _interactableComponent.MyPickupType, _interactionPickup.SpriteRenderPickup, _scaleImage);
        //Debug.Log($"Added item: {gameObject}, {_interactionPickup.TypeOfPickup} to the backpack.");
    }
}
