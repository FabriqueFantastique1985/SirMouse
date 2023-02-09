using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class ButtonBaseNew : MonoBehaviour, IClickable
{
    [SerializeField]
    private Animation _animationComponent;
    [SerializeField]
    protected string _animationName;

    [SerializeField]
    protected AudioElement _soundEffect;
    protected AudioController _audioInstance;

    protected virtual void Start()
    {
        _audioInstance = AudioController.Instance;
        if (_soundEffect.Clip != null)
        {
            _audioInstance.AddAudioElement(_soundEffect);
        }
    }

    public virtual void ClickedButton()
    {
        PlayAnimationPress();
        PlaySoundEffect();
    }

    protected virtual void PlaySoundEffect()
    {
        _audioInstance.PlayAudio(_soundEffect);
    }


    public void PlayAnimationPress()
    {
        _animationComponent.Stop();
        _animationComponent.Play(_animationName);
    }

    public void Click(Player player)
    {
        ClickedButton();
    }
}
