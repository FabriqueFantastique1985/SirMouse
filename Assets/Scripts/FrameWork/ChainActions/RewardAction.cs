using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private List<SkinPieceElement> _skinsToReward;

    public override void Execute()
    {
        base.Execute();
        RewardController.Instance.GiveReward(_skinsToReward);
        _maxTime = -1f;
    }
}
