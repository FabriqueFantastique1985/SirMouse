
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityCore
{
    namespace Menus
    {
        public class Page : MonoBehaviour
        {
            //public static readonly string FLAG_ON = "On";
            //public static readonly string FLAG_OFF = "Off";
            //public static readonly string FLAG_NONE = "None";

            public static PageState FLAG_ON = PageState.On;
            public static PageState FLAG_OFF = PageState.Off;
            public static PageState FLAG_NONE = PageState.None;


            public PageType Type;
            public bool useAnimation;
            public PageState TargetState { get; private set; } // can be publicly got, privately set

            private Animator m_Animator;

            private bool m_IsOn;
            public bool isOn
            {
                get
                {
                    return m_IsOn;
                }
                private set
                {
                    m_IsOn = value;
                }
            }

            #region Unity Functions

            private void OnEnable()
            {
                CheckAnimatorIntegrity();
            }

            #endregion



            #region Public Functions

            public void Animate(bool on)
            {
                if (useAnimation)
                {
                    m_Animator.SetBool("On", on);

                    StopCoroutine(AwaitAnimation(on));
                    StartCoroutine(AwaitAnimation(on));
                }
                else
                {
                    if (on == false)
                    {
                        gameObject.SetActive(false);
                        isOn = false;
                    }
                    else
                    {
                        isOn = true;
                    }
                }
            }

            #endregion



            #region Private Functions

            private IEnumerator AwaitAnimation(bool on)
            {
                if (on == true)
                {
                    TargetState = FLAG_ON;
                }
                else
                {
                    TargetState = FLAG_OFF;
                }

                // wait for animator to reach target state
                while (m_Animator.GetCurrentAnimatorStateInfo(0).IsName(TargetState.ToString()) == false)  // if my current state is the state I want to be in...
                {
                    yield return null;
                }

                // wait for animator to finish animating
                while (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    yield return null;
                }

                TargetState = FLAG_NONE;

                Debug.Log("Page [" + Type + "] finished transitioning to " + (on ? "On" : "Off"));

                if (on == false)
                {
                    isOn = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    isOn = true;
                }
            }

            private void CheckAnimatorIntegrity()
            {
                if (useAnimation)
                {
                    m_Animator = GetComponent<Animator>();
                    if (m_Animator == null)
                    {
                        Debug.Log("You opted to animate a page [" + Type + "], but no animator component exists on the object.");
                    }
                }
            }

            #endregion
        }
    }
}

