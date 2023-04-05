using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

public class FocusAction : ChainActionMonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private RectTransform _focusMask;
    [SerializeField] private Transform _focus;

    private TutorialFocusMask _tutorialFocus;

    protected Transform Focus
    {
        get { return _focus; }
        set { _focus = value; }
    }

    protected virtual void Start()
    {
        _startMaxTime = Mathf.Infinity;
        _tutorialFocus = new TutorialFocusMask();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;

        _tutorialFocus?.Initialize(ref _focusMask);

        _focusMask.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
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
