using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionResource : MoveAction
{
    private bool _hasBeenCollected = false;

    private void OnEnable()
    {
        ResourceController.Instance.ResourceCollected += ResourceCollected;
    }

    private void ResourceCollected()
    {
        _hasBeenCollected = true;
    }

    private void OnDisable()
    {
        ResourceController.Instance.ResourceCollected -= ResourceCollected;
    }

    protected override bool HasCompletedObjective()
    {
        return _hasBeenCollected;
    }
}
