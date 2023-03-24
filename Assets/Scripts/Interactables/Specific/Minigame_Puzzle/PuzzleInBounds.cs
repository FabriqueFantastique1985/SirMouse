using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEditor.PlayerSettings;

public class PuzzleInBounds : MonoBehaviour
{
    [SerializeField] private Collider _playFieldCollider;
    [SerializeField] private GameObject _puzzleParent;
    [SerializeField] private Camera _puzzleCamera;
    [SerializeField] private float _distanceFromScreenEdge = 20f;

    private float _offset;

    private Vector3 _puzzlePieceMin;
    private Vector3 _puzzlePieceMax;

    public Vector3 PuzzlePieceMin
    {
        get { return _puzzlePieceMin; }
    }
    public Vector3 PuzzlePieceMax
    {
        get { return _puzzlePieceMax; }
    }

    void Awake()
    {
        _offset = _playFieldCollider.transform.position.x + _playFieldCollider.bounds.extents.x - _puzzleParent.transform.position.x;

        // Set puzzle location
        Vector3 pos = _puzzleParent.transform.position;
        var screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _puzzleCamera.transform.position.z));

        float distFromEdge = _offset + _distanceFromScreenEdge;
        _puzzleParent.transform.position = new Vector3(screenBounds.x - distFromEdge, pos.y, pos.z);


        // Set puzzlePiece location
        Ray ray = _puzzleCamera.ViewportPointToRay(new Vector2(0, 0));
        RaycastHit hitInfo;
        LayerMask mask = LayerMask.GetMask("IgnoreRaycast");
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
        {
            _puzzlePieceMin = hitInfo.point;
        }

        ray = _puzzleCamera.ViewportPointToRay(new Vector2(1, 1));
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
        {
            _puzzlePieceMax = hitInfo.point;
        }

        //screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _puzzleParent.transform.position.z));
        ////_puzzlePieceMax.x = _playFieldCollider.transform.position.x - pos.x;
        //_puzzlePieceMax.x = screenBounds.x;
        //_puzzlePieceMax.y = screenBounds.y;
    }
}
