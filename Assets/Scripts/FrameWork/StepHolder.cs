using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHolder : MonoBehaviour
{
    private List<Step> _steps = new List<Step>();
    public List<Step> Steps
    {
        get
        {
            if (_steps.Count == 0)
            {
                GetSteps();
            }
            return _steps; 
        }
    }

    private void GetSteps()
    {
        _steps.AddRange(GetComponentsInChildren<Step>());
    }
}
