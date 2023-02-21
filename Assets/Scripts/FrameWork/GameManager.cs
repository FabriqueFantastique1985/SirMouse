using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public PlayField PlayField;
    public Player Player;

    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;

    [FormerlySerializedAs("MainCamera")] public Camera CurrentCamera;
    [FormerlySerializedAs("MainCameraScript")] public FollowCam FollowCamera;

    public GameObject PanelUIButtonsClosetAndBackpack;

    public LayerMask LayersMainGameSystemWillIgnore;
    public LayerMask LayersMiniGameSystemWillIgnore;

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

        if (PlayField == null)
        {
            PlayField = FindObjectOfType<PlayField>();
        }

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
            else
            {
                AdjustGameSystem(PlayField.GroundColliders);
            }
        }
    }

    // call this from scene controller when a scene is loaded
    public void AdjustGameSystem(Collider[] newGroundColliders)
    {
        _currentGameSystem = new MainGameSystem(Player, LayersMainGameSystemWillIgnore, newGroundColliders);
    }

    public void ExitMiniGameSystem(bool hasWon)
    {
        if (_currentGameSystem.GetType() == typeof(MiniGameSystem))
        {
            (_currentGameSystem as MiniGameSystem).EndMinigame(hasWon);
        }
    }

    public void EnterMainGameSystem()
    {
        _currentGameSystem = new MainGameSystem(Player, LayersMainGameSystemWillIgnore,
            GameManager.Instance.PlayField.GroundColliders);
    }

    public void EnterMiniGameSystem()
    {
        _currentGameSystem = new MiniGameSystem(Player, LayersMiniGameSystemWillIgnore);
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