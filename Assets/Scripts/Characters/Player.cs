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

    [SerializeField]
    private Transform _equippedItemTransform;

    private SirMouseState _state;
    private Vector3 _destination;

    private Interactable _equippedItem;

    #region Properties

    public SirMouseState State { get; set; }

    public Character Character => _character;
    public NavMeshAgent Agent => _agent;

    public Interactable EquippedItem => _equippedItem;

    #endregion
    
    
    //private void Awake()
    //{
    //    Initialize();
    //}

    private void Start()
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
            _state?.OnExit(this);
            _state = state;
            _state.OnEnter(this);
        }
    }

    public void SetState(SirMouseState newState)
    {
        _state?.OnExit(this);
        _state = newState;
        _state.OnEnter(this);
    }
}