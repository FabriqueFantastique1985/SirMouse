using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Step : MonoBehaviour
{
    #region Events

    public delegate void StepDelegate(bool autoSave);

    public event StepDelegate StepCompleted;

    #endregion
    
    [SerializeField]
    private ChainActionMonoBehaviour[] _onEnterChainActions;

    /// <summary>
    /// When true, an auto save will occur.
    /// </summary>
    [SerializeField, Tooltip("When true, an autoSave will occur.")]
    private bool _autoSave = false;
    
    private ChainMono _chain;

    private void Awake()
    {
        _chain = new ChainMono(true);
    }

    public void OnEnter()
    {
        _onEnterChainActions.Last().ChainActionDone += OnLastChainActionDone;
        for (var i = 0; i < _onEnterChainActions.Length; i++)
        {
            _chain.AddAction(_onEnterChainActions[i], true);
        }
    }

    private void OnLastChainActionDone()
    {
        StepCompleted?.Invoke(_autoSave);
        Debug.Log($"Step: {gameObject.name} completed.");
    }

    private void Update()
    {
        _chain.UpdateChain(Time.deltaTime);
    }

    public void ForceEnd()
    {
        Debug.Log($"Step: {gameObject.name} force ended.");
        _chain.ClearChain();
    }
}
