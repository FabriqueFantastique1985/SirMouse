using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour
{
    [SerializeField]
    private float _newOrthographicSize;

    [SerializeField]
    [Tooltip ("0 == default speed")]
    private float _zoomSpeed;

    private void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.CompareTag("Player"))
        //{
        //    return;
        //}

        GameManager.Instance.ZoomCamera.IncreaseDefaultZoom(_newOrthographicSize, _zoomSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        //if (!other.gameObject.CompareTag("Player"))
        //{
        //    return;
        //}

        GameManager.Instance.ZoomCamera.ResetDefaultZoom(_zoomSpeed);
    }
}
