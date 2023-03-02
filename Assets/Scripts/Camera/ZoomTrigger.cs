using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour
{
    [SerializeField]
    private float _zoomAmount;

    [SerializeField]
    [Tooltip ("0 == default speed")]
    private float _zoomSpeed;

    private int _amountOverlaps = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        ++_amountOverlaps;
        if (!other.gameObject.CompareTag("Player") && _amountOverlaps > 1)
        {
            return;
        }

        GameManager.Instance.ZoomCamera.IncreaseDefaultZoom(_zoomAmount, _zoomSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        --_amountOverlaps;
        if (!other.gameObject.CompareTag("Player") && _amountOverlaps > 0)
        {
            return;
        }

        GameManager.Instance.ZoomCamera.ResetDefaultZoom(_zoomSpeed);
    }
}
