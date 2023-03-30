using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepTracker : MonoBehaviour
{
    public delegate void StepTrackerDelegate();
    public event StepTrackerDelegate OnStepsCompleted;

    [SerializeField] private Step[] _steps;
    private bool _hasBeenCompleted = false;
    private int _currentStepIndex = 0;

    protected bool IsCurrentStepIndexInRange => _currentStepIndex < _steps.Length && 0 < _steps.Length && 0 <= _currentStepIndex;

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        // Delay step calls by a frame to give them a chance to initialize
        yield return null;
        if (_hasBeenCompleted == false && IsCurrentStepIndexInRange)
        {
            _steps[_currentStepIndex].StepCompleted += OnStepCompleted;
            _steps[_currentStepIndex].OnEnter();
        }
    }

    private void OnDisable()
    {
        if (IsCurrentStepIndexInRange) 
            _steps[_currentStepIndex].StepCompleted -= OnStepCompleted;
    }

    private void OnStepCompleted(bool autoSave)
    {
        _steps[_currentStepIndex].StepCompleted -= OnStepCompleted;
        _currentStepIndex++;

        if (IsCurrentStepIndexInRange == false)
        {
            OnStepsCompleted?.Invoke();
            return;
        }
        _steps[_currentStepIndex].StepCompleted += OnStepCompleted;
        _steps[_currentStepIndex].OnEnter();
    }

    public void SetCurrentStep(Step newStep, bool forceEndStep)
    {
        if (IsCurrentStepIndexInRange)
        {
            if (forceEndStep) 
                _steps[_currentStepIndex].ForceEnd();
            
            _steps[_currentStepIndex].StepCompleted -= OnStepCompleted;
        }

        for (int i = 0; i < _steps.Length; i++)
        {
            if (newStep == _steps[i])
            {
                _currentStepIndex = i;
                _steps[_currentStepIndex].StepCompleted += OnStepCompleted;
                _steps[_currentStepIndex].OnEnter();
                return;
            }
        }

        Debug.LogError(newStep.name + " was not found in the list of steps of this MiniGame!");
    }
}
