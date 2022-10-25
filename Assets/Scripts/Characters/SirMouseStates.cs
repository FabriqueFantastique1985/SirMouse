using System;
using System.Linq;
using UnityEngine;

public abstract class SirMouseState
{
    protected Player _player;
    protected static Vector3 Direction;

    public Action EnteredAction;
    public Action ExitedAction;

    public SirMouseState(Player player)
    {
    }
    
    public virtual void OnEnter(Player player)
    {
        EnteredAction?.Invoke();
    }

    public virtual void OnExit(Player player)
    {
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
        //_target = target;
        player.SetTarget(target);
        _target =  player.Agent.path.corners.Last();

        Direction = target - player.transform.position;
    }

    public override void OnEnter(Player player)
    {
        //  change animation of the player
        player.Character.SetAnimator(Character.States.Walking, Direction.x <= 0 );
    }

    public override SirMouseState Update(Player player)
    {
        float sqrDistToTarget = (player.transform.position - _target).sqrMagnitude;
        if (sqrDistToTarget < _minDistanceFromTarget * _minDistanceFromTarget) return new IdleState(player);

        return null;
    }
}



public class IdleState : SirMouseState
{
    public IdleState(Player player) : base(player)
    {
        
    }

    public override SirMouseState Update(Player player)
    {
        return null;
    }

    public override void OnEnter(Player player)
    {
        player.Character.SetAnimator(Character.States.Idle, Direction.x <= 0.0f);
    }
}




public class InteractionState : SirMouseState
{
    private float _timeInState;
   
    public InteractionState(Player player, AnimationClip animationClip, Action actionOnEnter = null, Action actionOnExit = null) : base(player)
    {
        _timeInState = animationClip.length;
        EnteredAction = actionOnEnter;
        ExitedAction = actionOnExit;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        //player.Character.SetAnimator();
    }

    public override SirMouseState Update(Player player)
    {
        _timeInState -= Time.deltaTime;
        if (_timeInState <= 0.0f) return new IdleState(player);

        return null;
    }
}
