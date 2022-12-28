using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fabrique;

public class Chain
{
    public delegate void ChainEvent(Chain chain);

    public event ChainEvent ChainEnded;

    private ChainAction _currentChainAction;
    private int _currentChainActionIndex = -1;
    private Queue<ChainAction> _chainActions = new Queue<ChainAction>();
    
    private float _elapsedTime = 0.0f;

    public void UpdateChain(float elapsedTime)
    {
        if (_currentChainAction == null) return;
        
        _elapsedTime += elapsedTime;
        if (_elapsedTime > _currentChainAction.MaxTime)
        {
            StartNextChainAction();
        }
    }

    public void AddAction(ChainAction action)
    {
        _chainActions.Enqueue(action);
    }

    public void StartNextChainAction()
    {
        try
        {
            _currentChainAction = _chainActions.Peek();
        }
        catch
        {
            _currentChainAction = null;
            Debug.Log("Chain was ended!");
            ChainEnded?.Invoke(this);
            return;
        }
        
        _currentChainAction = _chainActions.Dequeue();
        _elapsedTime = 0.0f;
        _currentChainAction.Execute();
    }
}
