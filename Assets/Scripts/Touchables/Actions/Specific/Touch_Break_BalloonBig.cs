using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Break_BalloonBig : Touch_Action
{
    [SerializeField]
    private GameObject _spriteParent;

    [SerializeField]
    private List<BalloonSetRandomSprite> _balloonChildren = new List<BalloonSetRandomSprite>();

    public override void Act()
    {
        // pickup audio is the one which will be called on this one !
        base.Act();

        if (_balloonChildren.Count > 0)
        {
            var randomBalloonindex = Random.Range(0, _balloonChildren.Count);
            var randomBalloon = _balloonChildren[randomBalloonindex];

            // remove chosen child from list (instant)
            _balloonChildren.Remove(randomBalloon);

            // activate particle of chosen balloon child
            randomBalloon.ParticleBreak.Play();
            // disable sprite of chosen balloon child
            randomBalloon.SpriteParent.SetActive(false);

            // check if this was the last balloon
            if (_balloonChildren.Count < 0)
            {
                this.gameObject.SetActive(false);
            }
        }

    }
}
