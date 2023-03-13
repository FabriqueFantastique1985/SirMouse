using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNeedyPickups : InteractableNeedy
{
    [Header("Needing Pickups")]
    [SerializeField]
    private List<Type_Pickup> _wantedPickups;

    private List<Type_Pickup> _heldPickups;



    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            // if I have all the required objects 
            // show interactBalloon 

            ShowInteractionBalloon();
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  // if statement doesn't need to exist if we use layers to decide what can enter the trigger !
        {
            HideInteractionBalloon();
        }
    }
}
