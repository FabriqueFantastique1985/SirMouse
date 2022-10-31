using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionClosetAdd : Interaction
{
    [SerializeField]
    private SkinType _skinType;

    public GameObject SkinObjectWithSrite;
    public SpriteRenderer SkinSpriteRenderer;

    private string _nameSpriteObject;


    // adjust the SkinObjectWithSprite name accordingly
    private void Start()
    {
        SkinObjectWithSrite.name = SkinSpriteRenderer.sprite.name;
        _nameSpriteObject = SkinObjectWithSrite.name;
    }



    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        SkinController.Instance.StartCoroutine(SkinController.Instance.ForceObjectInCloset(this.gameObject, SkinObjectWithSrite.transform.localScale.x));
        SkinController.Instance.AddSkinPieceToCloset(_skinType, SkinObjectWithSrite, _nameSpriteObject);
    }
}
