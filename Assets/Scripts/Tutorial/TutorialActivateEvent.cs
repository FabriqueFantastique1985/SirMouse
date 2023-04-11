using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivateEvent : MonoBehaviour
{
    public delegate void TutorialActivateDelegate();
    public event TutorialActivateDelegate OnTutorialActivate;

    protected void ActivateTutorial()
    {
        OnTutorialActivate?.Invoke();
    }

    public virtual void TutorialActivated()
    {

    }
}
