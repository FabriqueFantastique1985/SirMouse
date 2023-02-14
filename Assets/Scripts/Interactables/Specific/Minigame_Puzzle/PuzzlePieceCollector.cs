using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceCollector : MonoBehaviour
{
    public delegate void PuzzlePieceCollectorDelegate();
    public event PuzzlePieceCollectorDelegate OnPiecesPickedUp;
    
    [SerializeField]
    private List<Touch_PuzzlePiece> _puzzlePieces = new List<Touch_PuzzlePiece>();

    private List<Touch_PuzzlePiece> _currentPuzzlePieces = new List<Touch_PuzzlePiece>();
    
    void Start()
    {
        foreach (var piece in _puzzlePieces)
        {
            piece.OnPiecePickedUp += OnPiecePickedUp;
        }
    }

    private void OnPiecePickedUp(Touch_PuzzlePiece piece)
    {
        if(!_currentPuzzlePieces.Contains(piece))
        {
            _currentPuzzlePieces.Add(piece);
        }

        if (_currentPuzzlePieces.Count == _puzzlePieces.Count)
        {
            Debug.Log("You collected all the pieces! Yay!");
            OnPiecesPickedUp?.Invoke();
        }
    }
}
