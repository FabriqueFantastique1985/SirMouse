using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fabrique;

public class ChainMono : IDisposable
{
    public delegate void ChainEvent(ChainMono chain);

    public event ChainEvent ChainEnded;

    private Queue<ChainActionMonoBehaviour> _chainActions = new Queue<ChainActionMonoBehaviour>();
    private ChainActionMonoBehaviour _currentChainAction;
    
    private int _currentChainActionIndex = -1;
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

    public void AddAction(ChainActionMonoBehaviour action, bool startImmediately = false)
    {
        _chainActions.Enqueue(action);
        if (startImmediately) StartNextChainAction();
    }

    public void StartNextChainAction()
    {
        // Check if not still playing an action
        if (_isPlaying) return;
            
        // Exit current ChainAction
        if (_currentChainAction != null) _currentChainAction.OnExit();
        
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

        // Start Next Chain Action
        _currentChainAction = _chainActions.Dequeue();
        _elapsedTime = 0.0f;
        _currentChainAction.OnEnter();
        _currentChainAction.Execute();
        _isPlaying = true;
    }

    public void ClearChain()
    {
        _currentChainAction = null;
        _isPlaying = false;
        _chainActions.Clear();
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
