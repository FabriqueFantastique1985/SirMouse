using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [HideInInspector]
    public ChainActionType ActionType;

    public virtual void Execute()
    {
    }
}
