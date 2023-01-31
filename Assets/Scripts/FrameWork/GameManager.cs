using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public PlayField PlayField;
    public Player Player;

    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;

    public Camera MainCamera;
    public FollowCam MainCameraScript;

    //public PlayerReferences PlayerRefs;

    #region Fields

    private bool _blockInput = false;
    private GameSystem _currentGameSystem;
    private Chain _chain = new Chain(false);
    private ChainMono _chainMono = new ChainMono(false);
    #endregion

    #region Properties

    public bool BlockInput
    {
        get => _blockInput; 
        set => _blockInput = value;
    }
    
    public Chain Chain => _chain;
    public ChainMono ChainMono => _chainMono;

    #endregion

    private void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;

        EnterMainGameSystem();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var players = FindObjectsOfType<Player>();

        if (Player == null && players.Length > 0)
        {
            Player = players[0];
        }
        else if (Player != null && players.Length > 1)
        {
            foreach (var player in players)
            {
                if (player == Player) continue;
                else Destroy(player);
            }
        }
            
        if (PlayField == null)
        {
            PlayField = FindObjectOfType<PlayField>();
            if (PlayField == null) Debug.LogError($"No PlayField found in this scene {SceneManager.GetActiveScene()}");
        }
    }

    // call this from scene controller when a scene is loaded
    public void AdjustGameSystem(Collider[] newGroundColliders)
    {
        _currentGameSystem = new MainGameSystem(Player, new int[4]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13, 14
            },
            newGroundColliders);
    }
    public void EnterMainGameSystem()
    {
        _currentGameSystem = new MainGameSystem(Player, new int[4]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13, 14
            },
            GameManager.Instance.PlayField.GroundColliders);
    }
    public void EnterMiniGameSystem()
    {
        _currentGameSystem = new MiniGameSystem(Player, new int[4]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13, 14
            }
            );
    }

    private void Update()
    {
        //Debug.Log(_blockInput + " block bool");

        if (_blockInput == false) _currentGameSystem.HandleInput();
        _currentGameSystem.Update();

        _chain.UpdateChain(Time.deltaTime);
        _chainMono.UpdateChain(Time.deltaTime);
    }
}