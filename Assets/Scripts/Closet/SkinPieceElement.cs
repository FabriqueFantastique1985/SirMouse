using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class SkinPieceElement : MonoBehaviour
{
    public SkinPieceElementData Data;
    
    #region SetScoreValue
    private static int _maxScore = 0;
    public static int MaxScore
    {
        get { return _maxScore; }
        set 
        {
            if (value <= 0 || _maxScore != 0)
                return;
            _maxScore = value;
        }
    }

    private void Start()
    {
        if (_maxScore > 0)
        {
            Data.ScoreValue = Mathf.Clamp(Data.ScoreValue, 0, _maxScore);
        }
    }
    #endregion
}

[Serializable]
public class SkinPieceElementData
{
    public Type_Body MyBodyType;
    public Type_Skin MySkinType;

    public bool HidesSirMouseGeometry;

    [Header("To Assign only in Closet Buttons")]
    public int ScoreValue;
}