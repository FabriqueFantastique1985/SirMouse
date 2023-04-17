using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrumentCandle : InteractionInstrument
{
    [SerializeField]
    private GameObject _fireToActivate;

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

        StartCoroutine(GiveRewardAndEnableInput());
    }

    public override void HideInteraction()
    {
        base.HideInteraction();

        _fireToActivate.SetActive(true);
    }


    private IEnumerator GiveRewardAndEnableInput()
    {
        //float currentStateLength = GameManager.Instance.Player.Character.AnimatorRM.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(currentStateLength);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.BlockInput = false;

        // give possible rewards
        if (_rewardList != null)
        {
            RewardController.Instance.GiveReward(_rewardList.Rewards);
        }

        // show the fire
        HideInteraction();
    }
}
