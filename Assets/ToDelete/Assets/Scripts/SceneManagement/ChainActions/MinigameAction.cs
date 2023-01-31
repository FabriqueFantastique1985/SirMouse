using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAction : ChainActionMonoBehaviour
{
    [SerializeField]
    private RecipeController _recipeGame;

    [SerializeField]
    private ChainActionMonoBehaviour _onMiniGameWonAction;

    [SerializeField]
    private ChainActionMonoBehaviour _onMiniGameLostAction;
    
    private void Awake()
    {
        _recipeGame.MiniGameEnded += OnMiniGameEnded;
        _maxTime = Mathf.Infinity;
    }

    public override void Execute()
    {
        base.Execute();
        _recipeGame.StartMinigame();
    }

    private void OnMiniGameEnded(bool hasWon)
    {
        if (hasWon) _maxTime = -1.0f;
    }
}
