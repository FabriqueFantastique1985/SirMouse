using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListNeedyBalloons : MonoBehaviour
{
    [HideInInspector]
    public List<ListNeedyObjectsInMe> NeedyBalloons = new List<ListNeedyObjectsInMe>();

    private void Awake()
    {
        NeedyBalloons = GetComponentsInChildren<ListNeedyObjectsInMe>().ToList();
    }
}
