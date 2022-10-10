using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField]
    private Collider[] _groundColliders;

    [SerializeField]
    private Interactable[] _interactables;

    public Collider[] GroundColliders => _groundColliders;
    
    public Interactable[] Interactables => _interactables;
}
