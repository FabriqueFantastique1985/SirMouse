using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menus;

public class ButtonClosetOpenSpecificPieces : ButtonPaging
{
    public Type_Body BodyType;

    public bool IHaveDuplicateSkinPieces;
    public bool IHaveButtonsWithNotificationOn;
    public List<ButtonSkinPiece> ButtonsWithNotifsOnOnMyPage = new List<ButtonSkinPiece>();

    public GameObject NotificationObject;


    protected override void TurnOnPage()
    {
        ClosetController.Instance.ClickedSkinPiecePageButton(_turnThisPage);

        if (_turnThisPage == PageType.ClosetSwords || _turnThisPage == PageType.ClosetShields)
        {
            // show visuals sword & shield
            SkinsMouseController.Instance.characterGeoReferencesUI.SwordAndShieldMeshParent.SetActive(true);

            for (int i = 0; i < SkinsMouseController.Instance.EquipedSkinPiecesUI.Count; i++)
            {
                if (SkinsMouseController.Instance.EquipedSkinPiecesUI[i].MyBodyType == Type_Body.Sword 
                    || SkinsMouseController.Instance.EquipedSkinPiecesUI[i].MyBodyType == Type_Body.Shield)
                {
                    SkinsMouseController.Instance.EquipedSkinPiecesUI[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            // hide visuals
            SkinsMouseController.Instance.characterGeoReferencesUI.SwordAndShieldMeshParent.SetActive(false);

            for (int i = 0; i < SkinsMouseController.Instance.EquipedSkinPiecesUI.Count; i++)
            {
                if (SkinsMouseController.Instance.EquipedSkinPiecesUI[i].MyBodyType == Type_Body.Sword
                    || SkinsMouseController.Instance.EquipedSkinPiecesUI[i].MyBodyType == Type_Body.Shield)
                {
                    SkinsMouseController.Instance.EquipedSkinPiecesUI[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
