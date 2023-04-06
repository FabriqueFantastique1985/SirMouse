using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableCanvasAction : ChainActionMonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _image;

    protected virtual void Start()
    {
        _startMaxTime = .1f;
        //_image.enabled = false;
        _canvas.gameObject.SetActive(true);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.Instance.BlockInput = true;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        //_image.enabled = true;
        GameManager.Instance.BlockInput = false;
    }
}
