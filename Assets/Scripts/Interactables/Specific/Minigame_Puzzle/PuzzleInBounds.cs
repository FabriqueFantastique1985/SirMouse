using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PuzzleInBounds : MonoBehaviour
{
    [SerializeField] private Collider _playFieldCollider;
    [SerializeField] private GameObject _puzzleParent;
    [SerializeField] private Camera _puzzleCamera;
    [SerializeField] private float _distanceFromScreenEdge = 20f;

    private float _offset;

    void Start()
    {
        _offset = _playFieldCollider.transform.position.x + _playFieldCollider.bounds.extents.x - _puzzleParent.transform.position.x;
    }

    private void Update()
    {
        Vector3 pos = _puzzleParent.transform.position;
        var screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _puzzleCamera.transform.position.z));
        
        float distFromEdge = _offset + _distanceFromScreenEdge;
        _puzzleParent.transform.position = new Vector3(screenBounds.x - distFromEdge, pos.y, pos.z);
    }

}
