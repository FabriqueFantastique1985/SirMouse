using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MiniGame
{
    [SerializeField] DragAndDrop _puzzleMinigame;

    private void Start()
    {
        _puzzleMinigame.OnPuzzleCompleted += OnPuzzleCompleted;
    }

    private void OnPuzzleCompleted()
    {
        base.EndMiniGame(true);
    }

    // called from interaction
    public override void StartMiniGame()
    {
        base.StartMiniGame();
        _puzzleMinigame.StartMiniGame();
    }
}
