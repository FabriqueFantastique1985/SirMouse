using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAction : ChainActionMonoBehaviour
{
    private Action _action;
    
    public Action Action
    {
        get => _action;
        set => _action = value;
    }

    private void Awake()
    {
        // TO CHECK: does this cause the action to last a tick?
        _maxTime = 0.0f;
    }

    public override void Execute()
    {
        base.Execute();
        _action?.Invoke();
    }
}

