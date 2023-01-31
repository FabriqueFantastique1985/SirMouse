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
    private Rigidbody _rigidbody;

    void Start()
    {
        _animation.enabled = false;

        float randomOffset = Random.Range(0.0f, _ramdomOffsetUpperLimit);
        StartCoroutine(ActivateAnimation(randomOffset));
    }

    private IEnumerator ActivateAnimation(float randomTime)
    {
        yield return new WaitForSeconds(randomTime);

        _animation.enabled = true;
        _animation.Play();
    }
}