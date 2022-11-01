using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityCore.Menus;
using UnityEngine;

public class ButtonSkinCycle : ButtonBaseNew
{
    public SkinType _skinType;

    public override void ClickedButton()
    {
        base.ClickedButton();

        SkinController.Instance.CycleSkinPiece(_skinType);
    }
}
