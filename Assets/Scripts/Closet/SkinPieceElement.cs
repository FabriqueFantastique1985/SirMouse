using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class SkinPieceElement : MonoBehaviour
{
    public SkinPieceElementData Data;
    
    public bool HidesSirMouseGeometry;

    public bool ShowUpper;
    public bool ShowLower;

    [Header("To Assign only in Closet Buttons")]
    public int ScoreValue;
    
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
            ScoreValue = Mathf.Clamp(ScoreValue, 0, _maxScore);
        }

        //foreach (Type_Body bodyType in Enum.GetValues(typeof(Type_Body)))
        //{
        //    ShouldHideGeometry.Add(new HideGeometry(bodyType, true));
        //}
    }
    #endregion
}

[Serializable]
public class HideGeometry
{
    public HideGeometry(Type_Body bodyType, bool shouldBeHidden)
    {
        BodyType = bodyType;
        ShouldBeHidden = shouldBeHidden;
    }

    public Type_Body BodyType;
    public bool ShouldBeHidden = true;
}

[Serializable]
public class SkinPieceElementData
{
    public Type_Body MyBodyType;
    public Type_Skin MySkinType;
}