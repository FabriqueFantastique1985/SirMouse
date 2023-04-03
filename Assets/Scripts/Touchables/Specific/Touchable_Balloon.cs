using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable_Balloon : Touchable
{
    //[Header("Balloon Collection parent")]
    //[SerializeField]
    //private BalloonCollection _balloonCollection;

    //// when I'm clicked, destroys random balloon
    //protected override void ExtraLogic()
    //{
    //    base.ExtraLogic();

    //    if (_balloonCollection != null)
    //    {
    //        int randomBalloon = Random.Range(0, _balloonCollection.MyTouchableBalloons.Count);

    //        StartCoroutine(DestroyBalloon(randomBalloon));
    //    }      
    //}


    //IEnumerator DestroyBalloon(int chosenIndex)
    //{
    //    Touchable_Balloon balloonToDestroy = _balloonCollection.MyTouchableBalloons[chosenIndex];

    //    yield return new WaitForSeconds(1f);

    //    _balloonCollection.MyTouchableBalloons.Remove(balloonToDestroy);

    //    Destroy(balloonToDestroy.gameObject);
    //}
}
