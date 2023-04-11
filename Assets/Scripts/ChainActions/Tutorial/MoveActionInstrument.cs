using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionInstrument : MoveAction
{
    private bool _hasBeenCollected = false;

    private void OnEnable()
    {
        InstrumentController.Instance.OnInstrumentUnlocked += InstrumentCollected;
    }

    private void InstrumentCollected(SlotInstrument instrument)
    {
        _hasBeenCollected = true;
    }

    private void OnDisable()
    {
        InstrumentController.Instance.OnInstrumentUnlocked -= InstrumentCollected;
    }

    protected override bool HasCompletedObjective()
    {
        return _hasBeenCollected;
    }
}
