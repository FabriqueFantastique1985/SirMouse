using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private bool _doOnce = false;

    [SerializeField] private float _time = 2f;

    private bool _isMoving = false;

    protected Transform StartPoint => _startPoint;
    protected Transform EndPoint => _endPoint;
    protected bool DoOnce => _doOnce;   

    protected bool IsMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }

    private void Awake()
    {
        transform.position = _startPoint.position;
    }

    protected IEnumerator MoveToEnd()
    {
        transform.position = _startPoint.position;

        while (Vector3.Distance(transform.position, _endPoint.position) > 0.001f)
        {
            // Move our position a step closer to the target.
            float speed = Vector3.Distance(_endPoint.position, _startPoint.position) / _time;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _endPoint.position, step);

            yield return null;
        }

        transform.position = _endPoint.position;
    }

    protected IEnumerator MoveToStart()
    {
        transform.position = _endPoint.position;

        while (Vector3.Distance(transform.position, _startPoint.position) > 0.001f)
        {
            // Move our position a step closer to the target.

            float speed = Vector3.Distance(_endPoint.position, _startPoint.position) / _time;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _startPoint.position, step);

            yield return null;
        }

        transform.position = _startPoint.position;
    }
}
