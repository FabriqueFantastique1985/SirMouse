using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionStartPodium : Interaction
{
    [SerializeField]
    private PodiumController _podiumController;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        _podiumController.StartMiniGame();
    }
}
