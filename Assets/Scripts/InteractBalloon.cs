using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class InteractBalloon : MonoBehaviour
{
    public virtual void ExecuteInteraction()
    {
        Debug.Log("Action executed");
    }
}
