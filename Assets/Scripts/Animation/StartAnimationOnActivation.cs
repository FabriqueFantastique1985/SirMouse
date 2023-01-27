using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationOnActivation : MonoBehaviour
{
    [SerializeField]
    private Animation _myAnimationComponent;

    private void OnEnable()
    {
        _myAnimationComponent.Play();
    }
}
