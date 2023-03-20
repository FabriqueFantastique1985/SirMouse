﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinsMouseController : MonoBehaviour
{
    public static SkinsMouseController Instance { get; private set; }

    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;
    
    public Animator ClosetWrapInsideCamera;

    //[HideInInspector]
    public List<SkinPieceElement> EquipedSkinPieces = new List<SkinPieceElement>();
    public List<SkinPieceElement> EquipedSkinPiecesUI = new List<SkinPieceElement>(); // iterate over this list

    Dictionary<Type_Body, int> _bodyPiecesScore = new Dictionary<Type_Body, int>();

    #region SkinPiecesFields

    [Header("Skins on player Rig")]
    // below scripts are present on the correct transform in the SirMouse Rig
    public SkinPiecesForThisBodyType SkinPiecesHat;
    public SkinPiecesForThisBodyType SkinPiecesHead;
    public SkinPiecesForThisBodyType SkinPiecesChest;
    public SkinPiecesForThisBodyType SkinPiecesArmLeft;
    public SkinPiecesForThisBodyType SkinPiecesArmRight;
    public SkinPiecesForThisBodyType SkinPiecesLegLeft;
    public SkinPiecesForThisBodyType SkinPiecesLegRight;
    public SkinPiecesForThisBodyType SkinPiecesFootLeft;
    public SkinPiecesForThisBodyType SkinPiecesFootRight;
    public SkinPiecesForThisBodyType SkinPiecesTail;
    public SkinPiecesForThisBodyType SkinPiecesSword;
    public SkinPiecesForThisBodyType SkinPiecesShield;
    //

    [Header("Skins on player Rig in UI")]
    // below scripts are present on the correct transform in the SirMouse Rig
    public SkinPiecesForThisBodyType SkinPiecesUIHat;
    public SkinPiecesForThisBodyType SkinPiecesUIHead;
    public SkinPiecesForThisBodyType SkinPiecesUIChest;
    public SkinPiecesForThisBodyType SkinPiecesUIArmLeft;
    public SkinPiecesForThisBodyType SkinPiecesUIArmRight;
    public SkinPiecesForThisBodyType SkinPiecesUILegLeft;
    public SkinPiecesForThisBodyType SkinPiecesUILegRight;
    public SkinPiecesForThisBodyType SkinPiecesUIFootLeft;
    public SkinPiecesForThisBodyType SkinPiecesUIFootRight;
    public SkinPiecesForThisBodyType SkinPiecesUITail;
    public SkinPiecesForThisBodyType SkinPiecesUISword;
    public SkinPiecesForThisBodyType SkinPiecesUIShield;
    //

    [Header("Skins on buttons in UI closet")]
    // below scripts are present on the buttons in the closet
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonHat;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonHead;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonChest;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonArmLeft;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonArmRight;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonLegLeft;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonLegRight;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonFootLeft;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonFootRight;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonTail;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonSword;
    public SkinPiecesForThisBodyTypeButton SkinPiecesButtonShield;
    //

    // complete list for easy iteration on cheat code
    private List<SkinPiecesForThisBodyTypeButton> _listsOfButtons = new List<SkinPiecesForThisBodyTypeButton>();

    #endregion


    // possible to put this onto seperate ScoreController
    #region ScoreFields
    [HideInInspector]
    public int ScoreTotal;
    #endregion


    private void Awake()
    {
        // Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _listsOfButtons.Add(SkinPiecesButtonHat);
        _listsOfButtons.Add(SkinPiecesButtonHead);
        _listsOfButtons.Add(SkinPiecesButtonChest);
        _listsOfButtons.Add(SkinPiecesButtonArmLeft);
        _listsOfButtons.Add(SkinPiecesButtonArmRight);
        _listsOfButtons.Add(SkinPiecesButtonLegLeft);
        _listsOfButtons.Add(SkinPiecesButtonLegRight);
        _listsOfButtons.Add(SkinPiecesButtonFootLeft);
        _listsOfButtons.Add(SkinPiecesButtonFootRight);
        _listsOfButtons.Add(SkinPiecesButtonTail);
        _listsOfButtons.Add(SkinPiecesButtonSword);
        _listsOfButtons.Add(SkinPiecesButtonShield);
    }

    private void Start()
    {
        _bodyPiecesScore.Clear();
        for (int i = 1; i < System.Enum.GetValues(typeof(Type_Body)).Length; ++i)
        {
            _bodyPiecesScore.Add((Type_Body)i, 0);
        }
    }

    #region Public Functions
    // called on the interaction add...
    public ButtonSkinPiece UnlockSkinPiece(SkinPieceElement skinPieceElement)
    {
        SkinPiecesForThisBodyTypeButton skinPieceForBodyX = null;

        switch (skinPieceElement.MyBodyType)
        {
            case Type_Body.None:
                skinPieceForBodyX = null;
                break;
            case Type_Body.Hat:
                skinPieceForBodyX = SkinPiecesButtonHat;
                break;
            case Type_Body.Head:
                skinPieceForBodyX = SkinPiecesButtonHead;
                break;
            case Type_Body.Chest:
                skinPieceForBodyX = SkinPiecesButtonChest;
                break;
            case Type_Body.ArmLeft:
                skinPieceForBodyX = SkinPiecesButtonArmLeft;
                break;
            case Type_Body.ArmRight:
                skinPieceForBodyX = SkinPiecesButtonArmRight;
                break;
            case Type_Body.LegLeft:
                skinPieceForBodyX = SkinPiecesButtonLegLeft;
                break;
            case Type_Body.LegRight:
                skinPieceForBodyX = SkinPiecesButtonLegRight;
                break;
            case Type_Body.FootLeft:
                skinPieceForBodyX = SkinPiecesButtonFootLeft;
                break;
            case Type_Body.FootRight:
                skinPieceForBodyX = SkinPiecesButtonFootRight;
                break;
            case Type_Body.Tail:
                skinPieceForBodyX = SkinPiecesButtonTail;
                break;
            case Type_Body.Sword:
                skinPieceForBodyX = SkinPiecesButtonSword;
                break;
            case Type_Body.Shield:
                skinPieceForBodyX = SkinPiecesButtonShield;
                break;
        }

        return FindCorrectSkinPieceButton(skinPieceForBodyX, skinPieceElement);
    }
    public ButtonSkinPiece UnlockAllSkins()
    { 
        for (int i = 0; i < _listsOfButtons.Count; i++)
        {
            for (int j = 0; j < _listsOfButtons[i].MySkinPiecesButtons.Count; j++)
            {
                // activate the button
                _listsOfButtons[i].MySkinPiecesButtons[j].Found = true;
                // enable the sprite over the sillhouette on the button
                _listsOfButtons[i].MySkinPiecesButtons[j].MySpriteToActivateWhenFound.SetActive(true);
            }
        }

        // always return the same button for the popup       
        return FindCorrectSkinPieceButton(SkinPiecesButtonSword, SkinPiecesButtonSword.MySkinPiecesButtons[1].MySkinPieceElement);
    }

    // called when a piece is dragged onto SirMouse...
    public void EquipSkinPiece(SkinPieceElement skinPieceElement)
    {
        SkinPiecesForThisBodyType skinPieceForBodyX = null;
        SkinPiecesForThisBodyType skinPieceForBodyUIX = null;

        switch (skinPieceElement.MyBodyType)
        {
            case Type_Body.None:
                skinPieceForBodyX = null;
                skinPieceForBodyUIX = null;
                break;
            case Type_Body.Hat:
                skinPieceForBodyX = SkinPiecesHat;
                skinPieceForBodyUIX = SkinPiecesUIHat;
                break;
            case Type_Body.Head:
                skinPieceForBodyX = SkinPiecesHead;
                skinPieceForBodyUIX = SkinPiecesUIHead;
                break;
            case Type_Body.Chest:
                skinPieceForBodyX = SkinPiecesChest;
                skinPieceForBodyUIX = SkinPiecesUIChest;
                break;
            case Type_Body.ArmLeft:
                skinPieceForBodyX = SkinPiecesArmLeft;
                skinPieceForBodyUIX = SkinPiecesUIArmLeft;
                break;
            case Type_Body.ArmRight:
                skinPieceForBodyX = SkinPiecesArmRight;
                skinPieceForBodyUIX = SkinPiecesUIArmRight;
                break;
            case Type_Body.LegLeft:
                skinPieceForBodyX = SkinPiecesLegLeft;
                skinPieceForBodyUIX = SkinPiecesUILegLeft;
                break;
            case Type_Body.LegRight:
                skinPieceForBodyX = SkinPiecesLegRight;
                skinPieceForBodyUIX = SkinPiecesUILegRight;
                break;
            case Type_Body.FootLeft:
                skinPieceForBodyX = SkinPiecesFootLeft;
                skinPieceForBodyUIX = SkinPiecesUIFootLeft;
                break;
            case Type_Body.FootRight:
                skinPieceForBodyX = SkinPiecesFootRight;
                skinPieceForBodyUIX = SkinPiecesUIFootRight;
                break;
            case Type_Body.Tail:
                skinPieceForBodyX = SkinPiecesTail;
                skinPieceForBodyUIX = SkinPiecesUITail;
                break;
            case Type_Body.Sword:
                skinPieceForBodyX = SkinPiecesSword;
                skinPieceForBodyUIX = SkinPiecesUISword;
                break;
            case Type_Body.Shield:
                skinPieceForBodyX = SkinPiecesShield;
                skinPieceForBodyUIX = SkinPiecesUIShield;
                break;
        }

        // disable other equiped skin pieces for bodyTypeX
        UnEquipOtherSkinPieces(skinPieceElement.MyBodyType);
        // find correct skin piece to equip for bodyTypeX
        FindCorrectSkinPieceRig(skinPieceForBodyX, skinPieceForBodyUIX, skinPieceElement);
    }
    #endregion



    #region Private Functions
    private ButtonSkinPiece FindCorrectSkinPieceButton(SkinPiecesForThisBodyTypeButton skinPiecesForBodyX, SkinPieceElement skinPieceElement)
    {
        for (int i = 0; i < skinPiecesForBodyX.MySkinPiecesButtons.Count; i++)
        {
            if (skinPiecesForBodyX.MySkinPiecesButtons[i].MySkinPieceElement.MySkinType == skinPieceElement.MySkinType)
            {
                // activate the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].Found = true;
                // enable the sprite over the sillhouette on the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].MySpriteToActivateWhenFound.SetActive(true);

                //return skinPiecesForBodyX.MySkinPiecesButtons[i].MySpriteToActivateWhenFound;
                return skinPiecesForBodyX.MySkinPiecesButtons[i];
            }
        }
        return null;
    }
    private void FindCorrectSkinPieceRig(SkinPiecesForThisBodyType skinPiecesForBodyX, SkinPiecesForThisBodyType skinPiecesForBodyUIX, SkinPieceElement skinPieceElement)
    {
        // should first add all of the pieces == skinType to temp lists 
        // (we iterate over every single skinPiece on BodyTypeX, so i'd better seperate additional lines of code for smaller temp lists)
        List<SkinPieceElement> tempListToClear = new List<SkinPieceElement>();
        List<SkinPieceElement> tempListToClearUI = new List<SkinPieceElement>();
        for (int i = 0; i < skinPiecesForBodyX.MySkinPieces.Count; i++)
        {
            // find corresponding skinTypes in the list of skinPieces (will not enter the if, if I'm using default --- check tempList size count 0)
            if (skinPiecesForBodyX.MySkinPieces[i].MySkinType == skinPieceElement.MySkinType)
            {
                tempListToClear.Add(skinPiecesForBodyX.MySkinPieces[i]);
                tempListToClearUI.Add(skinPiecesForBodyUIX.MySkinPieces[i]);

                // link the score that was set on the Closet
                skinPiecesForBodyX.MySkinPieces[i].ScoreValue = skinPieceElement.ScoreValue; 
                skinPiecesForBodyUIX.MySkinPieces[i].ScoreValue = skinPieceElement.ScoreValue; // 1 of these can be removed (as long as we check correct list for score)
            }
        }

        // indicating that we're trying to equip a Sir Mouse default skinPiece
        if (tempListToClear.Count == 0)
        {
            SetSirMouseGeometryState(skinPieceElement.MyBodyType, true);
        }

        // iterate through the temp lists
        for (int i = 0; i < tempListToClear.Count; i++)
        {
            // this logic is what applies the skins on the player character
            tempListToClear[i].gameObject.SetActive(true);
            // this logic is what applies the skins on the Closet mouse
            tempListToClearUI[i].gameObject.SetActive(true);

            Debug.Log("Equiping piece " + tempListToClear[i]);

            // add to list of equiped skinpieces
            EquipedSkinPieces.Add(tempListToClear[i]);
            EquipedSkinPiecesUI.Add(tempListToClearUI[i]);

            // de-activate old geo if Hide == true
            if (tempListToClear[i].HidesSirMouseGeometry == true)
            {
                SetSirMouseGeometryState(skinPieceElement.MyBodyType, false);
            }
            else
            {
                SetSirMouseGeometryState(skinPieceElement.MyBodyType, true);
            }
        }

        DebugConsoleScore();

        tempListToClear.Clear();
        tempListToClearUI.Clear();
    }
    private void UnEquipOtherSkinPieces(Type_Body bodyTypeToCheck)
    {
        // 1) iterate through equiped pieces, 2) find corresponding bodytypes
        List<SkinPieceElement> tempListToClear = new List<SkinPieceElement>();
        List<SkinPieceElement> tempListToClearUI = new List<SkinPieceElement>();
        for (int i = 0; i < EquipedSkinPieces.Count; i++)
        {
            if (EquipedSkinPieces[i].MyBodyType == bodyTypeToCheck)
            {
                tempListToClear.Add(EquipedSkinPieces[i]);
                tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
            }
        }

        // 3) set these to in-active, 4) remove from main list   
        for (int i = 0; i < tempListToClear.Count; i++)
        {
            tempListToClear[i].gameObject.SetActive(false);
            tempListToClearUI[i].gameObject.SetActive(false);

            EquipedSkinPieces.Remove(tempListToClear[i]);
            EquipedSkinPiecesUI.Remove(tempListToClearUI[i]);
        }

        tempListToClear.Clear();
        tempListToClearUI.Clear();
    }

    // makes old geometry visible/invisible
    private void SetSirMouseGeometryState(Type_Body bodyType, bool state) // false = invisible
    {
        switch (bodyType)
        {
            case Type_Body.None:
                break;
            case Type_Body.Hat:
                characterGeoReferences.EarL.gameObject.SetActive(state);
                characterGeoReferences.EarR.gameObject.SetActive(state);
                // ui
                characterGeoReferencesUI.EarL.gameObject.SetActive(state);
                characterGeoReferencesUI.EarR.gameObject.SetActive(state);
                break;
            case Type_Body.Head:
                characterGeoReferences.Head.gameObject.SetActive(state);
                // ui
                characterGeoReferencesUI.Head.gameObject.SetActive(state);
                break;
            case Type_Body.Chest:
                characterGeoReferences.Chest.gameObject.SetActive(state);
                characterGeoReferences.Skirt.gameObject.SetActive(state);
                
                characterGeoReferencesUI.Chest.gameObject.SetActive(state);
                characterGeoReferencesUI.Skirt.gameObject.SetActive(state);
                break;
            case Type_Body.ArmLeft:
                characterGeoReferences.ArmUpL.gameObject.SetActive(state);
                characterGeoReferences.HandL.gameObject.SetActive(state);
                characterGeoReferences.ElbowL.gameObject.SetActive(state);
                characterGeoReferences.ShoulderL.gameObject.SetActive(state);
                // ui
                characterGeoReferencesUI.ArmUpL.gameObject.SetActive(state);
                characterGeoReferencesUI.HandL.gameObject.SetActive(state);
                characterGeoReferencesUI.ElbowL.gameObject.SetActive(state);
                characterGeoReferencesUI.ShoulderL.gameObject.SetActive(state);
                break;
            case Type_Body.ArmRight:
                characterGeoReferences.ArmUpR.gameObject.SetActive(state);
                characterGeoReferences.HandR.gameObject.SetActive(state);
                characterGeoReferences.ElbowR.gameObject.SetActive(state);
                characterGeoReferences.ShoulderR.gameObject.SetActive(state);
                
                characterGeoReferencesUI.ArmUpR.gameObject.SetActive(state);
                characterGeoReferencesUI.HandR.gameObject.SetActive(state);
                characterGeoReferencesUI.ElbowR.gameObject.SetActive(state);
                characterGeoReferencesUI.ShoulderR.gameObject.SetActive(state);
                break;
            case Type_Body.LegLeft:
                characterGeoReferences.LegUpL.gameObject.SetActive(state);
                characterGeoReferences.KneeL.gameObject.SetActive(state);
                characterGeoReferences.LegLowL.gameObject.SetActive(state);
                
                characterGeoReferencesUI.LegUpL.gameObject.SetActive(state);
                characterGeoReferencesUI.KneeL.gameObject.SetActive(state);
                characterGeoReferencesUI.LegLowL.gameObject.SetActive(state);
                break;
            case Type_Body.LegRight:
                characterGeoReferences.LegUpR.gameObject.SetActive(state);
                characterGeoReferences.KneeR.gameObject.SetActive(state);
                characterGeoReferences.LegLowR.gameObject.SetActive(state);
                
                characterGeoReferencesUI.LegUpR.gameObject.SetActive(state);
                characterGeoReferencesUI.KneeR.gameObject.SetActive(state);
                characterGeoReferencesUI.LegLowR.gameObject.SetActive(state);
                break;
            case Type_Body.FootLeft:
                characterGeoReferences.FootL.gameObject.SetActive(state);
                
                characterGeoReferencesUI.FootL.gameObject.SetActive(state);
                break;
            case Type_Body.FootRight:
                characterGeoReferences.FootR.gameObject.SetActive(state);
                
                characterGeoReferencesUI.FootR.gameObject.SetActive(state);
                break;
            case Type_Body.Tail:
                characterGeoReferences.Tail.gameObject.SetActive(state);
                
                characterGeoReferencesUI.Tail.gameObject.SetActive(state);
                break;
            case Type_Body.Sword:
                characterGeoReferences.Sword.gameObject.SetActive(state);

                characterGeoReferencesUI.Sword.gameObject.SetActive(state);
                break;
            case Type_Body.Shield:
                characterGeoReferences.Shield.gameObject.SetActive(state);

                characterGeoReferencesUI.Shield.gameObject.SetActive(state);
                break;

        }
    }


    private void DebugConsoleScore()
    {
        int totalScore = 0;

        // Set correct score for each body part in dictionary
        for (int i = 0; i < EquipedSkinPieces.Count; i++)
        {
            _bodyPiecesScore[EquipedSkinPieces[i].MyBodyType] = EquipedSkinPieces[i].ScoreValue;
        }

        // Loop over dictionary and add to total score
        foreach (var item in _bodyPiecesScore)
        {
            totalScore += item.Value;
        }

        ScoreTotal = totalScore;

        //Debug.Log("Total Score is " + ScoreTotal);
    }

    #endregion
}
