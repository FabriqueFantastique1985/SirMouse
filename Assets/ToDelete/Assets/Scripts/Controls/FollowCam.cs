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
    private float _cameraMaxX = float.MaxValue;
    private float _cameraMaxZ = float.MaxValue;
    private float _cameraMinX = float.MinValue;
    private float _cameraMinZ = float.MinValue;

    public GameObject PointForRaycasting;

    private MeshCollider _cameraBounds;
    public bool _isBorderActive = true;
    private bool _isOutsideBorder = false;

    private void Awake()
    {
        if (target == null)
        {
            target = GameManager.Instance.Player.transform;
        }

        // resOffset = Mathf.RoundToInt((GetComponent<Camera>().aspect - 1.3f) * 10) * 0.4f + 0.2f;
        // minValue.x += resOffset;
        // maxValue.x -= resOffset;

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
            if (_cameraBounds && _isBorderActive && !_isOutsideBorder)
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
        else
            _isOutsideBorder = true;

        // Raycast allong Z
        if (GetBounds(out hitPoint, Vector3.forward))
            _cameraMaxZ = hitPoint.z - cameraHalfHeight;
        else
            _isOutsideBorder = true;

        if (GetBounds(out hitPoint, Vector3.back))
            _cameraMinZ = hitPoint.z + cameraHalfHeight;
        else
            _isOutsideBorder = true;

        // Reset querry settings
        Physics.queriesHitBackfaces = originalQuries;
    }

    private bool GetBounds(out Vector3 hitPoint, Vector3 direction)
    {
        RaycastHit hitInfo;
        hitPoint = Vector3.zero;
        Vector3 rayStartPosition = transform.position + direction / 100f;
        if (_cameraBounds.Raycast(new Ray(rayStartPosition, direction), out hitInfo, Mathf.Infinity))
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
        // Player position
        Vector3 targetPosition = target.position + offset;

        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothPosition;

        _isOutsideBorder = false;
    }

    public IEnumerator ZoomOut(float zoomGoal)
    {
        float originalSize = GameManager.Instance.CurrentCamera.orthographicSize;

        while (GameManager.Instance.CurrentCamera.orthographicSize < originalSize + zoomGoal)
        {
            GameManager.Instance.CurrentCamera.orthographicSize += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.CurrentCamera.orthographicSize = originalSize + zoomGoal;
    }
    public IEnumerator ZoomInNormal()
    {
        while (GameManager.Instance.CurrentCamera.orthographicSize > 5)
        {
            GameManager.Instance.CurrentCamera.orthographicSize -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.CurrentCamera.orthographicSize = 5;
    }

    IEnumerator Delay()
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

        // When setting bounds, reset camera max and min
        _cameraMaxX = _cameraMaxZ = float.MaxValue;
        _cameraMinX = _cameraMinZ = float.MinValue;

        gameObject.transform.position = target.transform.position + offset;
    }
}
