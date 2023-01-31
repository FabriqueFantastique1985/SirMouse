﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fabrique;

public class ChainMono : IDisposable
{
    public delegate void ChainEvent(ChainMono chain);

    public event ChainEvent ChainEnded;

    private ChainActionMonoBehaviour _currentChainAction;
    private int _currentChainActionIndex = -1;
    private Queue<ChainActionMonoBehaviour> _chainActions = new Queue<ChainActionMonoBehaviour>();
    
    private float _elapsedTime = 0.0f;

    private bool _isPlaying = false;
    private bool _destroyChainOnDone = false;

    public ChainMono(bool destroyChainOnDone)
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

    public void AddAction(ChainActionMonoBehaviour action)
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

    ~ChainMono()
    {
        ReleaseUnmanagedResources();
    }
}
