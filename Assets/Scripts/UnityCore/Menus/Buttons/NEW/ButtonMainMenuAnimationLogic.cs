using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMainMenuAnimationLogic : MonoBehaviour
{
    [SerializeField]
    private Animation _animationComponent;

    [SerializeField]
    private string _animationAppear;

    private void OnEnable()
    {
        if (_animationAppear != string.Empty)
        {
            _animationComponent.Play(_animationAppear);
        }    
    }
}
