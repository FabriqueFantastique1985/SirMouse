using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionStartPodium : Interaction
{
    public delegate void StartPodium();
    public event StartPodium OnMinigameStarted;


    [SerializeField]
    private PodiumController _podiumController;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        _podiumController.StartMiniGame();
        OnMinigameStarted?.Invoke();
    }
}
