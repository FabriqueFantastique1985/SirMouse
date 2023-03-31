using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFocusStepsAction : MonoBehaviour
{
    [SerializeField] private StepTracker _stepTracker;
    [SerializeField] private string _tag;
    [SerializeField] private RectTransform _focusMask;
    private void Start()
    {
        var taggedObj = GameObject.FindGameObjectWithTag(_tag);
        if (taggedObj.TryGetComponent(out StepHolder stepHolder))
        {
            _stepTracker.AddStep(stepHolder.Steps);
        }
    }
}
