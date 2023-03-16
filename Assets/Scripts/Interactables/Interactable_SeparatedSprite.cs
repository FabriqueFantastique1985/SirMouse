using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_SeparatedSprite : Interactable
{
    [Header("Separated Sprite")] [SerializeField] private Transform _separatedSprite;

    protected override void OnInteractBalloonClicked(Balloon sender, Player player)
    {
        base.OnInteractBalloonClicked(sender, player);

        Destroy(_separatedSprite.gameObject);
    }
}