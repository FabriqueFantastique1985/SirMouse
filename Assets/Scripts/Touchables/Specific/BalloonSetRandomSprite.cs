using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSetRandomSprite : MonoBehaviour
{
    [Header("Sprites to assign")]
    [SerializeField]
    private bool _randomizeMySprite;

    [SerializeField]
    private SpriteRenderer _balloonSpriteRenderer;
    [SerializeField]
    private List<Sprite> _spritesIRandomizeWithOnStart = new List<Sprite>();

    [Header("(for BigBalloon logic required)")]
    public ParticleSystem ParticleBreak;

    public GameObject SpriteParent;


    void Start()
    {
        if (_randomizeMySprite == true)
        {
            _balloonSpriteRenderer.sprite = _spritesIRandomizeWithOnStart[Random.Range(0, _spritesIRandomizeWithOnStart.Count)];
        }
    }
}
