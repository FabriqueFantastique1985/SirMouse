using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumController : MiniGame
{
    [SerializeField]
    List<ButtonPodium> _buttonsPodium = new List<ButtonPodium>();

    [SerializeField]
    private float _noInputTimer;

    [SerializeField]
    private Canvas _canvas;

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

    private void Start()
    {
        foreach(var button in _buttonsPodium)
        {
            button.OnButtonClicked += ButtonClicked;
        }

        _canvas.worldCamera = Camera.main;
        _canvas.gameObject.SetActive(false);

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
        
        if (_buttonClickedAmount < 4)
        {
            StartCoroutine(ClickTimer());
            return;
        }
        StartCoroutine(OnButtonsClicked());
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
        _canvas.gameObject.SetActive(true);
        _playerController = _animator.runtimeAnimatorController;
        _animator.runtimeAnimatorController = _poseController;

        // Move player into position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _childTransforms.Add(_playerObject.transform.GetChild(i).position);
            _playerObject.transform.GetChild(i).position = _playerLocation.position;
        }

        GameManager.Instance.EnterMiniGameSystem();
        StartCoroutine(ClickTimer());
    }

    private IEnumerator ClickTimer()
    {
        int amount = _buttonClickedAmount;
        yield return new WaitForSeconds(_noInputTimer);

        if (amount == _buttonClickedAmount)
        {
            EndMiniGame(false);
        }
    }

    private void EndMinigame(bool hasSucceeded)
    {
        // Check if minigame is actually running
        if (!_isMinigameActive)
        {
            return;
        }
        _isMinigameActive = false;

        base.EndMiniGame(hasSucceeded);

        // Hide canvas and reset player animator
        _canvas.gameObject.SetActive(false);
        _animator.runtimeAnimatorController = _playerController;

        // Reset variables for next run
        _buttonClickedAmount = 0;

        // Move player back to original position
        for (int i = 0; i < _playerObject.transform.childCount; i++)
        {
            _playerObject.transform.GetChild(i).position = _childTransforms[i];
        }

        GameManager.Instance.EnterMainGameSystem();
    }

    private IEnumerator OnButtonsClicked()
    {
        while (_isPlayingAnimation)
        {
            yield return null;
        }
        EndMinigame(true);
    }
}
