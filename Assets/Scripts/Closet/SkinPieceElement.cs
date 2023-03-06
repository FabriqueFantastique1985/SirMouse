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
        ScoreValue = Mathf.Clamp(ScoreValue, 0, 10);
    }
}
