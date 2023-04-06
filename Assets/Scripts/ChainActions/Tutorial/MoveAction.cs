using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MoveAction : ChainActionMonoBehaviour
{
    [SerializeField] private RectTransform _focusMask;
    [SerializeField] private Transform _focus;

    private TutorialSystem _tutorialSystem;
    private TutorialFocusMask _tutorialFocus;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
        _tutorialFocus = new TutorialFocusMask();
        enabled = false;
    }

    private void Update()
    {
        _focusMask.position = _tutorialFocus.GetWorldPosToCameraPos(_focus.transform.position);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        enabled = true;

        _tutorialFocus.Initialize(ref _focusMask);

        if (IsInTargetArea(GameManager.Instance.Player.transform.position, _focusMask.transform.position, _focusMask.rect))
        {
            _maxTime = -1f;
        }

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(RaycastHit hit, Vector3 mousePos)
    {
        // Check if mousepos is inside of mask
        //if (IsInTargetArea(mousePos, Camera.allCameras[0].WorldToScreenPoint(_focusMask.transform.position), _focusMask.rect))
        if (IsInTargetArea(mousePos, _focusMask.transform.position, _focusMask.rect))
        {
            // Check layer and move player when clicking on ground
            var player = GameManager.Instance.Player;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                player.SetState(new WalkingState(player, hit.point));
                GameManager.Instance.BlockInput = true;
                StartCoroutine(HasArrived());
            }
        }
    }

    private IEnumerator HasArrived()
    {
        var player = GameManager.Instance.Player;
        while (player.transform.position != player.Agent.destination)
        {
            yield return null;
        }
        _maxTime = -1f;
    }

    private bool IsInTargetArea(Vector3 myPos, Vector3 targetPos, Rect targetRect)
    {
        var posInImage = myPos - targetPos;
        bool isInArea = (posInImage.x >= -(targetRect.width / 2f) && posInImage.x < targetRect.width / 2f &&
                         posInImage.y >= -(targetRect.height / 2f) && posInImage.y < targetRect.height / 2f);

        return isInArea;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        
        enabled = false;

        // Unsubscribe from click event
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick -= OnClick;
        }

        GameManager.Instance.BlockInput = false;
    }
}
