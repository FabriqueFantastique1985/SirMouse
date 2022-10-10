using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField]
    private Collider _groundCollider;

    [SerializeField]
    private Interactable[] _interactables;

    public Collider GroundCollider => _groundCollider;
    
    public Interactable[] Interactables => _interactables;
}
