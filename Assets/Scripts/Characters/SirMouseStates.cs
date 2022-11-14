﻿using System;
using System.Linq;
using UnityEngine;

public abstract class SirMouseState
{
    protected Player _player;
    protected static Vector3 Direction;

    public Action EnteredAction;
    public Action ExitedAction;

    private bool _blockInput = false;

    protected bool IsMirrored => Direction.x <= 0.0f;

    public SirMouseState(Player player, bool blockInput = false)
    {
        _blockInput = blockInput;
    }
    
    public virtual void OnEnter(Player player)
    {
        GameManager.Instance.BlockInput = _blockInput;
        _player = player;
        EnteredAction?.Invoke();
    }

    public virtual void OnExit(Player player)
    {
        GameManager.Instance.BlockInput = false;
        ExitedAction?.Invoke();
    }
    
    public virtual SirMouseState Update(Player player)
    {
        return null;
    }
}

public class WalkingState : SirMouseState
{
    // Minimum distance to reach for reaching the destination.
    private const float _minDistanceFromTarget = 0.1f;
    private Vector3 _target;
    
    public WalkingState(Player player, Vector3 target) : base(player)
    {
        SetTarget(player, target);
    }

    public void SetTarget(Player player, Vector3 target)
    {
        player.SetTarget(target);
        _target =  player.Agent.path.corners.Last();
        Direction = target - player.transform.position;
        player.Character.SetCharacterMirrored(IsMirrored);
    }

    public override void OnEnter(Player player)
    {
        //  change animation of the player
        player.Character.SetAnimatorTrigger(Character.States.Walking, Direction.x <= 0 );
        
        //Debug.Log("Entered Walking State");
    }

    public override SirMouseState Update(Player player)
    {
        float sqrDistToTarget = (player.transform.position - _target).sqrMagnitude;
        if (sqrDistToTarget < _minDistanceFromTarget * _minDistanceFromTarget) player.SetState(new IdleState(player));

        return null;
    }
}



public class IdleState : SirMouseState
{
    public IdleState(Player player) : base(player)
    {
        
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        player.Character.SetAnimatorTrigger(Character.States.Idle, Direction.x <= 0.0f);
        
        //Debug.Log("Entered Idle state");
    }
}

public class PickUpState : SirMouseState
{
    private Interactable _interactable;
    private Type_Pickup _typePickup;
    private bool _isTwoHandPickup = false;
    
    public PickUpState(Player player, Interactable pickUp, Type_Pickup typePickup, bool isTwoHandPickup = false)
        : base(player, true)
    {
        _interactable = pickUp;
        _typePickup = typePickup;
        _isTwoHandPickup = isTwoHandPickup;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        
        player.Character.SetAnimatorTrigger(Character.States.PickUp, IsMirrored);
        player.Character.SetAnimatorBool(Character.States.TwoHanded, _isTwoHandPickup);
        player.Character.AnimationDoneEvent += OnAnimationDone;
        player.Character.InteractionDoneEvent += OnInteractionDone;
        player.Character.EnteredIdleEvent += OnIdleEntered;
        
        Debug.Log("Entered PickUp State");
    }


    public override void OnExit(Player player)
    {
        base.OnExit(player);
        
        player.Character.AnimationDoneEvent -= OnAnimationDone;
        player.Character.InteractionDoneEvent -= OnInteractionDone;
        player.Character.EnteredIdleEvent -= OnIdleEntered;
    }

    private void OnAnimationDone(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }
    
    private void OnIdleEntered(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }
    
    private void OnInteractionDone(Character.States state)
    {
        _player.Equip(_interactable);
    }
}

public class DropState : SirMouseState
{
    public DropState(Player player) : base(player, true)
    {
        player.Character.InteractionDoneEvent += OnInteractionDone;
        player.Character.EnteredIdleEvent += OnEnteredIdle;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        
        player.Character.SetAnimatorTrigger(Character.States.Drop, IsMirrored);
    }

    public override void OnExit(Player player)
    {
        base.OnExit(player);
        
        player.Character.InteractionDoneEvent -= OnInteractionDone;
        player.Character.EnteredIdleEvent -= OnEnteredIdle;
    }

    private void OnInteractionDone(Character.States state)
    {
        _player.Drop();
    }
    
    private void OnEnteredIdle(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }
}
