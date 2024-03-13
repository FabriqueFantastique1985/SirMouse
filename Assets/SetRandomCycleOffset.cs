using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomCycleOffset : StateMachineBehaviour
{
    [SerializeField]
    private float _minCycleOffset = 0.0f;
    [SerializeField]
    private float _maxCycleOffset = 1.0f;
    [SerializeField]
    private string _cycleParameterName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(_cycleParameterName, Random.Range(_minCycleOffset, _maxCycleOffset));
    }
}
