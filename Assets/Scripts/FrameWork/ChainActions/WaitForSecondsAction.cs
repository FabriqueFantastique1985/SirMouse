using System.Collections;
using UnityEngine;

public class WaitForSecondsAction : ChainActionMonoBehaviour
{
    [SerializeField] private float _seconds;

    private void Awake()
    {
        _startMaxTime = _seconds;
    }
}