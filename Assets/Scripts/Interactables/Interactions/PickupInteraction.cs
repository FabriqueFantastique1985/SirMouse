using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteraction : Interaction
{
    [Header("PickUpInteraction fields")]
    [SerializeField]
    private Interactable _pickUpInteractable;
   
    public bool IsTwoHandPickup = false;

    public Type_Pickup TypeOfPickup;
    public SpriteRenderer SpriteRenderPickup;

    protected override void SpecificAction(Player player)
    {
        player.PushState(new PickUpState(player, _pickUpInteractable, TypeOfPickup, IsTwoHandPickup));
    }

    protected override bool Prerequisite(Player player)
    {
        return player.EquippedItem == null;
    }
}
