using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Needy : Touch_Action
{
    [Header("Needy references")]
    [SerializeField]
    private InteractableNeedyTouchables _interactableOfInterest;


    public override void Act()
    {
        base.Act();

        // pop/float up (animation of the spriteParent)


        // code movement towards _interactableOfInterest (transform of the TouchableNeedy)


        // after it arrives, disable the object, add to list
        _interactableOfInterest.UpdateMyList(_touchableScript);

        Debug.Log("clicked needy");

        //this.gameObject.SetActive(false);
    }
}
