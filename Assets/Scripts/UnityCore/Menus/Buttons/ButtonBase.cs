using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

namespace UnityCore
{
    namespace Menus
    {
        public class ButtonBase : MonoBehaviour
        {
            [SerializeField]
            protected PageType _turnOnThisPage;

            [SerializeField]
            protected Animator _animatorForThisButton;
            [SerializeField]
            protected string _animationName;
            private bool _animationSwapped; // bool and methodology used so "Animator.Play" can interrupt its own animation

            [SerializeField]
            protected AudioElement _soundEffectOn;
            [SerializeField]
            protected AudioElement _soundEffectOff;


            private void Start()
            {
                var audioControl = AudioController.Instance;
                // add the possible sound effects to the AudioTable and the correct track
                if (_soundEffectOn.Clip != null)
                {
                    // there exists 1 Type more than there are Tracks -> move down by 1
                    audioControl.AddAudioElement(_soundEffectOn, ((int)_soundEffectOn.Type) - 1);
                }
                if (_soundEffectOff.Clip != null)
                {
                    // there exists 1 Type more than there are Tracks -> move down by 1
                    audioControl.AddAudioElement(_soundEffectOff, ((int)_soundEffectOff.Type) - 1);
                }
            }

            public virtual void Clicked()
            {
                if (PageController.Instance.PageIsOn(_turnOnThisPage) == true)
                {
                    PageController.Instance.TurnPageOff(_turnOnThisPage);
                    AudioController.Instance.PlayAudio(_soundEffectOff.Clip, _soundEffectOff.Type);
                }
                else
                {
                    PageController.Instance.TurnPageOn(_turnOnThisPage);
                    AudioController.Instance.PlayAudio(_soundEffectOn.Clip, _soundEffectOn.Type);
                }

                if (_animatorForThisButton != null && _animationName != string.Empty)
                {
                    if (_animationSwapped == false)
                    {
                        _animatorForThisButton.Play(_animationName);
                        _animationSwapped = true;
                    }
                    else
                    {
                        _animatorForThisButton.Play(_animationName + "_1"); // this is added to the duplicatee animation state in the controller
                        _animationSwapped = false;
                    }                   
                }              
            }
        }
    }
}

