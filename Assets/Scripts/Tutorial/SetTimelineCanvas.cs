using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetTimelineCanvas : MonoBehaviour
{
    private SetTimelineReference _timelineReference;
    private TutorialCanvas _tutorialCanvas;

    private void Start()
    {
        _timelineReference = GetComponent<SetTimelineReference>();
        _tutorialCanvas = new TutorialCanvas();
        _timelineReference.OnTimelineStarted += SetTimelineObjects;
    }

    public void SetTimelineObjects(PlayableDirector director)
    {
        _timelineReference.OnTimelineStarted -= SetTimelineObjects;

        Canvas canvas = null;
        _tutorialCanvas.Initialize(ref canvas);

        for (int i = 0; i < _timelineReference.GetTimelineCount(); i++)
        {
            _timelineReference.SetTimelineObjectAt(canvas.gameObject, i);
        }
    }
}
