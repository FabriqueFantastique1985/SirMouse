using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth_Event : Move
{
    public delegate void MoveAndReturnDelegate();
    public event MoveAndReturnDelegate OnMoveComplete;
    
    [SerializeField] private BackAndForth_Continous _moveEvent;

    private bool _hasMovedToStart = false;
    private bool _hasMovedToEnd = false;
    private bool _hasEventBeenSend = false;

    private void Start()
    {
        _moveEvent.OnMoveToStart += MoveToEnd;
        _moveEvent.OnMoveToEnd += MoveToStart;
    }

    private void OnDestroy()
    {
        _moveEvent.OnMoveToStart -= MoveToEnd;
        _moveEvent.OnMoveToEnd -= MoveToStart;
    }

    private void Update()
    {
        if (DoOnce && _hasMovedToEnd && _hasMovedToStart && !_hasEventBeenSend)
        {
            OnMoveComplete?.Invoke();
            _hasEventBeenSend = true;
        }
    }

    public void Reset()
    {
       _hasMovedToStart = false;
       _hasMovedToEnd = false;
       _hasEventBeenSend = false;
    }

    private new void MoveToEnd()
    {
        if (DoOnce && _hasMovedToEnd)
            return;

        if (Equals(transform.position, StartPoint.position))
        {
            StartCoroutine(MoveComplete(base.MoveToEnd(), result => _hasMovedToEnd = result));
        }
    }

    private new void MoveToStart()
    {
        if (DoOnce && _hasMovedToStart)
            return;
        
        if (Equals(transform.position, EndPoint.position))
        {
            StartCoroutine(MoveComplete(base.MoveToStart(), result => _hasMovedToStart = result));
        }
    }

    private IEnumerator MoveComplete(IEnumerator coroutine, Action<bool> isCompleted)
    {
        yield return StartCoroutine(coroutine);
        isCompleted(true);
    }
}
