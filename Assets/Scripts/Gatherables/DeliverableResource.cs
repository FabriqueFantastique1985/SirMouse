using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliverableResource 
{
    public Type_Resource ResourceType;

    [HideInInspector]
    public bool Delivered;
}
