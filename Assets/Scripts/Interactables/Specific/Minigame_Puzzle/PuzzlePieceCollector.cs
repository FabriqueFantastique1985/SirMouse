using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceCollector : MonoBehaviour
{
    public delegate void PuzzlePieceCollectorDelegate();
    public event PuzzlePieceCollectorDelegate OnAllPiecesPickedUp;
    public event PuzzlePieceCollectorDelegate OnPieceCollected;
    
    [SerializeField] private List<Touch_PuzzlePiece> _puzzlePieces = new List<Touch_PuzzlePiece>();

    private int _collectedPiecesCount = 0;

    [SerializeField] private float _hintTimer;

    [SerializeField] private List<Transform> _targetLocations = new List<Transform>();

    public int MaxPieces
    {
        get => _puzzlePieces.Count;
    }

    void Start()
    {
        foreach (var piece in _puzzlePieces)
        {
            piece.OnPiecePickedUp += OnPiecePickedUp;
        }
    }

    public void OnStartCollectingPieces()
    {
        if (_collectedPiecesCount == 0)
        {
            StartCoroutine(EnableParticle());
        }
    }

    private IEnumerator EnableParticle()
    {
        int collectedPieces = _collectedPiecesCount;

        yield return new WaitForSeconds(_hintTimer);

        // If collected pieces amount is the same, turn on particle
        if (collectedPieces == _collectedPiecesCount)
        {
            for (int i = 0; i < MaxPieces; i++)
            {
                if (_puzzlePieces[i])
                {
                    var touchable = _puzzlePieces[i].gameObject.GetComponent<Touchable>();
                    touchable?.ParticleGlowy.Play();
                    break;
                }
            }
        }
    }

    private void OnPiecePickedUp(Touch_PuzzlePiece piece)
    {
        OnPieceCollected?.Invoke();

        if (_collectedPiecesCount < _targetLocations.Count)
        {
            piece.TargetDestination = _targetLocations[_collectedPiecesCount].position;
        }

        ++_collectedPiecesCount;

        // Remove soon to be deleted piece from list of puzzle pieces
        int idx = _puzzlePieces.IndexOf(piece);
        if (idx >= 0 && idx < MaxPieces)
        {
            _puzzlePieces[idx] = null;
        }

        
        if (_collectedPiecesCount == MaxPieces)
        {
            OnAllPiecesPickedUp?.Invoke();
        }
        else
        {
            // Enable particle for hint if no pieces have been collected
            StartCoroutine(EnableParticle());
        }
    }
}
