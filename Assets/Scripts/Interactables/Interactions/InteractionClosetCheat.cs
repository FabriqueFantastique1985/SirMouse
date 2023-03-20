using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionClosetCheat : Interaction
{
    [Header("Skin Things")]
    public GameObject SpriteParent;


    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        if (SpriteParent != null)
        {
            ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SpriteParent));
        }

        RewardController.Instance.GiveCheatReward();
    }
}
