using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RatCrowdAnimatorController : MonoBehaviour
{
    [SerializeField]
    private List<RatAnimatorController> _ratAnimatorControllers;

    public void PlayIdleAnimations()
    {
        for (int i = 0; i < _ratAnimatorControllers.Count; i++)
        {
            _ratAnimatorControllers[i].PlayIdleAnimation(.8f);
        }
    }

    public void PlayCheerAnimations()
    {
        for (int i = 0; i < _ratAnimatorControllers.Count; i++)
        {
            _ratAnimatorControllers[i].PlayCheerAnimation(.8f);
        }
    }

    public void PlayAngryAnimations()
    {
        for (int i = 0; i < _ratAnimatorControllers.Count; i++)
        {
            _ratAnimatorControllers[i].PlayAngryAnimation(.8f);
        }
    }
}