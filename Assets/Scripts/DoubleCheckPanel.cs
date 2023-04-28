using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCheckPanel : MonoBehaviour
{
    [SerializeField]
    private Animation _animationComponent;

    private void OnEnable()
    {
        _animationComponent.Play();
    }
}
