using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNeedyPickups : InteractableNeedy
{
    [Header("Needing Pickups")]
    public List<Type_Pickup> WantedPickups;  // update this runtime + save

    public List<Type_Pickup> HeldPickups;  // update this runtime + save


    public Interactable InteractableToActivateForReward;



    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  
        {
            bool hasItemOfInterest = false;
            for (int i = 0; i < WantedPickups.Count; i++)
            {
                // if The player has any pickups on them (or in their inventory) -> add them to _heldPickups 
                if (BackpackController.BackpackInstance.PlayerHasItemOfInterest(WantedPickups[i]) == true)
                {
                    hasItemOfInterest = true;
                    break;
                }
            }

            if (hasItemOfInterest == true)
            {
                // show interactBalloon 
                ShowInteractionBalloon(); // clicking this delivers in the pickups you currently have (Interaction_Deliver)

                // BElOW CAN BE CHECKED FOR IN "InteractionNeedyPickupDelivered"
                // the previously clicked interaction should remove the sprites in the needy balloon that have been delivered
                //- to do...
                // the previously clicked interaction will then check if heldpickups has been filled 
                //- DONE ?
                // if completely filled -> activate another interactable , this interactables' interaction gives a reward
                //- DONE ?
                // after this reward has been given, possibly activate another interactable such as this one !
                //- DONE ?
            }
            else
            {
                ShowNeedyBalloon();
            }
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  
        {
            HideInteractionBalloon();
            HideNeedyBalloon();
        }
    }
}
