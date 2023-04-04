using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAction : ChainActionMonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;
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
