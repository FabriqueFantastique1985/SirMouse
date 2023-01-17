using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyLogic : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteToInfluence;

    private bool _becomeTransparent;
    private bool _becomeNormal;


    void Update()
    {
        if (_becomeTransparent == true)
        {
            _spriteToInfluence.color = Color.Lerp(_spriteToInfluence.color, new Color(1f, 1f, 1f, 0.5f), 0.1f);

            if (_spriteToInfluence.color.a <= 0.55f)
            {
                _spriteToInfluence.color = new Color(1f, 1f, 1f, 0.5f);
                this.enabled = false;
            }
        }
        else if (_becomeNormal == true)
        {
            _spriteToInfluence.color = Color.Lerp(_spriteToInfluence.color, new Color(1f, 1f, 1f, 1f), 0.1f);

            if (_spriteToInfluence.color.a >= 0.95f)
            {
                _spriteToInfluence.color = new Color(1f, 1f, 1f, 1f);
                this.enabled = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        this.enabled = true;
        _becomeNormal = false;
        _becomeTransparent = true;
    }

    private void OnTriggerExit(Collider other)
    {
        this.enabled = true;
        _becomeNormal = true;
        _becomeTransparent = false;
    }
}
