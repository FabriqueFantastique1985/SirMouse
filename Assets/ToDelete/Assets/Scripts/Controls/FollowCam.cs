using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(0, 10)]
    public float smoothFactor;

    public bool follow = false;

    public GameObject PointForRaycasting;

    // Camera bounds
    private MeshCollider _cameraBounds;
    public bool IsBorderActive = true;
    private bool _isOutsideBorder = false;

    private float _cameraMaxX = float.MaxValue;
    private float _cameraMaxZ = float.MaxValue;
    private float _cameraMinX = float.MinValue;
    private float _cameraMinZ = float.MinValue;

    private void Awake()
    {
        if (target == null)
        {
            target = GameManager.Instance.Player.transform;
        }

        if (offset == Vector3.zero)
        {
            offset = gameObject.transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
        StartCoroutine(Delay());
        if (follow)
        {
            if (_cameraBounds && IsBorderActive && !_isOutsideBorder)
            {
                FollowTargetInBounds();
            }
            else
            {
                FollowTarget();
            }
        }
    }

    private void SetCameraBounds()
    {
        // Change querry settings
        bool originalQuries = Physics.queriesHitBackfaces;
        Physics.queriesHitBackfaces = true;

        // Get camera measurements
        float cameraHalfHeight = 2f * Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect / 2f;

        // Raycast allong X
        Vector3 hitPoint;
        if (GetBounds(out hitPoint, Vector3.right))
            _cameraMaxX = hitPoint.x - cameraHalfWidth;
        else
            _isOutsideBorder = true;

        if (GetBounds(out hitPoint, Vector3.left))
            _cameraMinX = hitPoint.x + cameraHalfWidth;

        // Raycast allong Z
        if (GetBounds(out hitPoint, Vector3.forward))
            _cameraMaxZ = hitPoint.z - cameraHalfHeight;

        if (GetBounds(out hitPoint, Vector3.back))
            _cameraMinZ = hitPoint.z + cameraHalfHeight;

        // Reset querry settings
        Physics.queriesHitBackfaces = originalQuries;
    }

    /// <summary>
    /// Checks the bounds of _cameraBounds following direction and returns if it hit any edges and what point it hit.
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool GetBounds(out Vector3 hitPoint, Vector3 direction)
    {
        hitPoint = Vector3.zero;
        RaycastHit hitInfo;
        if (_cameraBounds.Raycast(new Ray(transform.position, direction), out hitInfo, Mathf.Infinity))
        {
            hitPoint = hitInfo.point;
            return true;
        }
        return false;
    }

    private void FollowTargetInBounds()
    {
        // Player position
        Vector3 targetPosition = target.position + offset;

        SetCameraBounds();

        Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, _cameraMinX, _cameraMaxX),
                targetPosition.y,
                Mathf.Clamp(targetPosition.z, _cameraMinZ, _cameraMaxZ)
                );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothPosition;
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothPosition;

        _isOutsideBorder = false;
    }

    private IEnumerator Delay()
    {
        float seconds = 1f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        follow = true;
    }

    /// <summary>
    /// Sets the bounds of the follow camera with values of a collider
    /// </summary>
    /// <param name="collider"></param>
    public void SetBounds(MeshCollider collider)
    {
        _cameraBounds = collider;

        // Reset camera max and min
        _cameraMaxX = _cameraMaxZ = float.MaxValue;
        _cameraMinX = _cameraMinZ = float.MinValue;
    }
}
