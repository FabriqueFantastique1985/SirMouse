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
    private bool _oneTimeUse;
    [SerializeField]
    private bool _hasACooldown;
    [SerializeField]
    private float _cooldownLength;

    private bool _onCooldown;
    private bool _usedSuccesfully;

    [Header("Audio")]
    public AudioElement AudioEM;

    #endregion


    #region Unity Functions

    private void Start()
    {
        var audioControl = AudioController.Instance;

        // add the possible sound effect to the AudioTable and the correct track       
        if (AudioEM.Clip != null)
        {
            // there exists 1 Type more than there are Tracks -> move down by 1
            audioControl.AddAudioElement(AudioEM, ((int)AudioEM.Type) - 1); 
        }
    }

    private void OnTriggerEnter(Collider other)
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
        AudioController.Instance.PlayAudio(AudioEM.Clip, AudioEM.Type);
    }

    #endregion



    #region Private Functions

    private IEnumerator ActivateCooldown()
    {
        _onCooldown = true;

        yield return new WaitForSeconds(_cooldownLength);

        _onCooldown = false;
    }

    #endregion







}
