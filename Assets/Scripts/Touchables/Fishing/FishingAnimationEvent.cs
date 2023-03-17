using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingAnimationEvent : MonoBehaviour
{
    [SerializeField] private BackAndForth_Continous _fishingHook;
    [SerializeField] private Touch_Event _touchEvent;
    [SerializeField] private Touchable _touchable;

    private void Start()
    {
        _touchable.OneTimeUse = true;
        _fishingHook.OnReachedStart += EnableAnimation;
        _touchEvent.OnPropTouched += DisableAnimation;
    }

    private void OnDestroy()
    {
        _fishingHook.OnReachedStart -= EnableAnimation;
        _touchEvent.OnPropTouched -= DisableAnimation;
    }

    private void EnableAnimation()
    {
        _touchable.UsedSuccesfully = false;
    }
    private void DisableAnimation()
    {
        _touchable.UsedSuccesfully = true;
    }
}
