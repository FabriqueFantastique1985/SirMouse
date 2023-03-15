using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListNeedyObjectsInMe : MonoBehaviour
{
    [Header("Assign me a type if I want pickups")]
    public Type_Pickup PickupsNeeded; // if this is none, then this should work for touchables

    public List<NeedyObject> NeedyObjects = new List<NeedyObject>();

    [HideInInspector]
    public bool CompletedMe;


    private void Awake()
    {
        NeedyObjects = GetComponentsInChildren<NeedyObject>().ToList();
    }

}
