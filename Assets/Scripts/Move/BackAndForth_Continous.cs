using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth_Continous : Move
{
    public delegate void MoveAndReturnDelegate(Vector3 targetPos);
    public event MoveAndReturnDelegate OnMoveToEnd;
    public event MoveAndReturnDelegate OnMoveToStart;

    [SerializeField] private float _waitTime;
    protected Touch_Event TouchEvent => _touchEvent;
    [SerializeField] private Touch_Event _touchEvent;

    protected virtual void Start()
    {
        _touchEvent.OnPropTouched += StartMove;
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

        OnMoveToEnd?.Invoke(EndPoint.position);
        yield return StartCoroutine(MoveToEnd());

        yield return new WaitForSeconds(_waitTime);

        OnMoveToStart?.Invoke(StartPoint.position);
        yield return StartCoroutine(MoveToStart());

        if (!DoOnce)
        {
            IsMoving = false;
        }
    }

}
