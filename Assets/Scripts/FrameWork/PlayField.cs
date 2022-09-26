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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
