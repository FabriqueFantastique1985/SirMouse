using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_TransformGO : Touch_Action
{
    [SerializeField] private GameObject _base;
    [SerializeField] private Collider _baseCollider;
    [SerializeField] private GameObject _second;
    [SerializeField] private Collider _secondCollider;

    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isBase;

    private void Awake()
    {
        _isBase = true;
    }

    protected override void Start()
    {
        base.Start();

        _base.SetActive(true);
        _second.SetActive(false);
        _touchableScript.Collider = _baseCollider;
        _secondCollider.enabled = false;
    }

    public override void Act()
    {
        base.Act();

        _particleSystem.Play();

        // Toggle between base and second sprite and collider
        if (_isBase)
        {
            _base.SetActive(false);
            _second.SetActive(true);
            _touchableScript.Collider = _secondCollider;
            _baseCollider.enabled = false;

            _isBase = false;
        }
        else
        {
            _base.SetActive(true);
            _second.SetActive(false);
            _touchableScript.Collider = _baseCollider;
            _secondCollider.enabled = false;

            _isBase = true;
        }
    }
}
