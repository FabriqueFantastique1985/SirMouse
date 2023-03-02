using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPodium : ButtonBaseNew
{
    public delegate void ButtonPodiumDelegate();
    public event ButtonPodiumDelegate OnButtonClicked;

    public override void ClickedButton()
    {
        base.ClickedButton();

        OnButtonClicked?.Invoke();
    }
}
