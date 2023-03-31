using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEditor.Timeline;
using UnityEngine.Assertions;

public class FocusAction : ChainActionMonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private RectTransform _focusMask;
    [SerializeField] private Transform _focus;

    //public RectTransform FocusMask
    //{
    //    set
    //    {
    //        if (!_focusMask)
    //        {
    //            _focusMask = value;
    //        }
    //    }
    //}

    private void Start()
    {
        _startMaxTime = Mathf.Infinity;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;
        if (!_focusMask)
        {
            var go = GameObject.FindGameObjectWithTag("TutorialFocus");
            if (go.TryGetComponent(out RectTransform transform))
            {
                _focusMask = transform;
            }
            Assert.IsNotNull(_focusMask, "Could not find object with tag tutorial focus");
        }

        _focusMask.anchoredPosition = Camera.allCameras[0].WorldToScreenPoint(_focus.transform.position);
    }

    public override void Execute()
    {
        base.Execute();
        _timeline.Play();
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return null;
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
