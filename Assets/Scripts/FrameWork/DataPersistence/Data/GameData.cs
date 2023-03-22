﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{

    public int MiniGameStepIndex = 0;

    public Dictionary<string, int> MinigamesIndices = new Dictionary<string, int>();
    public Dictionary<string, bool> ResourcesPickedUp = new Dictionary<string, bool>();
    public Dictionary<string, bool> InstrumentsPickedUp = new Dictionary<string, bool>();

    /// <summary>
    /// Equiped skin pieces RM is wearing. 
    /// </summary>
    public List<SkinPieceElement> EquipedSkinPieces = new List<SkinPieceElement>();
    public List<SkinPiecesForThisBodyTypeButton> ListsOfButtons = new List<SkinPiecesForThisBodyTypeButton>();

   // public Dictionary<string, List<ObjectData>> InteractablesPerScene = new Dictionary<string, List<ObjectData>>();

    // These should be initial values to start with 
    public GameData()
    {
        
    }
}

/*public class ObjectData
{
    public Interactable;
    public Vector3 Position;
    public bool IsActive;
    
}*/

public class SceneData
{
    public Dictionary<string, int> _miniGameStartIndices = new Dictionary<string, int>();
    public List<Interactable> _interactables = new List<Interactable>();
}

public class GlobalGameData
{
    // contains data regarding clothing pieces equipped, clothing pieces unlocked
}
