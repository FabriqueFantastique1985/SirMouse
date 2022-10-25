using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_PickupBackpack : Interactable
{
    public Type_Pickup PickupType; // this pickuptype should be on the component 

    protected override void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);

        // code whatever the interaction does here
        switch (_currentInteractionIndex)
        {
            case 0:
                PickUp();
                break;
            case 1:
                Backpackit();
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
    protected virtual void Backpackit()
    {
        Debug.Log("Backpacked");

        //HideBalloonBackpack();
        //BackpackController.BackpackInstance.AddItemToBackpack(this.gameObject, PickupType);
    }

    #endregion


}
