using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menus;

public class ButtonClosetOpenSpecificPieces : ButtonPaging
{

    protected override void TurnOnPage()
    {
        ClosetController.Instance.ClickedSkinPiecePageButton(_turnThisPage);
    }
}
