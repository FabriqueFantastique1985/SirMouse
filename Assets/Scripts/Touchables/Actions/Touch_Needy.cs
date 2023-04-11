using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Needy : Touch_Action
{
    [Header("Needy references")]
    [SerializeField]
    private InteractableNeedyTouchables _interactableOfInterest;

    [Header("Sprite Parent")]
    public GameObject MySpriteParent;



    public override void Act()
    {
        base.Act();

        // fancy animation (animation of the spriteParent)


        // add to list
        _interactableOfInterest.UpdateMyList(_touchableScript);

        // particle
        if (_interactableOfInterest.ParticlePoofTapped != null)
        {
            Instantiate(_interactableOfInterest.ParticlePoofTapped, this.transform.position, Quaternion.identity);
        }

        // makee invisible
        MySpriteParent.SetActive(false);
        _touchableScript.Collider.enabled = false;
    }
}
