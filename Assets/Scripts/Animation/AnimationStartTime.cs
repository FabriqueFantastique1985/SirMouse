using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartTime : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private float _ramdomOffsetUpperLimit = 2.0f;
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        float randomOffset = Random.Range(0.0f, _ramdomOffsetUpperLimit);

        if (_animation != null)
        {
            _animation.enabled = false;
            StartCoroutine(ActivateAnimation(randomOffset));
        }

        if (_animator != null)
        {
            _animator.SetFloat("StartTime", Mathf.Clamp(randomOffset,0.0f, 1.0f));
        }

    }

    private IEnumerator ActivateAnimation(float randomTime)
    {
        yield return new WaitForSeconds(randomTime);

        _animation.enabled = true;
        _animation.Play();
    }
}