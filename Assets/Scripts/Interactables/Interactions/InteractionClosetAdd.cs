using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SkinPieceElement))]
public class InteractionClosetAdd : Interaction
{
    [Header("Skin Things")]

    //[SerializeField]
    //private SkinPieceElement _mySkinPieceElement;

    [SerializeField]
    private List<SkinPieceElement> _mySkinPieceElements;

    public GameObject SpriteParent;
    [FormerlySerializedAs("SkinObjectWithSrite")] public GameObject SkinObjectWithSprite;

    public SpriteRenderer SkinSpriteRenderer; // this not needed anymore ?

    protected override void SpecificAction(Player player)
    {
        base.SpecificAction(player);

        if (SpriteParent != null)
        {
            ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SpriteParent));
        }
        else
        {
            ClosetController.Instance.StartCoroutine(ClosetController.Instance.SetObjectToFalseAfterDelay(this.gameObject, SkinObjectWithSprite.transform.parent.gameObject));
        }

        //SkinsMouseController.Instance.UnlockSkinPiece(_mySkinPieceElement);

        if (_mySkinPieceElements.Count == 0)
        {
            Debug.Log("'SkinPieceElement' reference missing");
        }
        else
        {
            RewardController.Instance.GiveReward(_mySkinPieceElements); 
        }
        
    }
}
