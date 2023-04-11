using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFocusStepsAction : MonoBehaviour
{
    [SerializeField] private StepTracker _stepTracker;
    [SerializeField] private string _tag;
    private void Start()
    {
        var taggedObj = GameObject.FindGameObjectsWithTag(_tag);

        // Get list of stepholders
        List<StepHolder> stepHolders = new List<StepHolder>();
        foreach (var obj in taggedObj)
        {
            StepHolder stepholder = obj?.GetComponentInChildren<StepHolder>();
            if (stepholder)
            {
                stepHolders.Add(stepholder);
            }
        }

        // Sort list of stepholders so lowest initiative comes first
        stepHolders.Sort((sh1, sh2) => sh1.Initiative.CompareTo(sh2.Initiative));
        foreach (var stepHolder in stepHolders) 
        { 
            _stepTracker.AddStep(stepHolder.Steps);
        }
    }
}
