using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameObjectsAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private GameObject[] _gameObjectsToEnable;
    
    [SerializeField]
    private GameObject[] _gameObjectsToDisable;

    private void Awake()
    {
        _maxTime = 0.0f;
    }

    public override void Execute()
    {
        base.Execute();
        for (int i = 0; i < _gameObjectsToEnable.Length; i++)
        {
            _gameObjectsToEnable[i].SetActive(true);
        }

        for (int i = 0; i < _gameObjectsToDisable.Length; i++)
        {
            _gameObjectsToDisable[i].SetActive(false);
        }
    }
}
