using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHolder : MonoBehaviour
{
    [SerializeField] private List<Step> _steps = new List<Step>();
    public List<Step> Steps
    {
        get 
        { 
            gameObject.SetActive(false);
            return _steps; 
        }
    }
}
