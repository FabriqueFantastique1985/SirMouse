using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrumentKey : InteractionInstrument
{
    [Header("Chest Object")]
    [SerializeField]
    private GameObject _chestToOpen;
    [SerializeField]
    private GameObject _particlePoofPrefab;

    [Header("Possible Reward List")]
    [SerializeField]
    private RewardList _rewardList;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // play animation
        //GameManager.Instance.Player.Character.AnimatorRM.SetTrigger("Swing");

        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);
        GameManager.Instance.BlockInput = true;
        IsCompleted = true;

        StartCoroutine(GiveRewardAndEnableInput());
    }

    public override void HideInteraction()
    {
        base.HideInteraction();

        Instantiate(_particlePoofPrefab, _chestToOpen.transform.position, Quaternion.identity);
        _chestToOpen.SetActive(false);
    }



    private IEnumerator GiveRewardAndEnableInput()
    {
        //float currentStateLength = GameManager.Instance.Player.Character.AnimatorRM.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(currentStateLength);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.BlockInput = false;

        // give rewards
        if (_rewardList != null)
        {
           RewardController.Instance.GiveReward(_rewardList.Rewards);
        }   

        // hide the chest
        HideInteraction();
    }
}
