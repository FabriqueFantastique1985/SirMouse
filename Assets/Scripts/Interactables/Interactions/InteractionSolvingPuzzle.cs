using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSolvingPuzzle : Interaction
{
    public PuzzleController PuzzleController;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        PuzzleController.StartMiniGame();
    }
}
