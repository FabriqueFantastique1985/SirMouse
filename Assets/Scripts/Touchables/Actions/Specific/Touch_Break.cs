using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Break : Touch_Action
{
    [SerializeField] private float _rewardWaitTime;
    [SerializeField] private float _deleteWaitTime;
    [SerializeField] private ParticleSystem _particle;
    
    [Header("Reward")]
    [SerializeField] private List<SkinPieceElement> _skinsToReward;

    public override void Act()
    {
        base.Act();

        _particle?.Play();

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_rewardWaitTime);
        if (_skinsToReward.Count > 0)
        {
            RewardController.Instance.GiveReward(_skinsToReward);
        }
        yield return new WaitForSeconds(_deleteWaitTime);
        Destroy(gameObject);
    }
}
