using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceScript : MonoBehaviour
{
    private Vector3 CorrectPosition;

    public bool PutInCorrectPosition;

    private Collider _collider;
        
    void Start()
    {
        CorrectPosition = transform.position;
        _collider = GetComponent<Collider>();

        ScramblePieces();

        // Scramble pieces every time puzzle starts
        var _puzzleMinigame = FindObjectOfType<DragAndDrop>();
        if (_puzzleMinigame)
        {
            _puzzleMinigame.OnPuzzleStarted += ScramblePieces;
        }
    }

    private void ScramblePieces()
    {
        transform.localPosition = new Vector3(Random.Range(-12f, -2.5f), Random.Range(-9.5f, 0.5f));
        _collider.enabled = true;
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
