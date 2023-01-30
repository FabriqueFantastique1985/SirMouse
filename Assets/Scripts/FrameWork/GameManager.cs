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
        _currentGameSystem = new MainGameSystem(Player, new int[3]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13
            },
            newGroundColliders);
    }
    public void EnterMainGameSystem()
    {
        _currentGameSystem = new MainGameSystem(Player, new int[3]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13
            },
            GameManager.Instance.PlayField.GroundColliders);
    }
    public void EnterMiniGameSystem()
    {
        _currentGameSystem = new MiniGameSystem(Player, new int[3]
            {
                Player.gameObject.layer,
                PlayField.Interactables.Length <= 0 ? 0 : PlayField.Interactables[0].gameObject.layer,
                13
            }
            );
    }

    private void Update()
    {
        //Debug.Log(_blockInput + " block bool");

        if (_blockInput == false) _currentGameSystem.HandleInput();
        _currentGameSystem.Update();

        _chain.UpdateChain(Time.deltaTime);
    }
}