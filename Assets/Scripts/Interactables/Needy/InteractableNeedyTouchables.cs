using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNeedyTouchables : InteractableNeedy
{
    [Header("Interactable To Activate")]
    private Interactable _interactableToActivate;

    [Header("Needing Touchables")]
    public List<TouchableNeedy> WantedTouchables;

    [HideInInspector]
    public List<TouchableNeedy> HeldTouchables;



    // called when a TouchableNeedy is clicked
    public void UpdateMyList(TouchableNeedy touchableNeedy)
    {
        HeldTouchables.Add(touchableNeedy);

        // if I have all of them... (== wantedTouchables)
        if (HeldTouchables.Count == WantedTouchables.Count)
        {
            // activate the newe interactable
        }
    }


    private void ActivateInteractable()
    {
        _interactableToActivate.gameObject.SetActive(true);

        // create this interactable as a script/prefab , give it the option to be resettable !!!
    }
}
