using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FocusInstrumentUnlockAction : FocusAction
{
    [SerializeField] private List<Type_Instrument> _arrowLeft;
    [SerializeField] private List<Type_Instrument> _arrowRight;

    [SerializeField] private PlayableAsset _timelineArrowLeft;
    [SerializeField] private PlayableAsset _timelineArrowRight;

    protected override void Start()
    {
        base.Start();

        InstrumentController.Instance.OnInstrumentUnlocked += SetFocus;
    }

    private void SetFocus(SlotInstrument slot)
    {
        if (_arrowLeft.Contains(slot.InstrumentType))
        {
            Timeline.playableAsset = _timelineArrowLeft;
        }
        else if (_arrowRight.Contains(slot.InstrumentType))
        {
            Timeline.playableAsset = _timelineArrowRight;
        }

        InstrumentController.Instance.OnInstrumentUnlocked -= SetFocus;
        Focus = slot.transform;
    }
}
