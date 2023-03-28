using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerAction : ChainActionMonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot _audioMixerSnapshots;
    [SerializeField] private float _transitionTime;
    [SerializeField] private float _delayTime = .5f;

    public override void Execute()
    {
        base.Execute();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayTime);
        _audioMixerSnapshots.TransitionTo(_transitionTime);
        yield return new WaitForSeconds(_transitionTime);
        _maxTime = -1;
    }
}