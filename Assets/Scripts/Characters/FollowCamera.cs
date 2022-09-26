using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _followTransform;

    [SerializeField]
    private float _smoothFactor = 0.125f;

    private Vector3 _offset;
    private Vector3 _currentTarget;

    private bool _hasArrived = false;
    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity,
                1 << GameManager.Instance.PlayField.gameObject.layer))
        {
            Vector3 offset = _followTransform.position - hit.point;
            transform.position += offset;
        }
        else
        {
            Debug.LogError("Camera not looking at the PlayField, so can't calibrate it so the player is in the middle.");
        }

        _offset = transform.position - _followTransform.position;
        _currentTarget = _followTransform.position + _offset;
    }

    private void FixedUpdate()
    {
        Vector3 lastPos = transform.position;
        _currentTarget = _followTransform.position + _offset;
        
        Vector3 movement = _currentTarget - lastPos;
        
        if (movement.sqrMagnitude > float.Epsilon)
        {
            transform.position =  Vector3.Lerp(lastPos, _currentTarget, _smoothFactor * Time.deltaTime);
        }
    }
}
