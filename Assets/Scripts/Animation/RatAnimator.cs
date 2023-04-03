using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimator : MonoBehaviour
{
    [SerializeField]
    private string _animationCommand;
    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        _animator.SetBool(_animationCommand, true);
    }
}
