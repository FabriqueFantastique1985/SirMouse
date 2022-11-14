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

    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;

    public NavMeshAgent Agent;
    //public PlayerReferences PlayerRefs;

    public static GameManager Instance { get; private set; }


    #region Fields

    private GameSystem _currentGameSystem;
    private bool _blockInput = false;

    #endregion

    #region Properties

    public bool BlockInput
    {
        get => _blockInput; 
        set => _blockInput = value;
    }

    #endregion

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
        _currentGameSystem = new MainGameSystem(Player, new int[1]
        {
            /*Player.gameObject.layer, */
            PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer
        });

        // set the 
        //Player.transform.SetParent(this.gameObject.transform);
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
        if (_blockInput == false) _currentGameSystem.HandleInput();
        _currentGameSystem.Update();
    }
}