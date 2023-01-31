using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChainActionSceneLoaded : ChainActionMonoBehaviour
{
    [SerializeField]
    private List<ChainActionMonoBehaviour> _chainActions;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _chainActions.ForEach(x => GameManager.Instance.ChainMono.AddAction(x));
        GameManager.Instance.ChainMono.StartNextChainAction();
    }
}
