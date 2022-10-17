using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_PickupBackpack : Interactable
{
    protected override void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);

        // code whatever the interaction does here
        switch (_interactionCurrentValue)
        {
            case 0:
                PickUp();
                break;
            case 1:
                Backpack();
                break;
            default:
                Debug.Log("Couldn't find the interaction on balloon click");
                break;
        }
    }




    #region Virtual Functions

    protected virtual void PickUp()
    {
        Debug.Log("PICKED UP");
    }
    protected virtual void Backpack()
    {
        Debug.Log("Backpacked");
    }

    #endregion


}
