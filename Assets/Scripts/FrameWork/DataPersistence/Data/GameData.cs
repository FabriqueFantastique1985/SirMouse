using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    public List<SkinPieceElementData> EquipedSkinPiecesData = new List<SkinPieceElementData>();
    public List<ButtonSkinPieceData> ButtonsSkinPieceData = new List<ButtonSkinPieceData>();

    public Dictionary<string, MerchantData> MerchantSavedData = new Dictionary<string, MerchantData>();
    public Dictionary<string, InstrumentInteractableData> InstrumentInteractionSavedData = new Dictionary<string, InstrumentInteractableData>();

    public Dictionary<string, bool> IsTutorialComplete = new Dictionary<string, bool>(); 

    // These should be initial values to start with 
    public GameData()
    {
        
    }
}