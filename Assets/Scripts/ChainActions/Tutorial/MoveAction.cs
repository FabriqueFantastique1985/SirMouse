using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MoveAction : ChainActionMonoBehaviour
{
    [SerializeField] private TutorialFocusMask _focusMask;
    [SerializeField] private bool _keepFocusCentered;
    [SerializeField] private Transform _focus;

    private TutorialSystem _tutorialSystem;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _focusMask.Initialize();

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(RaycastHit hit, Vector3 mousePos)
    {
        var posInImage = mousePos - _focusMask.Mask.transform.position;
        
        // Check if mousepos is inside of mask
        if (posInImage.x >= -(_focusMask.Mask.rect.width / 2f) && posInImage.x < _focusMask.Mask.rect.width / 2f &&
            posInImage.y >= -(_focusMask.Mask.rect.height / 2f) && posInImage.y < _focusMask.Mask.rect.height / 2f)
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
            if (_keepFocusCentered)
            {
                _focusMask.Mask.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
            }
            yield return null;
        }
        _maxTime = -1f;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        GameManager.Instance.BlockInput = false;

        // Unsubscribe from click event
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick -= OnClick;
        }
    }
}
