using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterableMoths : Encounterable
{
    [Header("Moth things")]
    [SerializeField]
    private List<Animation> _animationsMoths;
    [SerializeField]
    private List<Rigidbody> _rigidbodiesMoths;

    protected override void GenericBehaviour()
    {
        base.GenericBehaviour();

        // disable moth animations
        for (int i =0; i < _animationsMoths.Count; i++)
        {
            _animationsMoths[i].enabled = false;
        }

        // make moths fall down ded
        for (int i = 0; i < _rigidbodiesMoths.Count; i++)
        {
            _rigidbodiesMoths[i].useGravity = true;
        }
    }
}
