using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable_Balloon : Touchable
{
    [Header("Parent with Rigidbody if present")]
    [SerializeField]
    private Rigidbody _rigidBody;

    protected override void ExtraLogic()
    {
        base.ExtraLogic();

        if (_rigidBody != null)
        {
            StartCoroutine(DestroyBalloon());
        }      
    }


    IEnumerator DestroyBalloon()
    {
        yield return new WaitForSeconds(1f);

        Destroy(_rigidBody.gameObject);
    }
}
