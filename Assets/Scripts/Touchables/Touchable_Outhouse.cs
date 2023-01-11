using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable_Outhouse : Touchable
{
    [SerializeField]
    private SpriteRenderer _bottleSprite;

    [SerializeField]
    private List<Sprite> _possibleBottleSprites = new List<Sprite>();


    // animation event
    public void RandomizeBottleSprite()
    {
        var random = Random.Range(0, _possibleBottleSprites.Count - 1);

        _bottleSprite.sprite = _possibleBottleSprites[random];
    }
}
