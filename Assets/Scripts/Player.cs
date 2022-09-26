using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private Character _character;

    private SirMouseState _state;
    
    private Vector3 _destination;
    
    public SirMouseState State { get; set; }

    public Character Character => _character;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        SetState(new IdleState(this));
    }
    
    public void SetTarget(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    private void Update()
    {
        SirMouseState state = _state.Update(this);
        if (state != null)
        {
            _state = state;
            _state.OnEnter(this);
        }
    }

    public void SetState(SirMouseState newState)
    {
        _state = newState;
        _state.OnEnter(this);
    }
}