using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private PlayableDirector _cutscenePlayableDirector;

    [SerializeField, Tooltip("When true, input will be blocked while the cutscene is playing and will " +
                             "be renenabled when the cutscene is done playing.")]
    private bool _disableInputWhilePlaying = false;

    private bool _wasInputAlreadyBlocked = false;
    
    private void Awake()
    {
        _maxTime = (float)_cutscenePlayableDirector.duration;
    }

    public override void Execute()
    {
        base.Execute();
        _wasInputAlreadyBlocked = GameManager.Instance.BlockInput;
        if (_disableInputWhilePlaying) GameManager.Instance.BlockInput = true;
        _cutscenePlayableDirector.Play();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_disableInputWhilePlaying && _wasInputAlreadyBlocked == false) GameManager.Instance.BlockInput = false;
    }
}
