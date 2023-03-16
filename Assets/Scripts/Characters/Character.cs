using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    #region Enums

    public enum States
    {
        Idle = 0,
        Walking = 1,
        Jumping = 2,
        PickUp = 3,
        UnEquip = 4,
        Drop = 5,
        TwoHanded = 6,
        BackpackExtraction = 7,
    };

    #endregion

    #region Events

    public delegate void CharacterEvent(States state);

    public event CharacterEvent AnimationDoneEvent;
    public event CharacterEvent InteractionDoneEvent;

    public event CharacterEvent EnteredIdleEvent;

    #endregion
    
    #region Properties
    public Animator AnimatorRM
    {
        get { return _animatorRM; }
    }
    #endregion

    #region EditorFields

    [FormerlySerializedAs("AnimatorRM")] [SerializeField]
    private Animator _animatorRM;

    [SerializeField]
    private Animator _explosionAnimator;

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

    [FormerlySerializedAs("_backpackExtractionCondition")]
    [SerializeField]
    private string _backpackExtractionName = "BackpackExtraction";

    [SerializeField]
    private string _unEquipName = "UnEquip";

    [FormerlySerializedAs("_blinkCondition")]
    [SerializeField]
    private string _blinkName = "Blink";

    [SerializeField]
    private string _twoHandedPickUpBool = "TwoHanded";

    #endregion

    #region Fields

    private Vector3 _originalScale;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        if (_animatorRM == null) Debug.LogError("Animator is null, did you forget to give a reference to it?");
        _originalScale = transform.localScale;
    }

   // /// <summary>
   // /// Sets the animator
   // /// </summary>
   // /// <param name="state"></param>
   // /// <param name="mirror"></param>
   // /// <returns></returns>
   // public virtual float SetAnimator(States state, bool mirror = false)
   // {
   //     return _animatorRM.GetCurrentAnimatorStateInfo(0).length;
   // }

    public void SetAnimatorTrigger(States state, bool mirror = false)
    {
        ResetAllTriggers();
        SetCharacterMirrored(mirror);

        string animationString = _idleName;

        switch (state)
        {
            case States.Idle:
                animationString = _idleName;
                break;
            case States.Walking:
                animationString = _walkName;
                break;
            case States.Jumping:
                break;
            case States.PickUp:
                animationString = _pickupName;
                break;
            case States.UnEquip:
                animationString = _unEquipName;
                break;
            case States.Drop:
                animationString = _dropName;
                break;
            case States.BackpackExtraction:
                animationString = _backpackExtractionName;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        _animatorRM.SetTrigger(animationString);
    }
    public void SetAnimatorBool(States state, bool setValue)
    {
        switch (state)
        {
            case States.TwoHanded:
                _animatorRM.SetBool(_twoHandedPickUpBool, setValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    public void SetCharacterMirrored(bool mirror)
    {
        if (mirror)
            transform.localScale = new Vector3(_originalScale.x * -1, _originalScale.y,
                _originalScale.z);
        else
        {
            transform.localScale = new Vector3(_originalScale.x, _originalScale.y,
                _originalScale.z);
        }
    }
    public void SetBoolSleeping(bool forbidSleep)
    {
        //_animatorRM.SetBool("ForbidSleep", forbidSleep);
    }
    
    private void ResetAllTriggers()
    {
        for (int i = 0; i < _animatorRM.parameterCount; i++)
        {
            if (_animatorRM.parameters[i].type == AnimatorControllerParameterType.Trigger)
            {
                _animatorRM.ResetTrigger(_animatorRM.parameters[i].name);
            }
        }
    }

    IEnumerator BlinkTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 4.0f));
        _animatorRM.SetTrigger(_blinkName);
    }
    
    public void AnimationDone(States state)
    {
        AnimationDoneEvent?.Invoke(state);
        Debug.Log(" Animation done for State: " + state);
    }

    public void InteractionDone(States state)
    {
        InteractionDoneEvent?.Invoke(state);
        Debug.Log("Interaction done for State: " + state);
    }

    public void EnteredIdle()
    {
        EnteredIdleEvent?.Invoke(States.Idle);
        //Debug.Log("Entered Idle state");
    }

    public void PlayExplosion()
    {
        _explosionAnimator.SetTrigger("Activate");
    }


    // animation events
    public void UnEquipGear()
    {
        
        SkinsMouseController.Instance.characterGeoReferences.Sword.gameObject.SetActive(false);
        SkinsMouseController.Instance.characterGeoReferences.Shield.gameObject.SetActive(false);

        SkinsMouseController.Instance.characterGeoReferences.SwordBack.gameObject.SetActive(true);
        SkinsMouseController.Instance.characterGeoReferences.ShieldBack.gameObject.SetActive(true);
    }
    public void EquipGear()
    {
        SkinsMouseController.Instance.characterGeoReferences.SwordBack.gameObject.SetActive(false);
        SkinsMouseController.Instance.characterGeoReferences.ShieldBack.gameObject.SetActive(false);

        SkinsMouseController.Instance.characterGeoReferences.Sword.gameObject.SetActive(true);
        SkinsMouseController.Instance.characterGeoReferences.Shield.gameObject.SetActive(true);
    }
}