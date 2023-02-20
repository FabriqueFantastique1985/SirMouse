using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsMouseController : MonoBehaviour
{
    public static SkinsMouseController Instance { get; private set; }

    public Animator ClosetWrapInsideCamera;

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
    //

    //[HideInInspector]
    public List<SkinPieceElement> EquipedSkinPieces = new List<SkinPieceElement>();
    public List<SkinPieceElement> EquipedSkinPiecesUI = new List<SkinPieceElement>();

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
    }


    #region Public Functions
    // called on the interaction add...
    public void UnlockSkinPiece(Type_Body bodyType, Type_Skin skinType)
    {
        SkinPiecesForThisBodyTypeButton skinPieceForBodyX = null;

        switch (bodyType)
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
        }

        FindCorrectSkinPieceButton(skinPieceForBodyX, skinType, bodyType);
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
        }

        // disable other equiped skin pieces for bodyTypeX
        UnEquipOtherSkinPieces(skinPieceElement.MyBodyType);
        // find correct skin piece to equip for bodyTypeX
        FindCorrectSkinPieceRig(skinPieceForBodyX, skinPieceForBodyUIX, skinPieceElement);
    }
    #endregion



    #region Private Functions
    private void FindCorrectSkinPieceButton(SkinPiecesForThisBodyTypeButton skinPiecesForBodyX, Type_Skin skinType, Type_Body bodyType)
    {
        for (int i = 0; i < skinPiecesForBodyX.MySkinPiecesButtons.Count; i++)
        {
            if (skinPiecesForBodyX.MySkinPiecesButtons[i].MySkinPieceElement.MySkinType == skinType)
            {
                // activate the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].Found = true;
                // enable the sprite over the sillhouette on the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].MySpriteToActivateWhenFound.SetActive(true);

                break;
            }
        }
    }
    private void FindCorrectSkinPieceRig(SkinPiecesForThisBodyType skinPiecesForBodyX, SkinPiecesForThisBodyType skinPiecesForBodyUIX, SkinPieceElement skinPieceElement)
    {
        // should first add all of the pieces == skinType to temp lists 
        // (we iterate over every single skinPiece on BodyTypeX, so i'd better seperate additional lines of code for smaller temp lists)
        List<SkinPieceElement> tempListToClear = new List<SkinPieceElement>();
        List<SkinPieceElement> tempListToClearUI = new List<SkinPieceElement>();
        for (int i = 0; i < skinPiecesForBodyX.MySkinPieces.Count; i++)
        {
            if (skinPiecesForBodyX.MySkinPieces[i].MySkinType == skinPieceElement.MySkinType)
            {
                tempListToClear.Add(skinPiecesForBodyX.MySkinPieces[i]);
                tempListToClearUI.Add(skinPiecesForBodyUIX.MySkinPieces[i]);     
            }
        }

        // iterate through the temp lists
        for (int i = 0; i < tempListToClear.Count; i++)
        {
            // this logic is what applies the skins on the player character
            tempListToClear[i].gameObject.SetActive(true);
            // this logic is what applies the skins on the Closet mouse
            tempListToClearUI[i].gameObject.SetActive(true);

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
                GameManager.Instance.characterGeoReferences.EarL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.EarR.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.EarL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.EarR.gameObject.SetActive(state);
                break;
            case Type_Body.Head:
                GameManager.Instance.characterGeoReferences.Head.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.Head.gameObject.SetActive(state);
                break;
            case Type_Body.Chest:
                GameManager.Instance.characterGeoReferences.Chest.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.Skirt.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.Chest.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.Skirt.gameObject.SetActive(state);
                break;
            case Type_Body.ArmLeft:
                GameManager.Instance.characterGeoReferences.ArmUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.HandL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ElbowL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ShoulderL.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.ArmUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.HandL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.ElbowL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.ShoulderL.gameObject.SetActive(state);
                break;
            case Type_Body.ArmRight:
                GameManager.Instance.characterGeoReferences.ArmUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.HandR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ElbowR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ShoulderR.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.ArmUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.HandR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.ElbowR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.ShoulderR.gameObject.SetActive(state);
                break;
            case Type_Body.LegLeft:
                GameManager.Instance.characterGeoReferences.LegUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.KneeL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.LegLowL.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.LegUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.KneeL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.LegLowL.gameObject.SetActive(state);
                break;
            case Type_Body.LegRight:
                GameManager.Instance.characterGeoReferences.LegUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.KneeR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.LegLowR.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.LegUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.KneeR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferencesUI.LegLowR.gameObject.SetActive(state);
                break;
            case Type_Body.FootLeft:
                GameManager.Instance.characterGeoReferences.FootL.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.FootL.gameObject.SetActive(state);
                break;
            case Type_Body.FootRight:
                GameManager.Instance.characterGeoReferences.FootR.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.FootR.gameObject.SetActive(state);
                break;
            case Type_Body.Tail:
                GameManager.Instance.characterGeoReferences.Tail.gameObject.SetActive(state);
                // ui
                GameManager.Instance.characterGeoReferencesUI.Tail.gameObject.SetActive(state);
                break;

        }
    }

    #endregion
}
