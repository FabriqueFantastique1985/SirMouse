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
    public string lastActiveScene = "";
    
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

    /// <summary>
    /// Intended for only 1 mirror in the whole game
    /// </summary>
    public bool HasRecievedMirrorReward = false;

    /// <summary>
    /// Saves all touchables that drop gatherables through their ID and the amount already collected by the player
    /// </summary>
    public Dictionary<string, int> DroppedGatherable = new Dictionary<string, int>();

    /// <summary>
    /// Saves the type of resource the player has in their inventory and the amount of them
    /// </summary>
    public Dictionary<Type_Resource, int> ResourcesCollected = new Dictionary<Type_Resource, int>();

    // These should be initial values to start with 
    public GameData()
    {
        
    }
}