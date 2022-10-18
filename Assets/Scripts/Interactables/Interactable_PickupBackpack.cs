using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_PickupBackpack : Interactable
{
    [SerializeField]
    private PickupType _pickupType;

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
        /*
         * - parent the interactable to gamemanager (or any other part of dont destroyonload)
         * - setActive(false) the interactable
         * - add the itemType to the list
         */

        this.transform.SetParent(GameManager.Instance.transform);
        BackpackController.AddItemToBackpack(this.gameObject, _pickupType);
        this.gameObject.SetActive(false);
    }

    #endregion


}
