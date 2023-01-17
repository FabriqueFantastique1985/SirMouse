using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fabrique;

public class Chain : IDisposable
{
    public delegate void ChainEvent(Chain chain);

    public event ChainEvent ChainEnded;

    private ChainAction _currentChainAction;
    private int _currentChainActionIndex = -1;
    private Queue<ChainAction> _chainActions = new Queue<ChainAction>();
    
    private float _elapsedTime = 0.0f;

    private bool _isPlaying = false;
    private bool _destroyChainOnDone = false;

    public Chain(bool destroyChainOnDone)
    {
        _destroyChainOnDone = destroyChainOnDone;
    }

    public void UpdateChain(float elapsedTime)
    {
        if (_currentChainAction == null) return;
        
        _elapsedTime += elapsedTime;
        if (_elapsedTime > _currentChainAction.MaxTime)
        {
            _isPlaying = false;
            StartNextChainAction();
        }
    }

    public void AddAction(ChainAction action)
    {
        _chainActions.Enqueue(action);
    }

    public void StartNextChainAction()
    {
        if (_isPlaying)
        {
            Debug.Log("ChainAction was still playing. Aborting StartNextChainAction call.");
            return;
        }
        
        try
        {
            _currentChainAction = _chainActions.Peek();
        }
        catch
        {
            _currentChainAction = null;
            Debug.Log("Chain was ended!");
            ChainEnded?.Invoke(this);
            if (_destroyChainOnDone) Dispose();
            return;
        }
        
        _currentChainAction = _chainActions.Dequeue();
        _elapsedTime = 0.0f;
        _currentChainAction.Execute();
        _isPlaying = true;
    }


    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Chain()
    {
        ReleaseUnmanagedResources();
    }
}
