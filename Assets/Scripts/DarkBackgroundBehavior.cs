using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DarkBackgroundBehavior : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private TimelineAsset _fadeIn;
    [SerializeField] private TimelineAsset _fadeOut;

    private void Start()
    {
        _canvas.worldCamera = Camera.allCameras[0];
    }

    public void FadeIn()
    {
        _playableDirector.playableAsset = _fadeIn;
        _playableDirector.Play();
    }

    public void FadeOut()
    {
        _playableDirector.playableAsset = _fadeOut;
        _playableDirector.Play();
    }
}
