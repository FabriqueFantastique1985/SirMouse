using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Assertions;

public class PuzzlePieceCounter : MonoBehaviour
{
    [SerializeField]
    private PuzzlePieceCollector _puzzlePieceCollector;

    [SerializeField]
    private List<SpriteRenderer> _puzzlePieceSprites = new List<SpriteRenderer>();

    [SerializeField]
    private Interactable _interactableBalloon;

    int _collectedPieces = 0;

    private void Start()
    {
        _puzzlePieceCollector.OnPieceCollected += OnPieceCollected;

        _interactableBalloon.OnInteracted += DisableBubble;

        Assert.AreEqual(_puzzlePieceSprites.Count, _puzzlePieceCollector.MaxPieces, "Amount of pieces in bubble not the same as amount of pieces to collect");
    }

    private void OnPieceCollected()
    {
        _puzzlePieceSprites[_collectedPieces].color = Color.white;
        ++_collectedPieces;
    }

    private void DisableBubble()
    {
        gameObject.SetActive(false);
    }
}
