using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Touch_Reward : Touch_Action
{
    [Header("Reward")]
    [SerializeField] private List<SkinPieceElement> _skinsToReward;

    private bool _hasCollected = false;

    public override void Act()
    {
        if (_hasCollected)
        {
            return;
        }

        if (_skinsToReward.Count > 0)
        {
            RewardController.Instance.GiveReward(_skinsToReward);
            _hasCollected = true;
        }
    }
}
