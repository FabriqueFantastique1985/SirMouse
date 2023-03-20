using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPodium : ButtonBaseNew
{
    public delegate void ButtonPodiumDelegate(ButtonPodium button);
    public event ButtonPodiumDelegate OnButtonClicked;

    [SerializeField]
    private string _animationTrigger;
    private Animator _playerAnimator;

    public Animator PlayerAnimator
    {
        set { _playerAnimator = value; }
    }

    public override void ClickedButton()
    {
        base.ClickedButton();

        OnButtonClicked?.Invoke(this);
    }

    public void PlayAnimation()
    {
        _playerAnimator?.SetTrigger(_animationTrigger);
    }
}
