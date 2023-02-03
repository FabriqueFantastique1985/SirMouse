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
    public Type_Pickup EquippedPickupType => _equippedPickupType;
    
    #endregion

    #region Fields

    private Stack<SirMouseState> _stateStack = new Stack<SirMouseState>();
    private Interactable _equippedItem;
    private Type_Pickup _equippedPickupType;

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
        
        // reset material texture
        _characterGeoReferences.SirMouseBody.mainTexture = _characterGeoReferences.DefaultTex;
        _characterGeoReferences.SirMouseHands.mainTexture = _characterGeoReferences.HandsLight;
    }
    
    public void SetTarget(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    private void Update()
    {
        var currentState = _stateStack?.Peek();

        if (currentState != null) currentState.Update(this);
    }

    public void SetState(SirMouseState newState)
    {
        var currentState = _stateStack?.Pop();
        if (newState == currentState) return;
        currentState?.OnExit(this);
        
        _stateStack.Push(newState);
        newState.OnEnter(this);
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

    public void Equip(Interactable itemToEquip, bool outOfBackpack = false)
    {
        _equippedItem = itemToEquip;
        _equippedPickupType = itemToEquip.MyPickupType;

        _equippedItem.gameObject.SetActive(true);
        _equippedItem.transform.parent = _characterRigReferences.HandRightTransform;
        _equippedItem.transform.localPosition = Vector3.zero;

        //_equippedItem.transform.localRotation = Quaternion.identity;
        if (outOfBackpack == true)
        {
            // fix rotation of interactable
            itemToEquip.transform.localEulerAngles = new Vector3(60, 117.618f, 0);
        }

    }

    public void Drop()
    {
        _equippedItem.transform.parent = null;
        var playerPos = transform.position;

        _equippedItem.transform.position = new Vector3(playerPos.x, 0.0f, playerPos.z);
        _equippedItem.transform.localScale = new Vector3(1, 1, 1 ); // always the same orientation

        // activate collider again
        _equippedItem.GetComponent<Collider>().enabled = true;
        _equippedItem.InteractionBalloon.BalloonTrigger.enabled = true;

        _equippedItem = null;
        _equippedPickupType = Type_Pickup.None;
    }

    public void Click(Player player)
    {
        if (_equippedItem == null || State.GetType() != typeof(IdleState)) return;
        SetState(new DropState(this));
    }
}