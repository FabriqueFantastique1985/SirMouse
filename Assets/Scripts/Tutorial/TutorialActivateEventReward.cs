using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivateEventReward : TutorialActivateEvent
{


    private void Start()
    {
        RewardController.Instance.OnRewardGiven += RewardGiven;
    }

    private void OnDestroy()
    {
        RewardController.Instance.OnRewardGiven -= RewardGiven;
    }

    private void RewardGiven(SkinPieceElement reward)
    {
        ActivateTutorial();
    }

    public override void TutorialActivated()
    {
        RewardController.Instance.OnRewardGiven -= RewardGiven;
    }
}
