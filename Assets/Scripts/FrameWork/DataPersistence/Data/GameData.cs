using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameData
{
    /// <summary>
    /// Indices for every minigame. 
    /// </summary>
    public Dictionary<string, int> MinigamesIndices = new Dictionary<string, int>();

    /// <summary>
    /// Equiped skin pieces RM is wearing. 
    /// </summary>
    public List<SkinPieceElementData> EquipedSkinPiecesData = new List<SkinPieceElementData>();
    public List<ButtonSkinPieceData> ButtonsSkinPieceData = new List<ButtonSkinPieceData>();

    public Dictionary<int, MerchantData> MerchantData = new Dictionary<int, MerchantData>();

    /// <summary>
    /// string is the id of the gatherable and bool is if it has been gathered or not.
    /// </summary>
    public Dictionary<string, bool> GatherableData = new Dictionary<string, bool>();
    
    // These should be initial values to start with 
    public GameData()
    {
        
    }
}