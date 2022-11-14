using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteraction : Interaction
{
    [Header("PickUpInteraction fields")]
    [SerializeField]
    private Interactable _pickUpInteractable;

    [SerializeField]
    private bool _isTwoHandPickup = false;

    public Type_Pickup TypeOfPickup;
    public SpriteRenderer SpriteRenderPickup;

    protected override void SpecificAction(Player player)
    {
        player.PushState(new PickUpState(player, _pickUpInteractable, TypeOfPickup, _isTwoHandPickup));
    }

    protected override bool Prerequisite(Player player)
    {
        return player.EquippedItem == null;
    }
}
