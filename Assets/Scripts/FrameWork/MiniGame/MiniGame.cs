using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour
{
    #region Events

    public delegate void MiniGameDelegate(MiniGame miniGame, MiniGameArgs args);

    public event MiniGameDelegate OnMiniGameEnded;

    public struct MiniGameArgs
    {
        public bool SuccessfullyCompleted;
    }
    
    #endregion
    
    [SerializeField] 
    private bool _hasBeenCompleted = false;

    [SerializeField] 
    private Step[] _steps;

    private Step CurrentStep => _steps[_currentStepIndex];
    
    private int _currentStepIndex = 0;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (_hasBeenCompleted == false && CurrentStep != null)
        {
            CurrentStep.StepCompleted += OnStepCompleted;
            CurrentStep.OnEnter();
        }
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        CurrentStep.StepCompleted -= OnStepCompleted;
    }
    
    private void OnStepCompleted()
    {
        CurrentStep.StepCompleted -= OnStepCompleted;
        _currentStepIndex++;
        if (_currentStepIndex >= _steps.Length) return;
        CurrentStep.StepCompleted += OnStepCompleted;
        CurrentStep.OnEnter();
    }

    public void EndMiniGame(MiniGameArgs args)
    {
        OnMiniGameEnded?.Invoke(this, args);
    }

    public void SetCurrentStep(Step newStep)
    {
        //_currentStep.OnExit();
        CurrentStep.StepCompleted -= OnStepCompleted;
        for (int i = 0; i < _steps.Length; i++)
        {
            if (newStep == _steps[i])
            {
                _currentStepIndex = i;
                CurrentStep.OnEnter();
                return;
            }
        }
        
        Debug.LogError(newStep.name + " was not found in the list of steps of this MiniGame!");
    }

    public virtual void StartMiniGame()
    {
        
    }
}
