using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInBounds : MonoBehaviour
{
    [SerializeField] private Collider _playFieldCollider;
    [SerializeField] private GameObject _puzzleParent;
    [SerializeField] private Camera _puzzleCamera;
    [SerializeField] private float _distanceFromScreenEdge = .5f;

    private float _puzzlePieceMinX;

    public float PuzzlePieceMinX
    {
        get { return _puzzlePieceMinX; }
    }

    private float _puzzlePieceMaxX;

    public float PuzzlePieceMaxX
    {
        get { return _puzzlePieceMaxX; }
    }



    private void Awake()
    {
        SetPuzzlePosition();
        SetPuzzlePieceBoundaries(); 
    }

    private void SetPuzzlePosition()
    {
        // Position puzzle at the left edge of the screen
        Vector3 pos = _puzzleParent.transform.position;
        Vector3 screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, _puzzleCamera.transform.position.z));

        // Calculate the offset of the puzzle compared to left edge of puzzle
        float offset = _playFieldCollider.transform.position.x + _playFieldCollider.bounds.extents.x - pos.x;

        // Calculate total distance from left edge of screen
        float distFromEdge = offset + _distanceFromScreenEdge;
       _puzzleParent.transform.position = new Vector3(screenBounds.x - distFromEdge, pos.y, pos.z);
    }

    private void SetPuzzlePieceBoundaries()
    {
        // Setting min and max X based of screen bounds
        Vector3 pos = _puzzleParent.transform.position;
        Vector3 screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, pos.z));

        // Calculate offset, taking into account he puzzle extents
        float offsetRight = 2f * _playFieldCollider.bounds.extents.x;

        // Calculate puzzle Max X
        _puzzlePieceMaxX = screenBounds.x - offsetRight;
        _puzzlePieceMaxX -= _distanceFromScreenEdge;

        // Calculate puzzle Min X
        screenBounds = _puzzleCamera.ScreenToWorldPoint(new Vector3(0f, 0f, pos.z));
        _puzzlePieceMinX = screenBounds.x ;
        _puzzlePieceMinX += _distanceFromScreenEdge;
    }
}
