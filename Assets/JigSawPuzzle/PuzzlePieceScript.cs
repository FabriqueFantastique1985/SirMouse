using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzlePieceScript : MonoBehaviour
{
    private Vector3 CorrectPosition;

    public bool PutInCorrectPosition;

    private Collider _collider;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private float _minBoundsX;
    private float _maxBoundsX;

    void Start()
    {
        CorrectPosition = transform.position;
        _collider = GetComponent<Collider>();

        var bounds = FindObjectOfType<PuzzleInBounds>();
        _minBoundsX = bounds.PuzzlePieceMinX;
        _minBoundsX += _collider.bounds.size.x;
        _maxBoundsX = bounds.PuzzlePieceMaxX;
        _maxBoundsX -= _collider.bounds.size.x;

        RestartPuzzle(null);

        // Scramble pieces every time puzzle starts
        var _puzzleMinigame = FindObjectOfType<DragAndDrop>();
        if (_puzzleMinigame)
        {
            _puzzleMinigame.OnPuzzleRestarted += RestartPuzzle;
        }

        GetComponent<SortingGroup>().sortingOrder = 30;
    }

    private void RestartPuzzle(Sprite sprite)
    {
        //transform.localPosition = new Vector3(Random.Range(-12f, -2.5f), Random.Range(-9.5f, 0.5f));
        transform.position = new Vector3(Random.Range(_minBoundsX, _maxBoundsX), transform.position.y);
        transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(-9.5f, 0.5f), Random.Range(-1f, 1f));
        
        _collider.enabled = true;

        if (_spriteRenderer && sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }

    public bool CheckLatchOnSpot()
    {
        if (Vector3.Distance(transform.position, CorrectPosition) < 0.5f)
        {
            transform.position = CorrectPosition;
            PutInCorrectPosition = true;

            // disable the collider of this piece
            _collider.enabled = false;

            return true;
        }

        return false;
    }
}
