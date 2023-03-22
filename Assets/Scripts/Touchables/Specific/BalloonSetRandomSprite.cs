using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSetRandomSprite : MonoBehaviour
{
    [SerializeField]
    private bool _randomizeMySprite;

    [SerializeField]
    private SpriteRenderer _balloonSpriteRenderer;
    [SerializeField]
    private List<Sprite> _spritesIRandomizeWithOnStart = new List<Sprite>();


    void Start()
    {
        if (_randomizeMySprite == true)
        {
            _balloonSpriteRenderer.sprite = _spritesIRandomizeWithOnStart[Random.Range(0, _spritesIRandomizeWithOnStart.Count)];
        }
    }
}
