using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
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
        // play sound
        AudioController.Instance.PlayAudio(GatherableSpecificComponent._clipPickup);

        // play particle


        // set object to inactive
        this.gameObject.SetActive(false);
    }
}
