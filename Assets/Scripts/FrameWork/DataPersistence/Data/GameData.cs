using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameData
{
    /// <summary>
    /// The last scene the player was in.
    /// </summary>
    public string lastActiveScene;
    
    /// <summary>
    /// Indices for every minigame. 
    /// </summary>
    public Dictionary<string, int> MinigamesIndices = new Dictionary<string, int>();

    /// <summary>
    /// Equiped skin pieces RM is wearing. 
    /// </summary>
    public List<SkinPieceElementData> EquipedSkinPiecesData = new List<SkinPieceElementData>();
    public List<ButtonSkinPieceData> ButtonsSkinPieceData = new List<ButtonSkinPieceData>();

    public Dictionary<string, MerchantData> MerchantSavedData = new Dictionary<string, MerchantData>();
    public Dictionary<string, InstrumentInteractableData> InstrumentInteractionSavedData = new Dictionary<string, InstrumentInteractableData>();

    public Dictionary<string, bool> IsTutorialComplete = new Dictionary<string, bool>(); 

    /// <summary>
    /// string is the id of the gatherable and bool is if it has been gathered or not.
    /// </summary>
    public Dictionary<string, bool> GatherableData = new Dictionary<string, bool>();
    
    /// <summary>
    /// Type_Instrument is the type of instrument and bool is if it has been unlocked or not.
    /// </summary>
    public Dictionary<Type_Instrument, bool> InstrumentData = new Dictionary<Type_Instrument, bool>();
    public Dictionary<string, SlotResource.SlotResourceData> SlotResourceDatas = new Dictionary<string, SlotResource.SlotResourceData>();

    /// <summary>
    /// Intended for only 1 mirror in the whole game
    /// </summary>
    public bool HasRecievedMirrorReward = false;

    public Dictionary<string, int> DroppedGatherable = new Dictionary<string, int>();

    public Dictionary<string, bool> CleanedTouchables = new Dictionary<string, bool>();

    public Dictionary<string, bool> GatherablePuzzlePieces = new Dictionary<string, bool>();
    public bool IsPuzzleCompletedOnce;

    public int CurrentPuzzleImage;

    // These should be initial values to start with 
    public GameData()
    {
        
    }
}