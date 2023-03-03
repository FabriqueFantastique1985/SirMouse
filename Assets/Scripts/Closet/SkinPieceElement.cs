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

    [Header("To Assign only in Closet Buttons")]
    public int ScoreValue;

    private void Awake()
    {
        if (ScoreValue < 0)
            ScoreValue = 0;
        if (ScoreValue > 10)
            ScoreValue = 10;
    }
}
