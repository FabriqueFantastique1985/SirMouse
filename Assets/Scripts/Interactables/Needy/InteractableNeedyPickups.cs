using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNeedyPickups : InteractableNeedy
{
    [Header("Needing Pickups")]
    [SerializeField]
    private List<Type_Pickup> _wantedPickups;

    private List<Type_Pickup> _heldPickups;
}
