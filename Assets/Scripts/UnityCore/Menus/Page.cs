
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

            #region Unity Functions

            private void OnEnable()
            {
                
            }

            #endregion



            #region Public Functions

            public void Animate(bool on)
            {

            }

            #endregion



            #region Private Functions

            private IEnumerator AwaitAnimation(bool on)
            {
                yield return null;
            }

            private void CheckAnimatorIntegrity()
            {

            }

            #endregion
        }
    }
}

