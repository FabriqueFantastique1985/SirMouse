using System;
using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityCore.Audio;
using UnityEngine;

[Serializable]
public class AudioAction : ChainAction
{
    [SerializeField]
    private AudioElement _audioElement;

    public AudioElement AudioElement
    {
        get => _audioElement;
        set => _audioElement = value;
    }

    public AudioAction(AudioElement audioElement)
    {
        _audioElement = new AudioElement();
        _audioElement.Clip = audioElement.Clip;
        _audioElement.Type = audioElement.Type;
        ActionType = ChainActionType.Audio;
        _maxTime = audioElement.Clip == null ? 0.0f : _audioElement.Clip.length;
    }

    public override void Execute()
    {
        base.Execute();
        // Code to play an audio clip
        AudioController.Instance.PlayAudio(_audioElement);
    }
}
