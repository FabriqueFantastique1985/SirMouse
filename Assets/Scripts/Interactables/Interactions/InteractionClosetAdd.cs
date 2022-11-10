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

    [Header("Transform on SirMouse")]
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    [Header("Transform on SirMouse Right")]
    public Vector3 PositionR;
    public Vector3 RotationR;
    public Vector3 ScaleR;

    private SkinTransform _transformForSirMouse;
    private SkinTransform _transformForSirMouseRight;

    private void Start()
    {
        // adjust the SkinObjectWithSprite name accordingly
        SkinObjectWithSrite.name = SkinSpriteRenderer.sprite.name;
        _nameSpriteObject = SkinObjectWithSrite.name;

        StoreTransformValuesSkinPiece();
    }

    private void StoreTransformValuesSkinPiece()
    {
        _transformForSirMouse = new SkinTransform(Position, Rotation, Scale);

        if (_2TransformsPossible == true)
        {
            _transformForSirMouseRight = new SkinTransform(PositionR, RotationR, ScaleR);
        }
    }



    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        SkinController.Instance.StartCoroutine(SkinController.Instance.ForceObjectInCloset(this, SkinObjectWithSrite.transform.localScale.x));
        SkinController.Instance.StartCoroutine(SkinController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SkinObjectWithSrite.transform.parent.gameObject));

        AddToLists();
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
