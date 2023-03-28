using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBaseNew : MonoBehaviour, IClickable
{
    [Header("Animation stuff")]
    [SerializeField]
    protected Animation _animationComponent;
    [SerializeField]
    protected string _animationName;

    [Header("Audio")]
    [SerializeField]
    protected AudioElement _soundEffect;
    protected AudioController _audioInstance;

    protected virtual void Start()
    {
        _audioInstance = AudioController.Instance;   
    }

    public virtual void ClickedButton()
    {
        PlayAnimationPress();
        PlaySoundEffect();
    }

    protected virtual void PlaySoundEffect()
    {
        if (_soundEffect != null)
        {
            _audioInstance.PlayAudio(_soundEffect);
        }
    }
    protected virtual void PlaySoundEffectMultiple(AudioElement audioElement)
    {
        if (audioElement != null)
        {
            _audioInstance.PlayAudio(audioElement);
        }
    }


    public void PlayAnimationPress()
    {
        if (_animationComponent != null)
        {
            _animationComponent.Stop();
            _animationComponent.Play(_animationName);
        }
    }

    public virtual void Click(Player player)
    {
        ClickedButton();
    }
}
