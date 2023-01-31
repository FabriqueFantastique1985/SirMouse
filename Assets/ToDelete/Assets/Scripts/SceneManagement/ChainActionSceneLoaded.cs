using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChainActionSceneLoaded : ChainActionMonoBehaviour
{
    public ChainActionMonoBehaviour[] _chainActions;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        for (int i = 0; i < _chainActions.Length; i++)
        {
            GameManager.Instance.ChainMono.AddAction(_chainActions[i]);
        }

        GameManager.Instance.ChainMono.StartNextChainAction();
    }
}
