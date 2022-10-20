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
        PickUp = 3,
        Drop = 4
    };
    
    public Animator AnimatorRM;

    [FormerlySerializedAs("_runCondition")] [SerializeField]
    private string _walkName = "Run";

    [FormerlySerializedAs("_idleCondition")] [SerializeField]
    private string _idleName = "Idle";

    [FormerlySerializedAs("_jumpCondition")] [SerializeField]
    private string _jumpName = "Jump";

    [FormerlySerializedAs("_pickupCondition")] [SerializeField]
    private string _pickupName = "Pickup";

    [FormerlySerializedAs("_dropCondition")] [SerializeField]
    private string _dropName = "Drop";

    [FormerlySerializedAs("_blinkCondition")]
    [SerializeField]
    private string _blinkName = "Blink";

    private Vector3 _originalScale;
    
    // Start is called before the first frame update
    void Start()
    {
        if (AnimatorRM == null) Debug.LogError("Animator is null, did you forget to give a reference to it?");
        _originalScale = transform.localScale;
    }

    public virtual void SetAnimator(States state, bool mirror = false)
    {
        //if (AnimatorRM != null)
        //{
            ResetAllTriggers();

            if (mirror)
                transform.localScale = new Vector3(  _originalScale.x * -1, _originalScale.y,
                    _originalScale.z);
            else
            {
                transform.localScale = new Vector3( _originalScale.x , _originalScale.y,
                    _originalScale.z);
            }

            switch (state)
            {
                case States.Idle:
                    AnimatorRM.SetTrigger(_idleName);
                    break;
                case States.Walking:
                    AnimatorRM.SetTrigger(_walkName);
                    break;
                case States.Jumping:
                    break;
                case States.PickUp:
                    AnimatorRM.SetTrigger(_pickupName);
                    break;
                case States.Drop:
                    AnimatorRM.SetTrigger(_dropName);
                    break;
        }
        //}
    }

    private void ResetAllTriggers()
    {
        for (int i = 0; i < AnimatorRM.parameterCount; i++)
        {
            if (AnimatorRM.parameters[i].type == AnimatorControllerParameterType.Trigger)
            {
                AnimatorRM.ResetTrigger(AnimatorRM.parameters[i].name);
            }
        }  
    }

    IEnumerator BlinkTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 4.0f));
        AnimatorRM.SetTrigger(_blinkName);
    }
}