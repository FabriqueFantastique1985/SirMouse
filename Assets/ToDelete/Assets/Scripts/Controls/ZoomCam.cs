using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZoomCam : MonoBehaviour
{
    [Header("Steps")]
    [SerializeField]
    private float _zoomInSteps;
    [SerializeField]
    private float _zoomOutSteps;

    [Header("Target")]
    [SerializeField]
    private NavMeshAgent _playerAgent;

    [Header("Amounts")]
    [SerializeField]
    private float _zoomMaxAmout;
    [SerializeField]
    private float _zoomMinAmout;
    [SerializeField]
    private float _zoomAmoutOnMove;

    private float _initialOrthographicSize;
    private float _orthographicSizeOffset;
    private bool _isMoving = false;

    private Coroutine _zoomInRoutine;
    private Coroutine _zoomOutRoutine;
    
    public float ZoomAmountOnMove
    {
        get { return _zoomAmoutOnMove; }
        set { _zoomAmoutOnMove = value;}
    }

    private void Awake()
    {
        _initialOrthographicSize = GameManager.Instance.CurrentCamera.orthographicSize;
    }

    // Inside player instead of camera?
    private void Update()
    {
        if (!_playerAgent)
            return;

        if (_playerAgent.velocity.sqrMagnitude > 0.1f && !_isMoving)
        {
            Zoom(_zoomAmoutOnMove);
            _isMoving = true;
        }
        if (_playerAgent.velocity.sqrMagnitude < 0.1f && _isMoving)
        {
            _isMoving = false;
            ResetZoom();
        }
    }

    /// <summary>
    /// Zooms according to zoom amount.
    /// Positive amount will zoom out.
    /// Negative amount will zoom in.
    /// </summary>
    /// <param name="zoomAmount"></param>
    public void Zoom(float zoomAmount, float zoomSpeed = 0f)
    {
        if (_zoomInRoutine != null)
            StopCoroutine(_zoomInRoutine);
        if (_zoomOutRoutine != null)
            StopCoroutine(_zoomOutRoutine);
        
        if (zoomAmount > 0)
        {
            if (zoomSpeed == 0)
                zoomSpeed = _zoomOutSteps;

            _zoomOutRoutine = StartCoroutine(ZoomOut(zoomAmount, zoomSpeed));
        }
        else
        {
            if (zoomSpeed == 0)
                zoomSpeed = _zoomInSteps;

            StopCoroutine("ZoomOut");
            _zoomInRoutine = StartCoroutine(ZoomIn(zoomAmount, zoomSpeed));
        }
    }

    /// <summary>
    /// Resets the amount previously zoomed in or out to their default value
    /// </summary>
    public void ResetZoom(float zoomSpeed = 0f)
    {
        Zoom(_initialOrthographicSize + _orthographicSizeOffset - GameManager.Instance.CurrentCamera.orthographicSize, zoomSpeed);
    }

    public void IncreaseDefaultZoom(float newOrthographicSize, float zoomSpeed = 0f)
    {
        _orthographicSizeOffset = newOrthographicSize - _initialOrthographicSize;
        Zoom(_orthographicSizeOffset, zoomSpeed);
    }

    public void ResetDefaultZoom(float zoomSpeed = 0f)
    {
        _orthographicSizeOffset = 0;
        Zoom(_orthographicSizeOffset, zoomSpeed);
    }
    private IEnumerator ZoomOut(float zoomAmount, float zoomSpeed)
    {
        float originalSize = GameManager.Instance.CurrentCamera.orthographicSize;
        float zoomGoal = Mathf.Clamp(originalSize + zoomAmount, _zoomMinAmout + _orthographicSizeOffset, _zoomMaxAmout + _orthographicSizeOffset);
        while (GameManager.Instance.CurrentCamera.orthographicSize < zoomGoal)
        {
            GameManager.Instance.CurrentCamera.orthographicSize += zoomSpeed;
            yield return null;
        }

        GameManager.Instance.CurrentCamera.orthographicSize = zoomGoal;
    }


    private IEnumerator ZoomIn(float zoomAmount, float zoomSpeed)
    {
        float originalSize = GameManager.Instance.CurrentCamera.orthographicSize;
        float zoomGoal = Mathf.Clamp(originalSize + zoomAmount, _zoomMinAmout + _orthographicSizeOffset, _zoomMaxAmout + _orthographicSizeOffset);
        while (GameManager.Instance.CurrentCamera.orthographicSize > zoomGoal)
        {
            GameManager.Instance.CurrentCamera.orthographicSize -= zoomSpeed;
            yield return null;
        }

        GameManager.Instance.CurrentCamera.orthographicSize = zoomGoal;
    }
}
