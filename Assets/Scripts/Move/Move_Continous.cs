using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Continous : Move
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (!DoOnce)
        {
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
            yield return StartCoroutine(MoveToEnd());
        }
    }
}
