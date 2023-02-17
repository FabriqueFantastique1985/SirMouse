using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SkinPieceElement : MonoBehaviour
{
    public Type_Body MyBodyType;
    public Type_Skin MySkinType;

    public bool HidesSirMouseGeometry;

    public SkinPieceElement(Type_Body bodyType, Type_Skin skinType, bool hideGeo)
    {
        MyBodyType = bodyType;
        MySkinType = skinType;
        HidesSirMouseGeometry = hideGeo;
    }
}
