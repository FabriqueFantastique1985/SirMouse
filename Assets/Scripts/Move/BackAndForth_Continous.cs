using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth_Continous : Move
{
    public delegate void MoveAndReturnDelegate();
    public event MoveAndReturnDelegate OnMoveToEnd;
    public event MoveAndReturnDelegate OnReachedEnd;
    public event MoveAndReturnDelegate OnMoveToStart;
    public event MoveAndReturnDelegate OnReachedStart;

    [SerializeField] private float _waitTime;
    protected Touch_Event TouchEvent => _touchEvent;
    [SerializeField] private Touch_Event _touchEvent;

    protected virtual void Start()
    {
        _touchEvent.OnPropTouched += StartMove;
    }

    private void OnDestroy()
    {
        _touchEvent.OnPropTouched -= StartMove;
    }

    protected virtual void StartMove()
    {
        if (IsMoving)
            return;

        StartCoroutine(Move());
    }

    protected IEnumerator Move()
    {
        IsMoving = true;

        OnMoveToEnd?.Invoke();
        yield return StartCoroutine(MoveToEnd());
        OnReachedEnd?.Invoke(); 

        yield return new WaitForSeconds(_waitTime);

        OnMoveToStart?.Invoke();
        yield return StartCoroutine(MoveToStart());
        OnReachedStart?.Invoke();

        if (!DoOnce)
        {
            IsMoving = false;
        }
    }

}
