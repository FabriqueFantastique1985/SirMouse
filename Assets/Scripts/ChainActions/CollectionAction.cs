using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private PuzzlePieceCollector _puzzlePieceCollector;

    private void Start()
    {
        _puzzlePieceCollector.OnPiecesPickedUp += OnPiecesPickedUp;
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _puzzlePieceCollector.OnStartCollectingPieces();
    }

    private void OnPiecesPickedUp()
    {
        _maxTime = -1.0f;
    }
}
