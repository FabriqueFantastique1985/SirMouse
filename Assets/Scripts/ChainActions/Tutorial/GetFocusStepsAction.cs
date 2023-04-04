using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFocusStepsAction : MonoBehaviour
{
    [SerializeField] private StepTracker _stepTracker;
    [SerializeField] private string _tag;
    private void Start()
    {
        var taggedObj = GameObject.FindGameObjectWithTag(_tag);
        StepHolder stepholder = taggedObj?.GetComponentInChildren<StepHolder>();
        if (stepholder)
        {
            _stepTracker.AddStep(stepholder.Steps);
        }
    }
}
