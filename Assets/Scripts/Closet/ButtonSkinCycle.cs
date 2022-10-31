using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkinCycle : MonoBehaviour
{
    public SkinType _skinType;

    public void Clicked()
    {
        SkinController.Instance.CycleSkinPiece(_skinType, SkinOrientation.None);
    }
}
