using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetTimelinePlayer : MonoBehaviour
{
    [SerializeField] private int _timelineIndex = -1;
    private SetTimelineReference _timelineReference;

    private void Start()
    {
        _timelineReference = GetComponent<SetTimelineReference>();
        _timelineReference.OnTimelineStarted += SetTimelineObjects;
    }

    public void SetTimelineObjects(PlayableDirector director)
    {
        _timelineReference.OnTimelineStarted -= SetTimelineObjects;

        GameObject player = GameManager.Instance.Player.gameObject;

        if (_timelineIndex >= 0)
        {
            _timelineReference.SetTimelineObjectAt(player, _timelineIndex);
            return;
        }

        for (int i = 0; i < _timelineReference.GetTimelineCount(); i++)
        {
            _timelineReference.SetTimelineObjectAt(player, i);
        }
    }

}
