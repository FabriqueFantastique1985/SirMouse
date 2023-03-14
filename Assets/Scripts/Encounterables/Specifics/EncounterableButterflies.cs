using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterableButterflies : EncounterableMoths
{
    [SerializeField] private GameObject _interactableButterfly;
    [SerializeField] private Rigidbody _rigidbody;

    protected override void Start()
    {
        base.Start();
        _interactableButterfly.SetActive(false);
    }

    protected override void GenericBehaviour()
    {
        base.GenericBehaviour();

        StartCoroutine(Pickup());
    }

    private IEnumerator Pickup()
    {
        yield return new WaitForSeconds(1f);
        while(_rigidbody.velocity.y > 0.01f)
        {
            yield return null;
        }

        _interactableButterfly.SetActive(true);
        gameObject.SetActive(false);
    }
}
