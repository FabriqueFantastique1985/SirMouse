using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;

public class StartMissionInteraction : Interaction
{
    [SerializeField]
    private Mission _mission;
    
    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);
        MissionHandler.Instance.StartMission(_mission);
    }
}
