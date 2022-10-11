using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInteractable : Interactable
{
    protected override void OnInteractBalloonClicked(InteractBalloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);
        Destroy(gameObject);
    }
}
