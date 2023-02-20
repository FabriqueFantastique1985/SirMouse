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
        // if this is a skinPiece I have found...
        if (Found == true)  
        {
            // if I don't already have a skinPiece on my finger...
            if (ClosetController.Instance.ActivatedFollowMouse == false)
            {
                // anim + sound on base
                base.ClickedButton();

                // create copy of skinPiece
                ClosetController.Instance.ClickedSkinPieceButton(MySpriteToDuplicateAndMove, MySkinPieceElement, this.transform.position);
            }

        }
    }

    public override void Click(Player player)
    {
        base.Click(player);
    }
}
