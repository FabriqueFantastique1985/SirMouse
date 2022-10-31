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
                    audioControl.AddAudioElement(_soundEffectOn);
                }
                if (_soundEffectOff.Clip != null)
                {
                    // there exists 1 Type more than there are Tracks -> move down by 1
                    audioControl.AddAudioElement(_soundEffectOff);
                }
            }

            public virtual void Clicked()
            {
                if (PageController.Instance.PageIsOn(_turnOnThisPage) == true)
                {
                    PageController.Instance.TurnPageOff(_turnOnThisPage);
                    ExtraLogicPageOff();
                    AudioController.Instance.PlayAudio(_soundEffectOff);
                }
                else
                {
                    PageController.Instance.TurnPageOn(_turnOnThisPage);                 
                    ExtraLogicPageOn();
                    AudioController.Instance.PlayAudio(_soundEffectOn);
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


            public virtual void ExtraLogicPageOn()
            {

            }
            public virtual void ExtraLogicPageOff()
            {

            }
        }
    }
}

