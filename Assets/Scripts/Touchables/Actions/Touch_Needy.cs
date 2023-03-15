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

        // fancy animation (animation of the spriteParent)


        // add to list
        _interactableOfInterest.UpdateMyList(_touchableScript);

        //this.gameObject.SetActive(false); // this breaks something .....???
    }
}
