using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusInstrumentUnlockAction : FocusAction
{
    protected override void Start()
    {
        base.Start();

        InstrumentController.Instance.OnInstrumentUnlocked += SetFocus;
    }

    private void SetFocus(SlotInstrument slot)
    {
        InstrumentController.Instance.OnInstrumentUnlocked -= SetFocus;
        Focus = slot.transform;
    }
}
