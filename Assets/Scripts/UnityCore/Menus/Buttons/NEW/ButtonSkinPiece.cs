using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkinPiece : ButtonBaseNew
{
    //
    //public bool HidesSirMouseGeometry;
    //public Type_Body MyBodyType;
    //public Type_Skin MySkinType;
    //
    public SkinPieceElement MySkinPieceElement;

    public bool Found;

    public GameObject MySpriteToActivateWhenFound;
    public GameObject MySpriteToDuplicateAndMove;

    public override void ClickedButton()
    {
        if (Found == true)
        {
            // anim + sound on base
            base.ClickedButton();

            // create copy of skinPiece

        }
    }
}
