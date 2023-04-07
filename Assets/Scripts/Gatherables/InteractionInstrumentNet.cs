using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstrumentNet : InteractionInstrument
{
    [Header("Gameobject references")]
    [SerializeField]
    private GameObject ButterflyToSpawn;

    private Character _character;
    private bool _isCatchingButterflies = false;


    private void Start()
    {
        ButterflyToSpawn.SetActive(false);

        _character = GameManager.Instance.Player.Character;
        _character.AnimationDoneEvent += EnableJar;
    }

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        GameManager.Instance.Player.Character.AnimatorRM.SetTrigger("Swing");
        _isCatchingButterflies = true;

        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);
        GameManager.Instance.BlockInput = true;
    }

    private void EnableJar(Character.States state)
    {
        if (_isCatchingButterflies)
        {
            ButterflyToSpawn.SetActive(true);
            _character.AnimationDoneEvent -= EnableJar;
            GameManager.Instance.BlockInput = false;
        }
    }
}
