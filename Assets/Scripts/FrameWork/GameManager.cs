using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayField PlayField;
    public Player Player;

    public static GameManager Instance { get; private set; }

    private GameSystem _currentGameSystem;

    private void Awake()
    {
        // Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);

        _currentGameSystem = new MainGameSystem(Player, new int[2]
        {
            Player.gameObject.layer, PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
        });
    }

    private void Update()
    {
        _currentGameSystem.HandleInput();
        _currentGameSystem.Update();
    }
}

