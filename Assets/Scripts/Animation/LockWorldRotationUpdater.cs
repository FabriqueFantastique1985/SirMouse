using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWorldRotationUpdater : MonoBehaviour
{
    [SerializeField]
    private GameObject _spriteObjectToLock;


    // enable/disable this script when minigame starts/ends

    void Update()
    {
        _spriteObjectToLock.transform.rotation = Quaternion.Euler(30,0,0);

        //Debug.Log(_spriteObjectToLock.transform.rotation);
    }
}
