﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "ScriptableObjects/Tutorial")]
public class TutorialData : ScriptableObject
{
    private bool _isTutorialFinished = false;
    public bool IsTutorialFinished
    {
        get { return _isTutorialFinished; }
        set 
        { 
            _isTutorialFinished = value;
            // TODO: S: save this variable
        }
    }

    [field: SerializeField] private GameObject _tutorialObject;
    public GameObject TutorialObject
    {
        get { return _tutorialObject; }
        private set { _tutorialObject = value; }
    }
}
