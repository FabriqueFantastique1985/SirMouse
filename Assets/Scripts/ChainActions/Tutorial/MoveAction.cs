﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MoveAction : ChainActionMonoBehaviour
{
    [SerializeField] private RectTransform _focusMask;
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

        // Subscribe to click event
        _tutorialSystem = GameManager.Instance.CurrentGameSystem as TutorialSystem;
        if (_tutorialSystem != null)
        {
            _tutorialSystem.OnClick += OnClick;
        }
    }

    private void OnClick(LayerMask layer, Vector3 mousePos, Vector3 hitPoint)
    {
        var posInImage = mousePos - _focusMask.transform.position;
        
        // Check if mousepos is inside of mask
        if (posInImage.x >= -(_focusMask.rect.width / 2f) && posInImage.x < _focusMask.rect.width / 2f &&
            posInImage.y >= -(_focusMask.rect.height / 2f) && posInImage.y < _focusMask.rect.height / 2f)
        {
            // Check layer and move player when clicking on ground
            var player = GameManager.Instance.Player;
            if (layer == LayerMask.NameToLayer("Ground"))
            {
                player.SetState(new WalkingState(player, hitPoint));
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
                _focusMask.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
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
