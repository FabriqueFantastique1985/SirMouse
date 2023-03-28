﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PodiumController : MiniGame
{
    #region Events
    public delegate void PodiumControllerDelegate();
    public event PodiumControllerDelegate OnPoseTaken;
    public event PodiumControllerDelegate OnMiniGameEnd;
    #endregion

    #region Fields
    [SerializeField] List<ButtonPodium> _buttonsPodium = new List<ButtonPodium>();

    [SerializeField] private float _poseTimer;
    [SerializeField] private int _amountOfPosesRequired = 4;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private ShineBehaviour _shineBehaviour;

    [Header ("Cutscene information")]
    [SerializeField] private PlayableDirector _cutscene01;
    [SerializeField] private PlayableDirector _cutscene03;
    [SerializeField] private string _playerTrackName;
    
    [Header("Player")]
    [SerializeField] private Transform _playerLocation;

    [SerializeField] private RuntimeAnimatorController _podiumAnimator;
    private RuntimeAnimatorController _playerController;

    private List<Vector3> _playerChildTransforms = new List<Vector3>();
    private Animator _animator;
    private GameObject _playerObject;

    private int _buttonClickedAmount;

    private bool _isMinigameActive = false;
    private bool _isPlayingAnimation = false;
    #endregion

    #region Properites
    public int AmountOfPosesRequired
    {
        get { return _amountOfPosesRequired; }
    }
    public int AmountOfPosesTaken
    {
        get { return _buttonClickedAmount; }
    }
    #endregion

    private void Start()
    {
        foreach(var button in _buttonsPodium)
        {
            button.OnButtonClicked += ButtonClicked;
            //button.gameObject.SetActive(false);
        }

        _animator = GameManager.Instance.Player.Character.AnimatorRM;
        _playerObject = GameManager.Instance.Player.gameObject;

        // Set button information
        foreach (var button in _buttonsPodium)
        {
            button.PlayerAnimator = _animator;
        }

        GameManager.Instance.Player.Character.AnimationDoneEvent += AnimationDone;
    }

    private void AnimationDone(Character.States state)
    {
        if (_isPlayingAnimation)
        {
            _isPlayingAnimation = false;
        }
    }

    private void ButtonClicked(ButtonPodium button)
    {
        // Only register button click when no animation is playing
        if (_isPlayingAnimation)
        {
            return;
        }
        _isPlayingAnimation = true;

        // Play animation
        button.PlayAnimation();

        // Keep track of button pressed amount
        ++_buttonClickedAmount;
        _buttonClickedAmount = Mathf.Clamp(_buttonClickedAmount, 0, _amountOfPosesRequired);

        OnPoseTaken?.Invoke();
    }

    private IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(_poseTimer);

        // Only end minigame after last animation is played
        while (_isPlayingAnimation)
        {
            yield return null;
        }
        EndMiniGame();
    }

    private void EndMiniGame()
    {
        // Check if minigame is actually running
        if (!_isMinigameActive)
        {
            return;
        }
        _isMinigameActive = false;

        base.EndMiniGame(true);

        // Hide canvas and reset player animator
        foreach (var button in _buttonsPodium)
        {
            button.gameObject.SetActive(false);
        }
        _animator.runtimeAnimatorController = _playerController;

        // Reset variables for next run
        _buttonClickedAmount = 0;

        OnMiniGameEnd?.Invoke();
    }

    public void SetPlayerReference()
    {
        // Turn off shine
        _shineBehaviour.IsShineActive = false;

        // Turn off current camera
        GameManager.Instance.CurrentCamera.gameObject.SetActive(false);

        // Set player rig
        SetPlayerReference(_cutscene01, GameManager.Instance.Player.Character.AnimatorRM, _playerTrackName);
        SetPlayerReference(_cutscene03, GameManager.Instance.Player.Character.AnimatorRM, _playerTrackName);

        // Move player into position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _playerChildTransforms.Add(_playerObject.transform.GetChild(i).position);
            _playerObject.transform.GetChild(i).position = _playerLocation.position;
        }

        GameManager.Instance.EnterMiniGameSystem();
    }

    public void SetPlayerReference(PlayableDirector director, Animator animator, string trackName)
    {
        // Reference:
        // https://forum.unity.com/threads/need-to-set-bindings-at-runtime.851503/
        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        var trackList = timeline.GetOutputTracks();

        foreach (var track in trackList)
        {
            if (track.name == trackName)
            {
                director.SetGenericBinding(track, animator);
            }
        }
    }

    public void PodiumEnd()
    {
        // Turn on shine
        _shineBehaviour.IsShineActive = true;

        // Move player back to original position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _playerObject.transform.GetChild(i).position = _playerChildTransforms[i];
        }

        // Turn on player camera
        GameManager.Instance.CurrentCamera.gameObject.SetActive(true);

        GameManager.Instance.EnterMainGameSystem();
    }

    public override void StartMiniGame()
    {
        // Check if minigame is already running
        if (_isMinigameActive)
        {
            return;
        }
        _isMinigameActive = true;

        base.StartMiniGame();

        // Set canvas and player animator controller
        foreach (var button in _buttonsPodium)
        {
            //button.gameObject.SetActive(true);
        }

        _playerController = _animator.runtimeAnimatorController;
        _animator.runtimeAnimatorController = _podiumAnimator;

        StartCoroutine(ClickTimer());
    }
}
