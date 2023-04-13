using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioElement _clipPickup;

    public virtual void PickedUpGatherable()
    {

    }
}
