using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotResource : MonoBehaviour
{
    public Type_Resource ResourceType;
    public bool SlotTaken;
    public int Amount;

    [Header("References Children")]
    public GameObject Visuals;
    public Image ImageAmount;
}
