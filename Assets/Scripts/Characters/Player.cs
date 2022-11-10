using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IClickable
{
    #region EditorFields

    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private Character _character;

    [SerializeField]
    private CharacterRigReferences _characterRigReferences;
    [SerializeField]
    private CharacterGeoReferences _characterGeoReferences;

    #endregion

    #region Properties

    public SirMouseState State => _stateStack?.Peek();
    public Character Character => _character;
    public NavMeshAgent Agent => _agent;
    public Interactable EquippedItem => _equippedItem;
    public static SirMouseState s_WalkingState;
    
    #endregion

    #region Fields

    private Stack<SirMouseState> _stateStack = new Stack<SirMouseState>();
    private Interactable _equippedItem;

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
        _stateStack.Push(new IdleState(this));
        
        //test code
        s_WalkingState = new WalkingState(this, _agent.destination);
    }
    
    public void SetTarget(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    private void Update()
    {
        var currentState = _stateStack?.Peek();

        if (currentState != null) currentState.Update(this);
        
      // SirMouseState state = _state.Update(this);
      // if (state != null)
      // {
      //     _state?.OnExit(this);
      //     _state = state;
      //     _state.OnEnter(this);
      // }
    }

    public void SetState(SirMouseState newState)
    {
        var currentState = _stateStack?.Pop();
        if (newState == currentState) return;
        currentState?.OnExit(this);
        
        _stateStack.Push(newState);
        newState.OnEnter(this);
        
        // _state?.OnExit(this);
        // _state = newState;
        // _state.OnEnter(this);
    }

    public void PushState(SirMouseState newState)
    {
        // Exit current state
        var currentState = _stateStack.Peek();
        currentState?.OnExit(this);

        _stateStack.Push(newState);
        newState.OnEnter(this);
    }

    public void PopState()
    {
        var currentState = _stateStack.Pop();
        currentState?.OnExit(this);
    }

    public void Equip(Interactable itemToEquip)
    {
        _equippedItem = itemToEquip;
        _equippedItem.transform.parent = _characterRigReferences.HandRightTransform;
        _equippedItem.transform.localPosition = Vector3.zero;
        //_equippedItem.transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        _equippedItem.transform.parent = null;
        var playerPos = transform.position;
        _equippedItem.transform.position = new Vector3(playerPos.x, 0.0f, playerPos.z);
        _equippedItem = null;
    }

    public void Click(Player player)
    {
        if (_equippedItem == null || State.GetType() != typeof(IdleState)) return;
        SetState(new DropState(this));
    }
}