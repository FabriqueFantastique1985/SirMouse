using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PuzzlePieceScript : MonoBehaviour
{
    private Vector3 CorrectPosition;

    public bool PutInCorrectPosition;

    private Collider _collider;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Vector3 _rangeMin;
    private Vector3 _rangeMax;

    void Start()
    {
        CorrectPosition = transform.position;
        _collider = GetComponent<Collider>();

        var inBounds = FindObjectOfType<PuzzleInBounds>();
        _rangeMin = inBounds.PuzzlePieceMin;
        _rangeMax = inBounds.PuzzlePieceMax;

        RestartPuzzle(null);

        // Scramble pieces every time puzzle starts
        var _puzzleMinigame = FindObjectOfType<DragAndDrop>();
        if (_puzzleMinigame)
        {
            _puzzleMinigame.OnPuzzleRestarted += RestartPuzzle;
        }
    }

    private void RestartPuzzle(Sprite sprite)
    {
        //float zValue = transform.position.z;
        //float rotation = transform.parent.rotation.x;

        //float yvalue = zValue * Mathf.Cos(rotation);
        //transform.position = new Vector3(Random.Range(_rangeMin.x, _rangeMax.x), Random.Range(_rangeMin.y, _rangeMax.y), _rangeMax.z); ;
        
        
        
        //transform.position += new Vector3(0f, yvalue, 0f);

        transform.localPosition = new Vector3(Random.Range(-12f, -2.5f), Random.Range(-9.5f, 0.5f));
        //transform.localPosition = new Vector3(Random.Range(_rangeMin.x, _rangeMax.x), Random.Range(_rangeMin.y, _rangeMax.y), transform.position.z);
        //transform.position = _rangeMax;
        
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
