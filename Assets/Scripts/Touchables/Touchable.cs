using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

[RequireComponent(typeof(Touch_Action))]
public class Touchable : MonoBehaviour
{
    public Touch_Action _touchComponent;

    // usage values
    public bool OneTimeUse;
    public bool HasACooldown;

    private bool _usedSuccesfully;
    private bool _onCooldown;
    [SerializeField]
    private float _cooldownLength;

    // components any tap-able would have
    [SerializeField]
    private Animator _animator; // all
    [SerializeField]
    private Collider _collider; // all

    // above this is base
    private float _animationPopDuration;

    // etc
    public LayerMask LayersToCastOn; // should perhaps be the same as gamemanager layers



    #region Unity Functions

    private void Start()
    {
        // for update method de-activation
        this.enabled = false;
    }

    #endregion

    #region Public Functions

    // logic for what should happen when this gets tapped
    public void PlayTapEvent()
    {
        if (OneTimeUse == false || OneTimeUse == true && _usedSuccesfully == false)
        {
            if (_onCooldown == false)
            {
                // calls the acting function on the Touch_Action
                _touchComponent.Act();

                // plays animation (only if my touch component is not the "Move")
                if (_animator != null && GetComponent<Touch_Move>() == null)
                {
                    _animator.SetTrigger("Activate");
                }
           
                // if cooldown is present
                if (HasACooldown == true)
                {
                    StartCoroutine(ActivateCooldown());
                }

                _usedSuccesfully = true;
            }
        }
    }

    #endregion



    public IEnumerator ActivateCooldown()
    {
        _onCooldown = true;
        _collider.enabled = false;

        yield return new WaitForSeconds(_cooldownLength);

        _onCooldown = false;
        _collider.enabled = true;
    }
    private IEnumerator DisableAnimationComponent()
    {
        yield return new WaitForSeconds(_animationPopDuration);

        //_animation.enabled = false;
    }

}
