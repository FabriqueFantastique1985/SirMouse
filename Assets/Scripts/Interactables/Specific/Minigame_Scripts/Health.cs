using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animation MyAnimationComponent;

    public Sprite SpriteHeadNormal;
    public Sprite SpriteHeadExploded;

    public SpriteRenderer SpriteRenderer;

    public Animator ParticleExplosion;


    public void PlayExplosion()
    {
        ParticleExplosion.gameObject.SetActive(true);
        ParticleExplosion.SetTrigger("Activate");
    }

    public void ResetExplosion()
    {
        ParticleExplosion.gameObject.SetActive(false);
    }
}
