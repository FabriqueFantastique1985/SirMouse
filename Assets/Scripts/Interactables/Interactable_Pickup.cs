using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Pickup : Interactable
{
    [SerializeField]
    private PickupType Pickup_0;

    protected override void InitializeThings()
    {
        base.InitializeThings();
    }


    protected override void ShowBalloon(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null) // add to if --> whether the item the player has == the same as the _requiredPickup
        {
            _balloon.gameObject.SetActive(true);
            _balloonAnimator.Play(_animFloat);
        }
    }
}
