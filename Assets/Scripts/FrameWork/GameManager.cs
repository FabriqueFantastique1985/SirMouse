using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public PlayField PlayField;
    public Player Player;
    public Character Character;

    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;

    public NavMeshAgent Agent;
    //public PlayerReferences PlayerRefs;

    #region Fields

    private bool _blockInput = false;
    private GameSystem _currentGameSystem;
    private Chain _chain = new Chain();

    #endregion

    #region Properties

    public bool BlockInput
    {
        get => _blockInput; 
        set => _blockInput = value;
    }
    
    public Chain Chain => _chain;

    #endregion

    private void Awake()
    {
        base.Awake();

        _currentGameSystem = new MainGameSystem(Player, new int[1]
        {
            PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
        });
    }

    // call this from scene controller when a scene is loaded
    public void AdjustGameSystem(Collider[] newGroundColliders)
    {
        _currentGameSystem = new MainGameSystem(Player, new int[2]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
            },
            newGroundColliders);
    }

    private void Update()
    {
        //Debug.Log(_blockInput + " block bool");

        if (_blockInput == false) _currentGameSystem.HandleInput();
        _currentGameSystem.Update();

        _chain.UpdateChain(Time.deltaTime);
    }
}