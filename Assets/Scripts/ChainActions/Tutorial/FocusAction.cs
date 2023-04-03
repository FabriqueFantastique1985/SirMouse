using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor.Timeline;
using System;

public class FocusAction : ChainActionMonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private TutorialFocusMask _focusMask;
    [SerializeField] private Transform _focus;

    private const string _tutorialFocusTag = "TutorialFocus";

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;

        _focusMask.Initialize();

        _focusMask.Mask.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
    }

    private void SearchFocusMask()
    {
        var go = GameObject.FindGameObjectWithTag(_tutorialFocusTag);
        RectTransform transform = go.GetComponent<RectTransform>();
        if (transform == null)
            throw new MissingComponentException();
        _focusMask.Mask = transform;
    }

    public override void Execute()
    {
        base.Execute();
        _timeline.Play();

        // If timeline is not setup correctly, will instantly stop this action
        if (_timeline.state == PlayState.Playing)
        {
            _timeline.stopped += TimelineEnd;
        }
        else
        {
            _maxTime = -1f;
        }
    }

    private void TimelineEnd(PlayableDirector obj)
    {
        obj.stopped -= TimelineEnd;
        _maxTime = -1f;
    }

    public override void OnExit()
    {
        base.OnExit();
        GameManager.Instance.BlockInput = false;
    }
}
