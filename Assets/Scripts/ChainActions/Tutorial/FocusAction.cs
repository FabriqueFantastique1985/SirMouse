using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor.Timeline;

public class FocusAction : ChainActionMonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private RectTransform _focalPoint;
    [SerializeField] private Transform _focus;
    [SerializeField] private bool _isReversed;

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;
        _focalPoint.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
    }

    public override void Execute()
    {
        base.Execute();
        //if (_isReversed)
        //{
        //    _timeline.playable
        //}
        _timeline.Play();
        _timeline.stopped += TimelineEnd;
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
