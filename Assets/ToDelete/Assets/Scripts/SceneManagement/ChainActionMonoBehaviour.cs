using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChainActionMonoBehaviour : MonoBehaviour
{
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
    private string _nameChainAction = "haha";

    [HideInInspector]
    public ChainActionType ActionType;

    public virtual void Execute()
    {
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
