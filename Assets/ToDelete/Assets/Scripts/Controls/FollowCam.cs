using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public bool borders = true;
    public Transform target;
    public Vector3 offset;
    [Range(0, 10)]
    public float smoothFactor;
    public Vector3 minValue, maxValue;

    public bool follow = false;

    private float resOffset;

    public GameObject PointForRaycasting;

    private void Awake()
    {
        if (target == null)
        {
            target = GameManager.Instance.Player.transform;
        }
        
        resOffset = Mathf.RoundToInt((GetComponent<Camera>().aspect - 1.3f) * 10) * 0.4f + 0.2f;
        minValue.x += resOffset;
        maxValue.x -= resOffset;

        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    private void Update()
    {
        StartCoroutine(Delay());
        if (follow)
        {
            Follow();
        }
    }

    void Follow()
    {
        // Player position
        Vector3 targetPosition = target.position + offset;
        Vector3 boundPosition;
        // Level Boundries
        if (borders)
        {
            boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
                Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
                Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z)
                );
        }
        else
        {
            boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, -1000, 1000),
                Mathf.Clamp(targetPosition.y, -1000, 1000),
                Mathf.Clamp(targetPosition.z, -1000, 1000)
                );
        }

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

    public IEnumerator ZoomOut(float zoomGoal)
    {
        float originalSize = GameManager.Instance.MainCamera.orthographicSize;

        while (GameManager.Instance.MainCamera.orthographicSize < originalSize + zoomGoal)
        {
            GameManager.Instance.MainCamera.orthographicSize += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.MainCamera.orthographicSize = originalSize + zoomGoal;
    }
    public IEnumerator ZoomInNormal()
    {
        while (GameManager.Instance.MainCamera.orthographicSize > 5)
        {
            GameManager.Instance.MainCamera.orthographicSize -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.MainCamera.orthographicSize = 5;
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
}
