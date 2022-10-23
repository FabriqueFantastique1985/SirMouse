using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    public Touch_Action _touchComponent;

    // usage values
    public bool OneTimeUse;
    public bool HasACooldown;
    [HideInInspector]
    public bool UsedSuccesfully;
    private bool _onCooldown;
    [SerializeField]
    private float _cooldownLength;

    // components any tap-able would have
    [SerializeField]
    private Animator _animator; // all
    [SerializeField]
    private Collider _collider; // all

    // above this is base

    private const string _animPop = "Spawnable_Pop";
    private const string _animIdle = "Spawnable_Scaler";
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
        if (OneTimeUse == false || OneTimeUse == true && UsedSuccesfully == false)
        {
            if (_onCooldown == false)
            {
                // calls the acting function on the Touch_Action
                _touchComponent.Act();
               
                // if cooldown is present
                if (HasACooldown == true)
                {
                    StartCoroutine(ActivateCooldown());
                }
            }
        }
    }

    #endregion
    //if (CanBeMoved == true) // if true, enables update
    //{
    //    _activatedFollowMouse = true;
    //    this.enabled = true;
    //}
    //// checks for spawned object and animations

    //if (SpawnsAPrefab == true)
    //{
    //    SpawnedObjecLogic();
    //}
    //else
    //{
    //}










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
