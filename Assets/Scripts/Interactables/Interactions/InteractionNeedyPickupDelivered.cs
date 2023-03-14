using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNeedyPickupDelivered : Interaction
{
    // Deliver any pickups that I want to me, remove them from the player

    [SerializeField]
    private InteractableNeedyPickups _myInteractable;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // for all items in players backpack, check if they correspond with a type I need
        List<ButtonPickupBackpack> buttonPickupsToRemove = new List<ButtonPickupBackpack>();
        for (int i = 0; i < _myInteractable.WantedPickups.Count; i++)
        {
            for (int j = 0; j < BackpackController.ItemButtonsInBackpack.Count; j++)
            {
                if (BackpackController.ItemButtonsInBackpack[j].MyPickupType == _myInteractable.WantedPickups[i])
                {
                    // if this has not yet been added to my list (dont want to add the exact same pickup 3 times)
                    if (buttonPickupsToRemove.Contains(BackpackController.ItemButtonsInBackpack[j]) == false)
                    {
                        var buttonOfInterest = BackpackController.ItemButtonsInBackpack[j];
                        buttonPickupsToRemove.Add(buttonOfInterest);
                        _myInteractable.HeldPickups.Add(buttonOfInterest.MyPickupType);

                        Debug.Log(buttonOfInterest.name + " I have been added to the list of things to remove" );

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
                _myInteractable.HeldPickups.Add(GameManager.Instance.Player.EquippedPickupType); // check if this becomes None !!!

                Debug.Log("The item in the Players hands been added to the list of things to remove");

                // delete it from your hands
                BackpackController.BackpackInstance.RemoveSingularItemFromHands();
                break;
            }
        }

        // if this made my HeldPickups.count == WantedPickups.count
        if (_myInteractable.HeldPickups.Count >= _myInteractable.WantedPickups.Count)
        {
            // activate new interactable that gives reward...
            _myInteractable.InteractableToActivateForReward.gameObject.SetActive(true);
            // disable my current one...
            _myInteractable.gameObject.SetActive(false);
        }
    }
}
