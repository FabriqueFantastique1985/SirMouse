using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractBalloon : MonoBehaviour
{
    #region Events

    public delegate void BalloonDelegate(InteractBalloon sender, Player player);
    
    /// <summary>
    /// Gets invoked when clicked on.
    /// </summary>
    public event BalloonDelegate OnBalloonClicked;

    #endregion


    public void Click(Player player)
    {
        OnBalloonClicked?.Invoke(this, player);
    }
}
