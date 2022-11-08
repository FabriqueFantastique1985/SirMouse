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
    
    protected override void SpecificAction(Player player)
    {
        player.PushState(new PickUpState(player, _pickUpInteractable, _isTwoHandPickup));
    }

    protected override bool Prerequisite(Player player)
    {
        return player.EquippedItem == null;
    }
}
