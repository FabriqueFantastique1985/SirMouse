using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PodiumController : MiniGame
{
    public delegate void PodiumControllerDelegate();
    public event PodiumControllerDelegate OnPoseTaken;
    public event PodiumControllerDelegate OnMiniGameEnd;

    [SerializeField]
    List<ButtonPodium> _buttonsPodium = new List<ButtonPodium>();

    [SerializeField]
    private float _poseTimer;

    [SerializeField]
    private int _amountOfPosesRequired = 4;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _buttons;

    [Header ("Cutscene 01 information and references")]
    [SerializeField]
    private PlayableDirector _cutscene01;
    [SerializeField]
    private string _playerTrackName;

    [Header("Player")]
    [SerializeField]
    private Transform _playerLocation;

    [SerializeField]
    private RuntimeAnimatorController _poseController;
    private RuntimeAnimatorController _playerController;

    private int _buttonClickedAmount;
    private Animator _animator;
    private GameObject _playerObject;

    private List<Vector3> _childTransforms = new List<Vector3>();

    private bool _isMinigameActive = false;
    private bool _isPlayingAnimation = false;

    public int AmountOfPosesRequired
    {
        get { return _amountOfPosesRequired; }
    }
    public int AmountOfPosesTaken
    {
        get { return _buttonClickedAmount; }
    }

    private void Start()
    {
        foreach(var button in _buttonsPodium)
        {
            button.OnButtonClicked += ButtonClicked;
        }

        _canvas.worldCamera = Camera.main;
        _buttons.SetActive(false);

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
        if (_isPlayingAnimation)
        {
            return;
        }
        _isPlayingAnimation = true;

        button.PlayAnimation();
        ++_buttonClickedAmount;
        _buttonClickedAmount = Mathf.Clamp(_buttonClickedAmount, 0, _amountOfPosesRequired);

        OnPoseTaken?.Invoke();
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
        _buttons.SetActive(true);
        _playerController = _animator.runtimeAnimatorController;
        _animator.runtimeAnimatorController = _poseController;

        GameManager.Instance.EnterMiniGameSystem();
        StartCoroutine(ClickTimer());
    }

    private IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(_poseTimer);

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
        _buttons.SetActive(false);
        _animator.runtimeAnimatorController = _playerController;

        // Reset variables for next run
        _buttonClickedAmount = 0;

        // Move player back to original position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _playerObject.transform.GetChild(i).position = _childTransforms[i];
        }

        GameManager.Instance.EnterMainGameSystem();
        OnMiniGameEnd?.Invoke();
    }

    public void SetPlayerReference()
    {
        // Turn off current camera
        GameManager.Instance.CurrentCamera.gameObject.SetActive(false);

        // Move player into position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _childTransforms.Add(_playerObject.transform.GetChild(i).position);
            _playerObject.transform.GetChild(i).position = _playerLocation.position;
        }

        // Set player rig
        TimelineAsset timeline = _cutscene01.playableAsset as TimelineAsset;
        var trackList = timeline.GetOutputTracks();

        foreach (var track in trackList)
        {
            if (track.name == _playerTrackName)
            {
                _cutscene01.SetGenericBinding(track, GameManager.Instance.Player.Character.AnimatorRM);
            }
        }
    }
}
