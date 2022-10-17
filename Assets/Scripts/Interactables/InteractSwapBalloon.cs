using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSwapBalloon : MonoBehaviour
{
    #region Events

    public delegate void SwapBalloonDelegate(InteractSwapBalloon sender, Player player);

    /// <summary>
    /// Gets invoked when clicked on.
    /// </summary>
    public event SwapBalloonDelegate OnSwapBalloonClicked;

    #endregion


    public void Click(Player player)
    {
        OnSwapBalloonClicked?.Invoke(this, player);
    }
}
