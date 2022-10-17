using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _followTransform;

    [SerializeField]
    private float _smoothFactor = 0.125f;

    [SerializeField]
    private Collider2D _boundaryCollider;

    private Vector3 _offset;
    private Vector3 _currentTarget;

    private int _layerMask;

    /// <summary>
    /// The starting point for the ray checking for boundaries middle of the top and bottom of the camera's frustum
    /// </summary>
    private Vector3 _boundaryCheckerRayOriginTop;
    private Vector3 _boundaryCheckerRayOriginBottom;

    /// <summary>
    /// The starting point for the ray checking for boundaries left and right side of the camera's frustum
    /// </summary>
    private Vector3 _boundaryCheckerRayOriginLeft;
    private Vector3 _boundaryCheckerRayOriginRight;

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
            Debug.LogError(
                "Camera not looking at the PlayField, so can't calibrate it so the player is in the middle.");
        }

        if (_camera == null)
        {
            Debug.LogError("Camera reference not filled in! Using Camera Main for now");
            _camera = Camera.main;
        }

        _offset = transform.position - _followTransform.position;
        _currentTarget = _followTransform.position + _offset;

        float halfHeight = _camera.orthographicSize;
        float halfWidth = _camera.aspect * halfHeight;

        _boundaryCheckerRayOriginTop = transform.up * halfHeight;
        _boundaryCheckerRayOriginBottom = -transform.up * halfHeight;
        _boundaryCheckerRayOriginRight = transform.right * halfWidth;
        _boundaryCheckerRayOriginLeft = -transform.right * halfWidth;

        if (_boundaryCollider != null) _layerMask = 1 << _boundaryCollider.gameObject.layer;
        else Debug.LogError(" No boundary collider added! Add this to determine its layer");
    }

    private void FixedUpdate()
    {
        Vector3 lastPos = transform.position;

        var nextTarget = _followTransform.position + _offset; 
        // Calculating vertical and horizontal movement
        var futureHorizontalMovement = (nextTarget - transform.position).x;
        var futureVerticalMovement = (nextTarget - transform.position).z;
        
        // Horizontal movement check
        CheckForBoundaryHit(out bool blockHorizontalMovement, transform.position + new Vector3(futureHorizontalMovement, 0.0f, 0.0f), _boundaryCheckerRayOriginLeft);
        if (blockHorizontalMovement == false) CheckForBoundaryHit(out blockHorizontalMovement, transform.position + new Vector3(futureHorizontalMovement, 0.0f, 0.0f), _boundaryCheckerRayOriginRight);

        // Vertical movement check
        CheckForBoundaryHit(out bool blockVerticalMovement, transform.position + new Vector3(0.0f, 0.0f, futureVerticalMovement), _boundaryCheckerRayOriginTop);
        if (blockVerticalMovement == false) CheckForBoundaryHit(out blockVerticalMovement, transform.position + new Vector3(0.0f, 0.0f, futureVerticalMovement), _boundaryCheckerRayOriginBottom);

        // If we have to block a certain movement, don't add the futureMovement to the new _currentTarget.
        if (blockHorizontalMovement == false) _currentTarget.x = transform.position.x + futureHorizontalMovement;
        if (blockVerticalMovement == false) _currentTarget.z = transform.position.z + futureVerticalMovement;

        Vector3 movement = _currentTarget - lastPos;

        if (movement.sqrMagnitude > float.Epsilon)
        {
            transform.position = Vector3.Lerp(lastPos, _currentTarget, _smoothFactor * Time.deltaTime);
        }
    }

    private void CheckForBoundaryHit(out bool blockMovement, Vector3 rayOrigin, Vector3 rayOriginOffset)
    {
        Ray ray = new Ray(rayOrigin + rayOriginOffset, transform.forward);
        RaycastHit2D hit2d = Physics2D.GetRayIntersection(ray, Mathf.Infinity, _layerMask);

        blockMovement = hit2d.collider != null;

        if (blockMovement)
        {
            Debug.Log("Hitting boundary: " + hit2d.collider.name);
        }
        
        //3D Raycast
        //RaycastHit hit;
        //Ray ray = new Ray(rayOrigin + rayOriginOffset, transform.forward);
       // blockMovement = Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);
        
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);
    }
}