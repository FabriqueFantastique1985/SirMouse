using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private bool _sitting = false;
    [SerializeField]
    private bool _backSide = false;
    [SerializeField]
    private int _cheeringVariations;
    [SerializeField]
    private float _minSpeed = .7f;
    [SerializeField]
    private float _maxSpeed = 1.3f;

    [SerializeField]
    private bool _cheerWithCrowd;
    [SerializeField]
    private bool _angryWithCrowd;


    // Animator parameters


    // Triggers
    private const string IDLE = "Idle";
    private const string CHEER = "Cheer";
    private const string ANGRY = "Angry";

    // Bools
    private const string SITTING = "Sitting";
    private const string BACKSIDE = "BackSide";

    private const string IDLE_STATE = "IDLE_STATE";
    private const string CHEERING_STATE = "CHEERING_STATE";

    // Floats
    private const string ANIMATION_INDEX = "AnimationIndex";
    private const string ANIMATION_SPEED = "AnimationSpeed";
    private const string START_TIME = "StartTime";

    private void Awake()
    {
        if (_sitting == false)
        {
            _animator.SetLayerWeight(1, 1f);
        }

        PlayIdleAnimation();
    }

    public void PlayIdleAnimation()
    {        
        _animator.SetBool(SITTING, _sitting);
        _animator.SetBool(BACKSIDE, _backSide);
        _animator.SetFloat(START_TIME, Random.Range(0f, .5f));
        _animator.SetFloat(ANIMATION_SPEED, Random.Range(_minSpeed,_maxSpeed));
        _animator.SetTrigger(IDLE);
    }
    public void PlayIdleAnimation(float delayUpperLimit)
    {
        float delaySeconds = Random.Range(0f, delayUpperLimit);
        StartCoroutine(PlayIdleAnimationDelay(delaySeconds));
    }

    IEnumerator PlayIdleAnimationDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayIdleAnimation();
    }

    public void PlayCheerAnimation()
    {
        if (!_animator.GetBool(IDLE_STATE) || !_animator.GetBool(CHEERING_STATE) && _cheerWithCrowd == true)
        {
            _animator.SetBool(SITTING, _sitting);
            _animator.SetBool(BACKSIDE, _backSide);
            _animator.SetInteger(ANIMATION_INDEX, Random.Range(0, _cheeringVariations));
            _animator.SetFloat(START_TIME, Random.Range(0f, .5f));
            _animator.SetFloat(ANIMATION_SPEED, Random.Range(_minSpeed, _maxSpeed));
            _animator.SetTrigger(CHEER);
        }
    }

    public void PlayCheerAnimation(float delayUpperLimit)
    {
        float delaySeconds = Random.Range(0f, delayUpperLimit);
        StartCoroutine(PlayCheerAnimationDelay(delaySeconds));
    }

    IEnumerator PlayCheerAnimationDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayCheerAnimation();
    }

    public void PlayAngryAnimation()
    {
        if (!_animator.GetBool(IDLE_STATE) || !_animator.GetBool(CHEERING_STATE))
        {
            _animator.SetBool(SITTING, _sitting);
            _animator.SetBool(BACKSIDE, _backSide);
            _animator.SetInteger(ANIMATION_INDEX, Random.Range(0, _cheeringVariations)); // needs to be angry variation
            _animator.SetFloat(START_TIME, Random.Range(0f, .5f));
            _animator.SetFloat(ANIMATION_SPEED, Random.Range(_minSpeed, _maxSpeed));
            _animator.SetTrigger(ANGRY);
        }
    }

    public void PlayAngryAnimation(float delayUpperLimit)
    {
        float delaySeconds = Random.Range(0f, delayUpperLimit);
        StartCoroutine(PlayAngryAnimationDelay(delaySeconds));
    }

    IEnumerator PlayAngryAnimationDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayAngryAnimation();
    }
}