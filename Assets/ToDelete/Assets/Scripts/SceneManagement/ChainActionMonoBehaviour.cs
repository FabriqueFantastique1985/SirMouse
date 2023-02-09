using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChainActionMonoBehaviour : MonoBehaviour
{
    #region Events

    public delegate void ChainActionDelegate();

    public event ChainActionDelegate ChainActionDone;

    #endregion
    
    #region Enums

    public enum ChainActionType
    {
        Empty,
        Audio,
        Cutscene,
        MovePlayer,
    }

    #endregion
    protected float _maxTime = 0.0f;

    public float MaxTime => _maxTime;

    [SerializeField, HideInInspector]
    private string _nameChainAction = "DefaultChainAction";

    [HideInInspector]
    public ChainActionType ActionType;

    public virtual void Execute()
    {
    }

    public virtual void OnExit()
    {
        ChainActionDone?.Invoke();
        Debug.Log("ChainAction: " + _nameChainAction + " finished.");
    }
    
    private void OnEnable()
    {
        _nameChainAction = gameObject.name;
    }

    private void OnValidate()
    {
        _nameChainAction = gameObject.name;
    }
}
