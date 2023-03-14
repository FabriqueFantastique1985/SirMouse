using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterableFire : EncounterablePrerequisite
{
    [SerializeField] private GameObject _fire;

    protected override void Start()
    {
        base.Start();

        _fire?.SetActive(false);
    }

    protected override void GenericBehaviour()
    {
        base.GenericBehaviour();

        _fire?.SetActive(true);
    }
}
