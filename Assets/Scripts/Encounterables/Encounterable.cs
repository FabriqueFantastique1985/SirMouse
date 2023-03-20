using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Encounterable : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Animator _animator;

    [Header("Cooldown Related")]
    [SerializeField]
    protected bool _oneTimeUse;
    [SerializeField]
    protected bool _hasACooldown;
    [SerializeField]
    private float _cooldownLength;

    protected bool _onCooldown;
    protected bool _usedSuccesfully;

    [SerializeField]
    private bool _giveRandomOffsetAnimationIdle;
    [SerializeField]
    private float _randomOffsetUpperLimit = 2.0f;

    [Header("Audio")]
    public AudioElement SoundEffect;
    [SerializeField]
    private AudioClip[] _audioClips;

    #endregion


    #region Unity Functions

    protected virtual void Start()
    {
        //var audioControl = AudioController.Instance;
        //// add the possible sound effect to the AudioTable and the correct track (OLD logic)    
        //if (SoundEffect.Clip != null)
        //{
        //    // there exists 1 Type more than there are Tracks -> move down by 1
        //    audioControl.AddAudioElement(SoundEffect); 
        //}

        if (_giveRandomOffsetAnimationIdle == true)
        {
            _animator.enabled = false;

            float randomOffset = Random.Range(0.0f, _randomOffsetUpperLimit);
            StartCoroutine(ActivateAnimatorOffsetted(randomOffset));
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // use layers so it only detects player entering
        if (_oneTimeUse == false || _oneTimeUse == true && _usedSuccesfully == false)
        {
            // check for cooldown
            if (_onCooldown == false)
            {
                GenericBehaviour();

                // if cooldown is present
                if (_hasACooldown == true)
                {
                    StartCoroutine(ActivateCooldown());
                }
            }
        }
    }

    #endregion



    #region Virtual Functions

    protected virtual void GenericBehaviour()
    {
        if (_animator != null)
        {
            _animator.enabled = true;
            _animator.SetTrigger("Activate");  // !!! create this trigger on the animators !!!        
        }

        // also play sound effect
        if (SoundEffect.Clip != null)
        {
            if (_audioClips.Length != 0)
            {
                SoundEffect.Clip = _audioClips[Random.Range(0, _audioClips.Length)];
            }
            AudioController.Instance.PlayAudio(SoundEffect);
        }
        
        _usedSuccesfully = true;
    }

    #endregion



    #region Private Functions

    protected IEnumerator ActivateCooldown()
    {
        _onCooldown = true;

        yield return new WaitForSeconds(_cooldownLength);

        _onCooldown = false;
    }

    protected IEnumerator ActivateAnimatorOffsetted(float randomTime)
    {
        yield return new WaitForSeconds(randomTime);

        if (_animator != null)
        {
            _animator.enabled = true;
        }
    }

    #endregion
}
