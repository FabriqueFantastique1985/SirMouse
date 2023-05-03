using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrWitchAnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private const string IDLE_TRIGGER = "Idle";

    public void TriggerIdleAnimation()
    {
        _animator.SetTrigger(IDLE_TRIGGER);
    }
}
