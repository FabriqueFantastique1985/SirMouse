using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MiniGame
{
    [SerializeField] DragAndDrop _puzzleMinigame;

    // called from interaction
    public override void StartMiniGame()
    {
        base.StartMiniGame();
        _puzzleMinigame.StartMiniGame();
    }
}
