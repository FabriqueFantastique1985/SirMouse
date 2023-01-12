using System.Collections;
using System.Collections.Generic;
using Fabrique;
using UnityEngine;

public class ObjectiveAction : ChainAction
{
    [SerializeField]
    private Objective _toCompleteObjective;

    public override void Execute()
    {
        base.Execute();
        _toCompleteObjective.SetObjectiveReached();
    }
}
