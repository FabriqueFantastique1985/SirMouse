using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionInstrumentNet : InteractionInstrument
{
    [Header("Gameobject references")]
    [SerializeField]
    private List<GameObject> _butterfliesToSpawn = new List<GameObject>();

    private Character _character;
    private bool _isCatchingButterflies = false;

    private int _gatheredObjectCounter = 0;

    private void Start()
    {
        for (int i = 0; i < _butterfliesToSpawn.Count; i++)
        {
            _butterfliesToSpawn[i].SetActive(false);
            if (_butterfliesToSpawn[i].TryGetComponent(out GatherableObject gatherable))
            {
                gatherable.ObjectGathered += GatheredObject;
            }
        }

        _character = GameManager.Instance.Player.Character;
        _character.AnimationDoneEvent += EnableJar;
    }

    private void GatheredObject(GatherableObject thisGatherable)
    {
        thisGatherable.ObjectGathered -= GatheredObject;

        ++_gatheredObjectCounter;
        if (_gatheredObjectCounter == _butterfliesToSpawn.Count)
        {
            IsCompleted = true;
        }
    }

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        GameManager.Instance.Player.Character.AnimatorRM.SetTrigger("Swing");
        _isCatchingButterflies = true;

        GameManager.Instance.Player.Agent.SetDestination(GameManager.Instance.Player.gameObject.transform.position);
        GameManager.Instance.BlockInput = true;

        StartCoroutine(DisableBlockInput());
    }

    public override void HideInteraction()
    {
        base.HideInteraction();

        // maybe hide butterflies ?
    }

    private IEnumerator DisableBlockInput()
    {
        float curentStateLength = GameManager.Instance.Player.Character.AnimatorRM.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(curentStateLength);

        EnableJar(Character.States.Idle);
    }

    private void EnableJar(Character.States state)
    {
        if (_isCatchingButterflies)
        {
            for (int i = 0; i < _butterfliesToSpawn.Count; i++)
            {
                _butterfliesToSpawn[i].SetActive(true);
            }

            _character.AnimationDoneEvent -= EnableJar;
            GameManager.Instance.BlockInput = false;
        }
    }
}
