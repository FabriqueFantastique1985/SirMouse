using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private PlayableDirector _cutscenePlayableDirector;

    private void Awake()
    {
        _maxTime = (float)_cutscenePlayableDirector.duration;
    }

    public override void Execute()
    {
        base.Execute();
        _cutscenePlayableDirector.Play();
    }
}
