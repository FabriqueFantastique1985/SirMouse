using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableNeedy : Touchable
{
    [Header("Needy references")]
    [SerializeField]
    private InteractableNeedyTouchables _interactableOfInterest;



    protected override void NeedyMethod()
    {
        base.NeedyMethod();

        //_interactableOfInterest.HeldTouchables.Add();
    }
}
