using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionGetReward : Interaction
{
    [SerializeField] private List<SkinPieceElement> _mySkinPieceElements = new List<SkinPieceElement>();

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);
        RewardController.Instance.GiveReward(_mySkinPieceElements);
    }

}
