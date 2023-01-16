using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationActivatingObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToDisable, _objectToEnable;


    public void ExchangeSpriteWithObject()
    {
        _objectToEnable.SetActive(true);
        _objectToDisable.SetActive(false);      
    }
}
