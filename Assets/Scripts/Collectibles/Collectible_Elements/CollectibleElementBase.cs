using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CollectibleElementBase : MonoBehaviour
{
    // needed to know what panel to influence
    public CollectibleType CollectibleType;

    // needed to know what panel to influence
    public CollectibleSpecificType CollectibleSpecificType;
}
