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

    [FormerlySerializedAs("MainCamera")] public Camera CurrentCamera;
    [FormerlySerializedAs("MainCameraScript")] public FollowCam FollowCamera;
    public ZoomCam ZoomCamera;

    public GameObject PanelUIButtonsClosetAndBackpack;

    [HideInInspector]
    public SceneSetup CurrentSceneSetup;

    public LayerMask LayersMainGameSystemWillIgnore;
    public LayerMask LayersMiniGameSystemWillIgnore;
    public LayerMask LayersTutorialSystemWillIgnore;

    //public PlayerReferences PlayerRefs;

    private GameSystem _previousSystem = null; 

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
    public GameSystem CurrentGameSystem => _currentGameSystem; 

    #endregion
    
    protected override void Awake()
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

        // make sleeping legal
        Player.Character.SetBoolSleeping(false);
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

        // make sleeping legal
        Player.Character.SetBoolSleeping(false);
    }

    public void EnterMiniGameSystem()
    {
        _currentGameSystem = new MiniGameSystem(Player, LayersMiniGameSystemWillIgnore);

        // make sleeping illegal
        Player.Character.SetBoolSleeping(true);
    }

    public void EnterTutorialSystem()
    {
        _previousSystem = _currentGameSystem;
        _currentGameSystem = new TutorialSystem(Player, LayersTutorialSystemWillIgnore);

        // make sleeping illegal
        Player.Character.SetBoolSleeping(true);
    }

    public void ExitTutorialSystem()
    {
        if (_previousSystem != null)
        {
            _currentGameSystem = _previousSystem;
            _previousSystem = null;
        }
        else
        {
            _currentGameSystem = new MainGameSystem(Player, LayersMainGameSystemWillIgnore);
        }
    }

    private void Update()
    {
        if (_blockInput == false) 
            _currentGameSystem.HandleInput();
        _currentGameSystem.Update();

        _chain.UpdateChain(Time.deltaTime);
        _chainMono.UpdateChain(Time.deltaTime);
    }
}