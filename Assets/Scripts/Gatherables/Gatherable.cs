using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
    [Header("Gatherable Component")]
    public GatherableObject GatherableSpecificComponent;

    [Header("Other Components")]
    [SerializeField]
    private Collider _trigger;

    [Header("Children Objects")]
    [SerializeField]
    private GameObject SpriteVisuals;


    protected virtual void OnTriggerEnter(Collider other)
    {
        GatherableSpecificComponent.PickedUpGatherable();

        VisualEventProc();
    }


    protected virtual void VisualEventProc()
    {

    }
}
