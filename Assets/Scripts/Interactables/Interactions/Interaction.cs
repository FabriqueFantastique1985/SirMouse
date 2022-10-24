using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private Sprite _interactionSprite;

    [SerializeField]
    private GameObject _spriteObject;

    public Sprite InteractionSprite => _interactionSprite;

    public GameObject SpriteObject => _spriteObject;
    
    //[SerializeField]
    //private GameObject _interactionSprite;

    //public GameObject InteractionSprite => _interactionSprite;

    public virtual void Execute()
    {
        Debug.Log("Interaction Executed");
    }
}
