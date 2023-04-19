﻿using System;
using System.Collections.Generic;
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
    private Vector3 _lastPosition;
    private int _framesStandingStill = 0;
    
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
        _framesStandingStill = 0;
        _lastPosition = player.transform.position;
        player.Character.SetAnimatorTrigger(Character.States.Walking, Direction.x <= 0 );
        
        //Debug.Log("Entered Walking State");
    }

    public override SirMouseState Update(Player player)
    {
        // check if the player is standing still
        if (Mathf.Approximately(player.transform.position.sqrMagnitude, _lastPosition.sqrMagnitude))
        {
            _framesStandingStill++;
            if (_framesStandingStill > 5)
            {
                player.SetState(new IdleState(player));
            }
        }
        else
        {
            _framesStandingStill = 0;
        }

        // Set the direction of the player
        Direction = player.transform.position - _lastPosition;
        player.Character.SetCharacterMirrored(IsMirrored);
        _lastPosition = player.transform.position;

        // check if the player reached the target
        float sqrDistToTarget = (player.transform.position - _target).sqrMagnitude;
        if (sqrDistToTarget < _minDistanceFromTarget * _minDistanceFromTarget) player.SetState(new IdleState(player));

        #if UNITY_EDITOR
        Debug.DrawLine(player.transform.position, _target, Color.red);
        #endif
        
        
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

        Debug.Log("Entered DropState State");
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




public class BackpackExtractionState : SirMouseState
{
    private Interactable _interactableToExtract;
    private Type_Pickup _typePickupToExtract;
    private bool _isTwoHandPickup = false;
    private GameObject _pressedButton;

    public BackpackExtractionState(Player player, Interactable pickUpToExtract, Type_Pickup typePickupToExtract, bool isTwoHandPickup = false)
        : base(player, true)
    {
        _interactableToExtract = pickUpToExtract;
        _typePickupToExtract = typePickupToExtract;
        _isTwoHandPickup = isTwoHandPickup;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        player.Character.SetAnimatorTrigger(Character.States.BackpackExtraction, IsMirrored);  
        //player.Character.SetAnimatorBool(Character.States.TwoHanded, _isTwoHandPickup);

        // reset the layer weights so the animation on body plays as intended
        player.Character.GetComponent<Animator>().SetLayerWeight(2, 0);
        player.Character.GetComponent<Animator>().SetLayerWeight(3, 0); // always 2 handed animation...

        player.Character.AnimationDoneEvent += OnAnimationDone;
        player.Character.InteractionDoneEvent += OnInteractionDone;
        player.Character.EnteredIdleEvent += OnIdleEntered;

        Debug.Log("Entered Backpack-Extracting State");
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
        // 3) set layer weights depending on the interactable specifics
        if (_isTwoHandPickup == true)
        {
            _player.Character.GetComponent<Animator>().SetLayerWeight(3, 1);
        }
        else
        {
            _player.Character.GetComponent<Animator>().SetLayerWeight(3, 0);
            _player.Character.GetComponent<Animator>().SetLayerWeight(2, 1);
        }
        

        _player.SetState(new IdleState(_player));
    }

    private void OnIdleEntered(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }

    private void OnInteractionDone(Character.States state)
    {
        // 1) put equiped item in backpack 
        var backPack = BackpackController.Instance;
        var player = GameManager.Instance.Player;       
        if (player.EquippedItem != null)
        {
            var pickupInteraction = player.EquippedItem.GetComponent<PickupInteraction>();

            backPack.AddItemToBackpackFromHands(player.EquippedItem, player.EquippedItem.gameObject, player.EquippedPickupType, pickupInteraction.SpriteRenderPickup);
        }

        // 2) put new pickup into hands (player.equip) 
        _player.Equip(_interactableToExtract, true);
    }
}


public class InstrumentEquipState : SirMouseState
{
    private Type_Instrument _instrumentType;
    private bool _isTwoHanded = false;


    public InstrumentEquipState(Player player, Type_Instrument typeInstrument, bool isTwoHandPickup = false)
        : base(player, true)
    {
        _instrumentType = typeInstrument;
        _isTwoHanded = isTwoHandPickup;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        player.Character.SetAnimatorTrigger(Character.States.InstrumentEquip, IsMirrored);

        // reset the layer weights so the animation on body plays as intended
        player.Character.GetComponent<Animator>().SetLayerWeight(2, 0);
        player.Character.GetComponent<Animator>().SetLayerWeight(3, 0); // always 2 handed animation...

        player.Character.AnimationDoneEvent += OnAnimationDone;
        player.Character.InteractionDoneEvent += OnInteractionDone;
        player.Character.EnteredIdleEvent += OnIdleEntered;

        //Debug.Log("Entered Equiping State");
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
        // 3) set layer weights depending on the interactable specifics
        if (_isTwoHanded == true)
        {
            _player.Character.GetComponent<Animator>().SetLayerWeight(3, 1);
        }
        else
        {
            _player.Character.GetComponent<Animator>().SetLayerWeight(3, 0);
            _player.Character.GetComponent<Animator>().SetLayerWeight(2, 1);
        }


        _player.SetState(new IdleState(_player));
    }

    private void OnIdleEntered(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }

    private void OnInteractionDone(Character.States state)
    {
        InstrumentController.Instance.EquipInstrument(_instrumentType);
    }
}


public class InstrumentUnequipState : SirMouseState
{
    public InstrumentUnequipState(Player player) : base(player, true)
    {
        player.Character.InteractionDoneEvent += OnInteractionDone;
        player.Character.EnteredIdleEvent += OnEnteredIdle;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        player.Character.SetAnimatorTrigger(Character.States.InstrumentUnequip, IsMirrored);

        //Debug.Log("Entered Unequiping State");
    }

    public override void OnExit(Player player)
    {
        base.OnExit(player);

        player.Character.InteractionDoneEvent -= OnInteractionDone;
        player.Character.EnteredIdleEvent -= OnEnteredIdle;


    }

    private void OnInteractionDone(Character.States state)
    {
        InstrumentController.Instance.UnEquipInstrument();
    }

    private void OnEnteredIdle(Character.States state)
    {
        _player.SetState(new IdleState(_player));
    }
}
