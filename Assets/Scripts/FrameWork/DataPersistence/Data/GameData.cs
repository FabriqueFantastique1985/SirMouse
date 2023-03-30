using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    //private Dictionary<string, Vector3> _interactables = new Dictionary<string, Vector3>();

    public int MiniGameStepIndex = 0;

    public Dictionary<TutorialData, bool> _isTutorialComplete = new Dictionary<TutorialData, bool>(); 

    // These should be initial values to start with 
    public GameData()
    {
        
    }
}
