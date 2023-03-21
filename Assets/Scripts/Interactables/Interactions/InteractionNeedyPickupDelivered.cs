using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNeedyPickupDelivered : Interaction
{
    // Deliver any pickups that I want to me, remove them from the player

    [Header("My own Interactable")]
    [SerializeField]
    private InteractableNeedyPickups _myInteractable;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // for all items in players backpack, check if they correspond with a type I need
        List<ButtonPickupBackpack> buttonPickupsToRemove = new List<ButtonPickupBackpack>();

        //List<Type_Pickup> tempList = new List<Type_Pickup>();
        //for (int i = 0; i < _myInteractable.WantedPickups.Count; i++)
        //{
        //    tempList.Add(_myInteractable.WantedPickups[i]);
        //}

        for (int i = 0; i < _myInteractable.WantedPickups.Count; i++)
        {
            //Debug.Log("test 0");
            for (int j = 0; j < BackpackController.ItemButtonsInBackpack.Count; j++)
            {
                //Debug.Log("test 1");
                if (BackpackController.ItemButtonsInBackpack[j].MyPickupType == _myInteractable.WantedPickups[i])
                {
                    //Debug.Log("test 2");
                    // if this has not yet been added to my list (dont want to add the exact same pickup 3 times)
                    if (buttonPickupsToRemove.Contains(BackpackController.ItemButtonsInBackpack[j]) == false)
                    {
                        //Debug.Log("giving 1 item BAG");

                        var buttonOfInterest = BackpackController.ItemButtonsInBackpack[j];
                        buttonPickupsToRemove.Add(buttonOfInterest);

                        //_myInteractable.HeldPickups.Add(buttonOfInterest.MyPickupType);
                        _myInteractable.WantedPickups.Remove(_myInteractable.WantedPickups[i]); // should not be removing from the list it's looping in...

                        // update needy balloon
                        _myInteractable.NeedyBalloon.UpdateOneRequiredNeedyPickup(BackpackController.ItemButtonsInBackpack[j].MyPickupType);

                        BackpackController.BackpackInstance.RemoveSingularItemFromBackpack(buttonOfInterest);

                        break;
                    }
                }
            }
        }

        

        // also check for the item being held by the player
        for (int i = 0; i < _myInteractable.WantedPickups.Count; i++)
        {
            if (GameManager.Instance.Player.EquippedPickupType == _myInteractable.WantedPickups[i])
            {
                // particle poof


                // add to list
                //_myInteractable.HeldPickups.Add(GameManager.Instance.Player.EquippedPickupType); 
                _myInteractable.WantedPickups.Remove(_myInteractable.WantedPickups[i]);

                Debug.Log("giving 1 item HANDS");

                // update needy balloon
                _myInteractable.NeedyBalloon.UpdateOneRequiredNeedyPickup(GameManager.Instance.Player.EquippedPickupType);

                // delete it from your hands
                BackpackController.BackpackInstance.RemoveSingularItemFromHands();
                break;
            }
        }

        // if this made my HeldPickups.count >= WantedPickups.count
        if (_myInteractable.WantedPickups.Count <= 0) // change this to WantedPickups getting emptied -> if wanted.Count == 0
        {
            // activate new interactable that gives reward...
            _myInteractable.InteractableToActivateForReward.gameObject.SetActive(true);
            // disable my current one...
            _myInteractable.gameObject.SetActive(false);
        }
    }
}
