using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartTime : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;

    [Range(0f, 1f)]
    public float _ramdomOffsetUpperLimit = 1f;
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        float randomOffset = Random.Range(0.0f, _ramdomOffsetUpperLimit);
        randomOffset = _animation.clip.length * randomOffset;

        if (_animation != null)
        {
            _animation[_animation.clip.name].time = randomOffset;
        }

        if (_animator != null)
        {
            _animator.SetFloat("StartTime", Mathf.Clamp(randomOffset,0.0f, 1.0f));
        }

    }
}