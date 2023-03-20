using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterableButterflies : EncounterablePrerequisite
{
    [SerializeField] private GameObject _butterflyJar;

    private Character _character;

    protected override void Start()
    {
        base.Start();
        _butterflyJar.SetActive(false);

        _character = GameManager.Instance.Player.Character;
        _character.AnimationDoneEvent += EnableJar;
    }

    private void OnDestroy()
    {
        _character.AnimationDoneEvent -= EnableJar;
    }

    protected override void GenericBehaviour()
    {
        base.GenericBehaviour();
        _character.AnimatorRM.SetTrigger("Swing");
        _butterflyJar.SetActive(true);
    }

    private void EnableJar(Character.States state)
    {
    }
}
