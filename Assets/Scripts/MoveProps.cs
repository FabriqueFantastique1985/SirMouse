using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProps : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _speed;

    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private bool _isMoving = true;
    public bool IsMoving
    {
        get { return _isMoving; }
        set 
        { 
            _isMoving = value;
            if (value)
                StartCoroutine(Move());
        }
    }

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_isMoving)
        {
            transform.position = _startPoint.position;
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));

            while (Vector3.Distance(transform.position, _endPoint.position) > 0.001f)
            {
                // Move our position a step closer to the target.
                var step = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _endPoint.position, step);

                yield return null;
            }
        }
    }
}
