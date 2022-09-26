using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public enum States
    {
        Idle = 0,
        Walking = 1,
        Jumping = 2,
        PickUp = 3
    };
    
    [SerializeField]
    private Animator _animator;

    [FormerlySerializedAs("_runCondition")] [SerializeField]
    private string _walkName = "Run";

    [FormerlySerializedAs("_idleCondition")] [SerializeField]
    private string _idleName = "Idle";
    
    // Start is called before the first frame update
    void Start()
    {
        if (_animator == null) Debug.LogError("Animator is null, did you forget to give a reference to it?");
    }

    public virtual void SetAnimator(States state)
    {
        ResetAllTriggers();
        
        switch (state)
        {
            case States.Idle:
                _animator.SetTrigger(_idleName);
                break;
            case States.Walking:
                _animator.SetTrigger(_walkName);
                break;
            case States.Jumping:
                break;
        }
    }

    private void ResetAllTriggers()
    {
        for (int i = 0; i < _animator.parameterCount; i++)
        {
            if (_animator.parameters[i].type == AnimatorControllerParameterType.Trigger)
            {
                _animator.ResetTrigger(_animator.parameters[i].name);
            }
        }
    }
}
