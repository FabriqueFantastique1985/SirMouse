using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldCamera : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (!_canvas.worldCamera)
        {
            _canvas.worldCamera = Camera.allCameras[0];
        }
    }
}
