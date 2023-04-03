using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerAction : ChainActionMonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot _audioMixerSnapshots;
    [SerializeField] private float _transitionTime;

    public override void Execute()
    {
        base.Execute();
        _audioMixerSnapshots.TransitionTo(_transitionTime);
    }
}