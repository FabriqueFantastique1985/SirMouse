using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableInstrument : GatherableObject
{
    public Type_Instrument InstrumentType;

    public override void PickedUpGatherable()
    {
        base.PickedUpGatherable();

        InstrumentController.Instance.UnlockInstrument(InstrumentType);
        UIFlyingToBackpackController.Instance.ThrowItemIntoBackpack(this, Type_Resource.None, InstrumentType);
    }
}
