using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WaitAction : ChainActionMonoBehaviour
{
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private bool _disableInput = false;

    private void Start()
    {
        _startMaxTime = _waitTime;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = _disableInput;
    }

    public override void OnExit()
    {
        base.OnExit();
        GameManager.Instance.BlockInput = false;
    }

}
