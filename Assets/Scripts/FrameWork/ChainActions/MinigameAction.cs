using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private MiniGame _miniGame;

    [SerializeField]
    private Step _stepToGoBackWhenFailed;
    
    private void Awake()
    {
        _miniGame.OnMiniGameEnded += OnMiniGameEnded;
        _startMaxTime = Mathf.Infinity;
    }

    public override void Execute()
    {
        base.Execute();
        _miniGame.StartMiniGame();
    }

    private void OnMiniGameEnded(MiniGame minigame, MiniGame.MiniGameArgs args)
    {
        if (args.SuccessfullyCompleted)
        {
            _maxTime = -1.0f;
        }
        else
        {
            _miniGame.SetCurrentStep(_stepToGoBackWhenFailed, true);
        }
    }
}
