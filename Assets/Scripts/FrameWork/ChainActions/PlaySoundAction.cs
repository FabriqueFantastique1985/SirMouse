using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAction : ChainActionMonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;

    public override void Execute()
    {
        base.Execute();
        _audioSource.Play();
    }
}