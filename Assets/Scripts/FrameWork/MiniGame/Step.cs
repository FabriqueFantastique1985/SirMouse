using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Step : MonoBehaviour
{
    #region Events

    public delegate void StepDelegate();

    public event StepDelegate StepCompleted;

    #endregion
    
    [SerializeField]
    private ChainActionMonoBehaviour[] _onEnterChainActions;

    public void OnEnter()
    {
        _onEnterChainActions.Last().ChainActionDone += OnLastChainActionDone;
        for (var i = 0; i < _onEnterChainActions.Length; i++)
        {
            GameManager.Instance.ChainMono.AddAction(_onEnterChainActions[i], true);
        }
    }

    private void OnLastChainActionDone()
    {
        StepCompleted?.Invoke();
        Debug.Log($"Step: {gameObject.name} completed.");
    }
}
