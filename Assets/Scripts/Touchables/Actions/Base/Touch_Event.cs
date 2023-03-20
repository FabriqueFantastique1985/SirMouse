using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Event : Touch_Action
{
    public delegate void TouchDelegate();
    public event TouchDelegate OnPropTouched;

    public override void Act()
    {
        base.Act();
        OnPropTouched?.Invoke();
    }
}
