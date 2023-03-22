using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable_Balloon : Touchable
{
    [Header("Balloon Collection parent")]
    [SerializeField]
    private BalloonCollection _balloonCollection;

    protected override void ExtraLogic()
    {
        base.ExtraLogic();

        if (_balloonCollection != null)
        {
            StartCoroutine(DestroyBalloon());
        }      
    }


    IEnumerator DestroyBalloon()
    {
        yield return new WaitForSeconds(1f);

        Destroy(_balloonCollection.gameObject);
    }
}
