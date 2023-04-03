using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SetTimelineReference : MonoBehaviour
{
    public delegate void TimelineReferenceDelegate(PlayableDirector director);
    public event TimelineReferenceDelegate OnTimelineStarted;

    [Serializable]
    private struct TimelineObject
    {
        public TimelineObject(PlayableDirector director, GameObject animatedObject, string trackName)
        {
            Director = director;
            AnimatedObject = animatedObject;
            TrackName = trackName;
        }

        public PlayableDirector Director;
        public GameObject AnimatedObject;
        public string TrackName;
    }

    [SerializeField] private List<TimelineObject> _timelineObjects;

    private void Start()
    {
        foreach (var timeline in _timelineObjects)
        {
            timeline.Director.played += TimelineStarted;
        }
    }

    private void OnDestroy()
    {
        foreach (var timeline in _timelineObjects)
        {
            timeline.Director.played -= TimelineStarted;
        }
    }

    public void TimelineStarted(PlayableDirector director)
    {
        OnTimelineStarted?.Invoke(director);
        foreach (var timeline in _timelineObjects)
        {
            SetReference(timeline.Director, timeline.AnimatedObject, timeline.TrackName);
        }
    }    

    private void SetReference(PlayableDirector director, GameObject animatedObject, string trackName)
    {
        // Reference:
        // https://forum.unity.com/threads/need-to-set-bindings-at-runtime.851503/
        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        var trackList = timeline.GetOutputTracks();

        foreach (var track in trackList)
        {
            if (track.name == trackName)
            {
                director.SetGenericBinding(track, animatedObject);
            }
        }
    }

    public int GetTimelineCount()
    {
        return _timelineObjects.Count;
    }

    public void SetTimelineObjectAt(GameObject go, int index)
    {
        if (index < _timelineObjects.Count && index >= 0)
        {
            _timelineObjects[index] = new TimelineObject(_timelineObjects[index].Director, go, _timelineObjects[index].TrackName);
        }
    }
}
