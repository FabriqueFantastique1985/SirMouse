using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrumentShears : InteractionInstrument
{
    [SerializeField]
    private bool CutFlowersAway;

    [SerializeField]
    private GameObject _flowersToCut;

    [SerializeField]
    private List<GameObject> _flowersToSpawn = new List<GameObject>();

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        // play animation
        //GameManager.Instance.Player.Character.AnimatorRM.SetTrigger("Swing");

        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);
        GameManager.Instance.BlockInput = true;

        StartCoroutine(SpawnObjectAndEnableInput());
    }


    public override void HideInteraction()
    {
        base.HideInteraction();

        if (_flowersToCut != null && CutFlowersAway == true)
        {
            _flowersToCut.SetActive(false);
        }
    }

    private IEnumerator SpawnObjectAndEnableInput()
    {
        //float currentStateLength = GameManager.Instance.Player.Character.AnimatorRM.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(currentStateLength);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.BlockInput = false;

        for (int i = 0; i < _flowersToSpawn.Count; i++)
        {
            _flowersToSpawn[i].SetActive(true);
        }

        HideInteraction();
    }


}
