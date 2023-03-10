using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Transform : Touch_Action
{
    [SerializeField] private Sprite _base;
    [SerializeField] private Collider _baseCollider;
    [SerializeField] private Sprite _second;
    [SerializeField] private Collider _secondCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _isBase;

    private void Awake()
    {
        _spriteRenderer.sprite = _base;
        _isBase = true;
    }

    protected override void Start()
    {
        base.Start();
        _touchableScript.Collider = _baseCollider;
        _secondCollider.enabled = false;
    }

    public override void Act()
    {
        base.Act();

        if (_isBase)
        {
            _spriteRenderer.sprite = _second;
            _touchableScript.Collider = _secondCollider;
            _baseCollider.enabled = false;
            _isBase = false;
        }
        else
        {
            _spriteRenderer.sprite = _base;
            _touchableScript.Collider = _baseCollider;
            _secondCollider.enabled = false;
            _isBase = true;
        }
    }

}
