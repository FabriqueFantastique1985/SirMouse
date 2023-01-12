using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;

public class MissionAction : ChainAction
{
    private enum ActionType
    {
        StartMission
    }

    [SerializeField]
    private Mission _mission;

    [SerializeField]
    private ActionType _actionType;

    public override void Execute()
    {
        base.Execute();

        switch (_actionType)
        {
            case ActionType.StartMission:
                _mission.StartMission();
                break;
        }
    }
}