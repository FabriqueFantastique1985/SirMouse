using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionClosetAdd : Interaction
{
    [SerializeField]
    private SkinType _skinType;

    public GameObject SkinObjectWithSrite;
    public SpriteRenderer SkinSpriteRenderer;

    [Header("In case the skin can be on 2 locations")]
    [SerializeField]
    private bool _2TransformsPossible;
    [SerializeField]
    private SkinType _skinTypeRight;

    private string _nameSpriteObject;

    [SerializeField]
    private SkinTransform _transformForSirMouse;
    [SerializeField]
    private SkinTransform _transformForSirMouseRight;



    private void Start()
    {
        // adjust the SkinObjectWithSprite name accordingly
        SkinObjectWithSrite.name = SkinSpriteRenderer.sprite.name;
        _nameSpriteObject = SkinObjectWithSrite.name;

    }


    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        ClosetController.Instance.StartCoroutine(ClosetController.Instance.ForceObjectInCloset(this, SkinObjectWithSrite.transform.localScale.x));
        ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SkinObjectWithSrite.transform.parent.gameObject));

        //SkinController.Instance.StartCoroutine(SkinController.Instance.ForceObjectInCloset(this, SkinObjectWithSrite.transform.localScale.x));
        //SkinController.Instance.StartCoroutine(SkinController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SkinObjectWithSrite.transform.parent.gameObject));

        //AddToLists();
    }

    public void AddToLists()
    {
        SkinController.Instance.AddSkinPieceToCloset(_skinType, SkinObjectWithSrite, _nameSpriteObject, _transformForSirMouse);
        if (_2TransformsPossible == true)
        {
            SkinController.Instance.AddSkinPieceToCloset(_skinTypeRight, SkinObjectWithSrite, _nameSpriteObject, _transformForSirMouseRight);
        }
    }
}
