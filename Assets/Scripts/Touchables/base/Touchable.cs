using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Touch_Action))]
public class Touchable : MonoBehaviour, IClickable
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

    [FormerlySerializedAs("_animator")] public Animator Animator;     
    public Collider Collider; 
    public ParticleSystem ParticleGlowy;


    private float _animationPopDuration;

    public LayerMask LayersToCastOn; // should perhaps be the same as gamemanager layers

    // audio // specifically for animation events
    [Header("Audio")]
    public List<AudioElement> AudioElements = new List<AudioElement>();

    #region Unity Functions


    public void Disable()
    {
        Collider.enabled = false;
        ParticleGlowy.Stop();
        Animator.SetBool("Activated", false);
    }
    public void Enable()
    {
        Collider.enabled = true;
        ParticleGlowy.Play();
        Animator.SetBool("Activated", true);
    }
    
    protected virtual void Start()
    {
        //Disable();
    }

    #endregion

    #region Public Functions

    // logic for what should happen when this gets tapped
    public void Click(Player player)
    {
        if (OneTimeUse == false || OneTimeUse == true && UsedSuccesfully == false)
        {
            if (_onCooldown == false)
            {
                GameManager.Instance.BlockInput = true;

                // calls the acting function on the Touch_Action
                _touchComponent.Act();

                // plays animation (only if my touch component is not the "Move")
                TriggerAnimation();
           
                // if cooldown is present
                if (HasACooldown == true)
                {
                    StartCoroutine(ActivateCooldown());
                }

                UsedSuccesfully = true;
            }
        }
    }

    // play animator animation through activate trigger
    protected virtual void TriggerAnimation()
    {
        // plays animation (only if my touch component is not the "Move")
        if (Animator != null && GetComponent<Touch_Move>() == null)
        {
            Animator.SetTrigger("Activate");
        }
    }

    // play animation event
    public void PlayAnimationEventSound(int index)
    {
        AudioController.Instance.PlayAudio(AudioElements[index]);
    }

    #endregion



    public IEnumerator ActivateCooldown()
    {
        _onCooldown = true;
        Collider.enabled = false;

        yield return new WaitForSeconds(_cooldownLength);

        _onCooldown = false;
        Collider.enabled = true;
    }
    private IEnumerator DisableAnimationComponent()
    {
        yield return new WaitForSeconds(_animationPopDuration);

        //_animation.enabled = false;
    }

}
