using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSolvingPuzzle : Interaction
{
    public PuzzleController PuzzleController;

    [SerializeField]
    private PuzzlePieceCollector _pieceCollector;

    private void Start()
    {
        _pieceCollector.OnAllPiecesPickedUp += OnPiecesPickedUp;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OnPiecesPickedUp()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();
    }

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        PuzzleController.StartMiniGame();
    }
}
