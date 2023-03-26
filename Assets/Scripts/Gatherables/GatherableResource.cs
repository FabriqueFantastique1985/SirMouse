using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableResource : GatherableObject
{
    public Type_Resource ResourceType;

    public override void PickedUpGatherable()
    {
        base.PickedUpGatherable();

        ResourceController.Instance.AddResource(ResourceType);

        UIFlyingToBackpackController.Instance.ThrowItemIntoBackpack(this, ResourceType);
    }
}
