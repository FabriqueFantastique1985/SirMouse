using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkinPiece : ButtonBaseNew
{
    public SkinPieceElement MySkinPieceElement;

    public GameObject MySpriteToActivateWhenFound;
    public GameObject MySpriteToDuplicateAndMove;

    [Header("status")]
    public bool Found;


    public override void ClickedButton()
    {
        if (Found == true)  
        {
            // anim + sound on base
            base.ClickedButton();

            // create copy of skinPiece
            ClosetController.Instance.ClickedSkinPieceButton(MySpriteToDuplicateAndMove, MySkinPieceElement);
        }
    }

    public override void Click(Player player)
    {
        base.Click(player);
    }
}
