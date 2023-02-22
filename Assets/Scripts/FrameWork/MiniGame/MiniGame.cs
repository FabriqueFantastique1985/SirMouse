using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour, IDataPersistence
{
    #region Events

    public delegate void MiniGameDelegate(MiniGame miniGame, MiniGameArgs args);

    public event MiniGameDelegate OnMiniGameEnded;

    public struct MiniGameArgs
    {
        public bool SuccessfullyCompleted;
    }
    
    #endregion

    /// <summary>
    /// Reference to the game object that is used to quit the minigame.
    /// </summary>
    [SerializeField]
    private GameObject _exitGameObject;
    
    [SerializeField] 
    protected bool _hasBeenCompleted = false;

    [SerializeField] 
    private Step[] _steps;

    #region Fields
    
    protected int _currentStepIndex = 0;

    #endregion

    #region Properties

    protected bool IsCurrentStepIndexInRange => _currentStepIndex < _steps.Length && 0 < _steps.Length && 0 <= _currentStepIndex;

    #endregion
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (_hasBeenCompleted == false && IsCurrentStepIndexInRange)
        {
            _steps[_currentStepIndex].StepCompleted += OnStepCompleted;
            _steps[_currentStepIndex].OnEnter();
        }
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        if (IsCurrentStepIndexInRange) _steps[_currentStepIndex].StepCompleted -= OnStepCompleted;
    }
    
    private void OnStepCompleted(bool autoSave)
    {
        _steps[_currentStepIndex].StepCompleted -= OnStepCompleted;
        _currentStepIndex++;
        
        // Saves current index
        if (autoSave) DataPersistenceManager.Instance.SaveGame();
        
        if (IsCurrentStepIndexInRange == false) return;
        _steps[_currentStepIndex].StepCompleted += OnStepCompleted;
        _steps[_currentStepIndex].OnEnter();
    }

    public virtual void EndMiniGame(bool completeSuccesfully)
    {
        _exitGameObject.SetActive(false);
        var MiniGameArgs = new MiniGameArgs();
        MiniGameArgs.SuccessfullyCompleted = completeSuccesfully;
        OnMiniGameEnded?.Invoke(this, MiniGameArgs);
    }

    public void SetCurrentStep(Step newStep, bool forceEndStep)
    {
        //_currentStep.OnExit();

        if (IsCurrentStepIndexInRange)
        {
            if (forceEndStep) _steps[_currentStepIndex].ForceEnd();
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

    public virtual void StartMiniGame()
    {
        _exitGameObject.SetActive(true);
    }

    public void LoadData(GameData data)
    {
        _currentStepIndex = data.MiniGameStepIndex;
    }

    public void SaveData(ref GameData data)
    {
        data.MiniGameStepIndex = _currentStepIndex;
    }
}
