using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterableButterflies : EncounterablePrerequisite
{
    [SerializeField] private GameObject _butterflyJar;
    [SerializeField] private AnimationClip _playerAnimationClip;

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
    }

    private void EnableJar(Character.States state)
    {
        _butterflyJar.SetActive(true);
    }
}
