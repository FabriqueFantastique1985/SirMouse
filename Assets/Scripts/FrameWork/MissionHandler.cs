using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;

public class MissionHandler : MonoBehaviourSingleton<MissionHandler>
{
    [SerializeField]
    private List<Mission> _missions;
    
    private Mission _currentMission;
    
    protected override void Awake()
    {
        base.Awake();
        if (_missions.Count > 0) _currentMission = _missions[0];
    }

    public void StartMission(Mission mission)
    {
        _currentMission = mission;
        _currentMission.StartMission();
    }
    
    public void SetCurrentMission(Mission newMission)
    {
        _currentMission = newMission;
    }
}
