using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public PlayField PlayField;
    public Player Player;
    public Character Character;
    public NavMeshAgent Agent;
    public PlayerReferences PlayerRefs;
    

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

        // 1 protected player gets assigned here -> make this player part of dont destroyonload...
        _currentGameSystem = new MainGameSystem(Player, new int[2]
        {
            Player.gameObject.layer, PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
        });

        // set the 
        //Player.transform.SetParent(this.gameObject.transform);
    }



    // call this from scene controller when a scene is loaded
    public void AdjustGameSystem(Collider[] newGroundColliders)
    {
        _currentGameSystem = new MainGameSystem(Player, new int[2]
        {
            Player.gameObject.layer, PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
        }, 
        newGroundColliders);
    }




    private void Update()
    {
        _currentGameSystem.HandleInput();
        _currentGameSystem.Update();
    }
}

