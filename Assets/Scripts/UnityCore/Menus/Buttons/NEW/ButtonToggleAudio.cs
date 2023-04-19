using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonToggleAudio : MonoBehaviour, IClickable
{
    [SerializeField]
    private bool _influenceOST;

    [Header("Animation stuff")]
    [SerializeField]
    protected Animation _animationComponent;
    [SerializeField]
    protected string _animationNameTurnOff;
    [SerializeField]
    protected string _animationNameTurnOn;

    [Header("Audio")]
    [SerializeField]
    protected AudioElement _soundEffectClick;
    protected AudioController _audioInstance;

    private bool _buttonToggledOff;

    protected virtual void Start()
    {
        _audioInstance = AudioController.Instance;
    }

    public virtual void ClickedButton()
    {
        PlayAnimationPress();
        PlaySoundEffect();

        _buttonToggledOff = !_buttonToggledOff;

        // checks whether audio controller should mute or default the respective audio
        AdjustAudio();
    }

    protected virtual void PlaySoundEffect()
    {
        if (_soundEffectClick != null)
        {
            _audioInstance.PlayAudio(_soundEffectClick);
        }
    }
    public void PlayAnimationPress()
    {
        if (_animationComponent != null)
        {
            if (_buttonToggledOff == false)
            {
                _animationComponent.Play(_animationNameTurnOff);
            }
            else
            {
                _animationComponent.Play(_animationNameTurnOn);
            }
        }
    }
    private void AdjustAudio()
    {
        if (_buttonToggledOff == false) // if the button is toggled on -> default corresponding audio
        {
            if (_influenceOST)
            {
                _audioInstance.InfluenceVolumeOST(_buttonToggledOff);

                Debug.Log("Turned ON music");
            }
            else
            {
                _audioInstance.InfluenceVolumeFX(_buttonToggledOff);

                Debug.Log("Turned ON sfx");
            }

        }
        else
        {
            if (_influenceOST)
            {
                _audioInstance.InfluenceVolumeOST(_buttonToggledOff);

                Debug.Log("Turned OFF music");
            }
            else
            {
                _audioInstance.InfluenceVolumeFX(_buttonToggledOff);

                Debug.Log("Turned OFF sfx");
            }
        }
    }

    public virtual void Click(Player player)
    {
        ClickedButton();
    }
}