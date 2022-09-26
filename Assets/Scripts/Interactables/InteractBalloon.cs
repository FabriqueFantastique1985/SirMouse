using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractBalloon : MonoBehaviour
{
    #region Events

    public delegate void BalloonDelegate(InteractBalloon sender);
    
    /// <summary>
    /// Gets invoked when clicked on.
    /// </summary>
    public event BalloonDelegate OnBalloonClicked;

    #endregion


    public void Click()
    {
        OnBalloonClicked?.Invoke(this);
    }
    
    
    
}
