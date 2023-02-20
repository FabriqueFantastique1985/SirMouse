using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinPieceElement))]
public class InteractionClosetAdd : Interaction
{
    [Header("Skin Things")]

    [SerializeField]
    private SkinPieceElement _mySkinPieceElement;

    public GameObject SkinObjectWithSrite;
    public SpriteRenderer SkinSpriteRenderer;

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        ClosetController.Instance.StartCoroutine(ClosetController.Instance.ForceObjectInCloset(this, SkinObjectWithSrite.transform.localScale.x));
        ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SkinObjectWithSrite.transform.parent.gameObject));

        SkinsMouseController.Instance.UnlockSkinPiece(_mySkinPieceElement);
    }
}
