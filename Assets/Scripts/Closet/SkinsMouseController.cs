using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SkinsMouseController : MonoBehaviour, IDataPersistence
{
    public static SkinsMouseController Instance { get; private set; }


    public CharacterRigReferences CharacterRigRefs;
    public CharacterRigReferences CharacterRigRefsUI;
    public CharacterGeoReferences characterGeoReferences;
    public CharacterGeoReferences characterGeoReferencesUI;

    public Animator ClosetWrapInsideCamera;

    //[HideInInspector]
    public List<SkinPieceElement> EquipedSkinPieces = new List<SkinPieceElement>();

    private Dictionary<Type_Body, Type_Skin> _equipedSkins = new Dictionary<Type_Body, Type_Skin>();

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

        Initialize();
        EquipFullOutfit(Type_Skin.Pyjama, true);
    }

    private void Initialize()
    {
        // Initializing the Equiped Skin Pieces List
        EquipedSkinPieces = new List<SkinPieceElement>();

        // Initializing the Equiped Skins Dictionary
        _equipedSkins.Add(Type_Body.Head, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.Chest, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.ArmLeftLower, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.ArmRightLower, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.LegLeftLower, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.LegRightLower, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.FootRight, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.FootLeft, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.Hat, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.Shield, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.Sword, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.Tail, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.KneeLeft, Type_Skin.Pyjama);
        _equipedSkins.Add(Type_Body.KneeRight, Type_Skin.Pyjama);

        // adds all the buttons of the closer UI to the overall list of buttons 
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

    /// <summary>
    /// Equips a full outfit based on the skinType
    /// </summary>
    /// <param name="skinType">The skinType being equipped (like Pyjama, Armour, ... ) </param>
    /// <param name="unequipAccessories">Things like hats, tails and such being unequipped or not. </param>
    private void EquipFullOutfit(Type_Skin skinType, bool unequipAccessories = false)
    {
        _equipedSkins[Type_Body.Head] = skinType;
        _equipedSkins[Type_Body.Chest] = skinType;
        _equipedSkins[Type_Body.ArmLeftLower] = skinType;
        _equipedSkins[Type_Body.ArmRightLower] = skinType;
        _equipedSkins[Type_Body.FootRight] = skinType;
        _equipedSkins[Type_Body.FootLeft] = skinType;
        _equipedSkins[Type_Body.LegLeftLower] = skinType;
        _equipedSkins[Type_Body.LegRightLower] = skinType;
        _equipedSkins[Type_Body.KneeLeft] = skinType;
        _equipedSkins[Type_Body.KneeRight] = skinType;

        if (unequipAccessories)
        {
            _equipedSkins[Type_Body.Hat] = Type_Skin.None;
            _equipedSkins[Type_Body.Shield] = Type_Skin.None;
            _equipedSkins[Type_Body.Sword] = Type_Skin.None;
            _equipedSkins[Type_Body.Tail] = Type_Skin.None;
        }

        // Equipping the actual skin piece in the rig and closet. 
        foreach (var skinPiecePair in _equipedSkins)
        {
            // Find skinpiece based on body type and skin type
            var skinPiece = FindSkinpiece(skinPiecePair.Key, skinPiecePair.Value, out ButtonSkinPiece button);
            if (skinPiece)
            {
                // Equip skinpiece
                EquipSkinPiece(skinPiece);
                
                // Set Data of skinpieceButton on true
                ButtonSkinPieceData skinPieceData = new ButtonSkinPieceData();
                skinPieceData.Found = true;
                button.Data = skinPieceData;

                // Unlock skinpiece in closet
                UnlockSkinPiece(skinPiece);
            }
        }
    }

    private SkinPieceElement FindSkinpiece(Type_Body skinpieceBodyType, Type_Skin skinpieceSkinType)
    {
        return FindSkinpiece(skinpieceBodyType, skinpieceSkinType, out ButtonSkinPiece button);
    }

    private SkinPieceElement FindSkinpiece(Type_Body skinpieceBodyType, Type_Skin skinpieceSkinType, out ButtonSkinPiece button)
    {
        if (skinpieceSkinType == Type_Skin.None)
        {
            button = null;
            return null;
        }
        
        // Loop over buttons (hat, head, chest, ...)
        for (int i = 0; i < _listsOfButtons.Count; i++)
        {
            // Loop over skinPieces (chicken, jester, pyjama)
            for (int j = 0; j < _listsOfButtons[i].MySkinPiecesButtons.Count; j++)
            {
                var skinPieceElement = _listsOfButtons[i].MySkinPiecesButtons[j].MySkinPieceElement;
                if (skinPieceElement.Data.MyBodyType != skinpieceBodyType)
                {
                    break;
                }

                if (skinPieceElement.Data.MySkinType == skinpieceSkinType)
                {
                    button = _listsOfButtons[i].MySkinPiecesButtons[j];
                    return skinPieceElement;
                }
            }
        }

        button = null;
        return null;
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

        switch (skinPieceElement.Data.MyBodyType)
        {
            default:
                //Debug.LogWarning("No skin piece for this body type found! " + skinPieceElement.Data.MyBodyType + "unlocking hat instead.");
                Debug.LogWarning("No skin piece for this body type found! " + skinPieceElement.Data.MyBodyType + " --> unlocking NOTHING.");
                //skinPieceForBodyX = SkinPiecesButtonHat;
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
            case Type_Body.ArmLeftLower:
                skinPieceForBodyX = SkinPiecesButtonArmLeft;
                break;
            case Type_Body.ArmRightLower:
                skinPieceForBodyX = SkinPiecesButtonArmRight;
                break;
            case Type_Body.LegLeftLower:
                skinPieceForBodyX = SkinPiecesButtonLegLeft;
                break;
            case Type_Body.LegRightLower:
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

        if (skinPieceForBodyX == null)
        {
            return null;
        }
        else
        {
            return FindCorrectSkinPieceButton(skinPieceForBodyX, skinPieceElement);
        }       
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
        return FindCorrectSkinPieceButton(SkinPiecesButtonSword,
            SkinPiecesButtonSword.MySkinPiecesButtons[1].MySkinPieceElement);
    }

    public void EquipSkinPiece(Type_Body bodyType, Type_Skin skinType)
    {
        SkinPiecesForThisBodyType skinPieceForBodyX = null;
        SkinPiecesForThisBodyType skinPieceForBodyUIX = null;

        switch (bodyType)
        {
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
            case Type_Body.ArmLeftUpper:
            case Type_Body.ArmLeftLower:
                skinPieceForBodyX = SkinPiecesArmLeft;
                skinPieceForBodyUIX = SkinPiecesUIArmLeft;
                break;
            case Type_Body.ArmRightUpper:
            case Type_Body.ArmRightLower:
                skinPieceForBodyX = SkinPiecesArmRight;
                skinPieceForBodyUIX = SkinPiecesUIArmRight;
                break;
            case Type_Body.KneeLeft:
            case Type_Body.LegLeftUpper:
            case Type_Body.LegLeftLower:
                skinPieceForBodyX = SkinPiecesLegLeft;
                skinPieceForBodyUIX = SkinPiecesUILegLeft;
                break;
            case Type_Body.KneeRight:
            case Type_Body.LegRightUpper:
            case Type_Body.LegRightLower:
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
        UnEquipOtherSkinPieces(bodyType);
        // find correct skin piece to equip for bodyTypeX
        FindCorrectSkinPieceRig(skinPieceForBodyX, skinPieceForBodyUIX, bodyType, skinType);
    }

    // called when a piece is dragged onto SirMouse...
    public void EquipSkinPiece(SkinPieceElement skinPieceElement)
    {
        SkinPiecesForThisBodyType skinPieceForBodyX = null;
        SkinPiecesForThisBodyType skinPieceForBodyUIX = null;

        switch (skinPieceElement.Data.MyBodyType)
        {
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
            case Type_Body.ArmLeftUpper:
            case Type_Body.ArmLeftLower:
                skinPieceForBodyX = SkinPiecesArmLeft;
                skinPieceForBodyUIX = SkinPiecesUIArmLeft;
                break;
            case Type_Body.ArmRightUpper:
            case Type_Body.ArmRightLower:
                skinPieceForBodyX = SkinPiecesArmRight;
                skinPieceForBodyUIX = SkinPiecesUIArmRight;
                break;
            case Type_Body.KneeLeft:
            case Type_Body.LegLeftUpper:
            case Type_Body.LegLeftLower:
                skinPieceForBodyX = SkinPiecesLegLeft;
                skinPieceForBodyUIX = SkinPiecesUILegLeft;
                break;
            case Type_Body.KneeRight:
            case Type_Body.LegRightUpper:
            case Type_Body.LegRightLower:
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
        UnEquipOtherSkinPieces(skinPieceElement.Data.MyBodyType);
        // find correct skin piece to equip for bodyTypeX
        FindCorrectSkinPieceRig(skinPieceForBodyX, skinPieceForBodyUIX, skinPieceElement);
    }

    public void HideOrShowSwordAndShield(bool showMe)
    {
        // check for possibly equiped skinpieces on shield/sword...   
        List<SkinPieceElement> tempList = new List<SkinPieceElement>();
        for (int i = 0; i < EquipedSkinPieces.Count; i++)
        {
            if (EquipedSkinPieces[i].Data.MyBodyType == Type_Body.Sword || EquipedSkinPieces[i].Data.MyBodyType == Type_Body.Shield)
            {
                tempList.Add(EquipedSkinPieces[i]);
            }
        }
        // enaable the visuals of said sword/shield
        if (tempList.Count > 1)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList[i].gameObject.SetActive(showMe);
            }
        }
        else if (tempList.Count > 0)
        {
            if (tempList[0].Data.MyBodyType == Type_Body.Sword)
            {
                // show the sword skin
                tempList[0].gameObject.SetActive(showMe);
                // show geo shield
                characterGeoReferences.Shield.gameObject.SetActive(showMe);
            }
            else
            {
                // show shield skin
                tempList[0].gameObject.SetActive(showMe);
                // show geo sword
                characterGeoReferences.Sword.gameObject.SetActive(showMe);
            }
        }
        else // else show the sir mouse geo
        {
            characterGeoReferences.Sword.gameObject.SetActive(showMe);
            characterGeoReferences.Shield.gameObject.SetActive(showMe);
        }
    }
    
    #endregion

    #region Private Functions

    private ButtonSkinPiece FindCorrectSkinPieceButton(SkinPiecesForThisBodyTypeButton skinPiecesForBodyX,
        SkinPieceElement skinPieceElement)
    {
        for (int i = 0; i < skinPiecesForBodyX.MySkinPiecesButtons.Count; i++)
        {
            if (skinPiecesForBodyX.MySkinPiecesButtons[i].MySkinPieceElement.Data.MySkinType ==
                skinPieceElement.Data.MySkinType)
            {
                // activate the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].Found = true;
                // enable the sprite over the sillhouette on the button
                skinPiecesForBodyX.MySkinPiecesButtons[i].MySpriteToActivateWhenFound.SetActive(true);
                
                //Debug.Log("Unlocked skin piece: " + skinPiecesForBodyX.MySkinPiecesButtons[i].MySkinPieceElement.Data.MySkinType 
                //                                  + " for body type: " + skinPiecesForBodyX.MySkinPiecesButtons[i].MySkinPieceElement.Data.MyBodyType);

                //return skinPiecesForBodyX.MySkinPiecesButtons[i].MySpriteToActivateWhenFound;
                return skinPiecesForBodyX.MySkinPiecesButtons[i];
            }
        }

        return null;
    }

    private void FindCorrectSkinPieceRig(SkinPiecesForThisBodyType skinPiecesForBodyX,
        SkinPiecesForThisBodyType skinPiecesForBodyUIX, SkinPieceElement skinPieceElement)
    {
        // should first add all of the pieces == skinType to temp lists 
        // (we iterate over every single skinPiece on BodyTypeX, so i'd better seperate additional lines of code for smaller temp lists)
        List<SkinPieceElement> tempList = new List<SkinPieceElement>();
        List<SkinPieceElement> tempListUI = new List<SkinPieceElement>();
        for (int i = 0; i < skinPiecesForBodyX.MySkinPieces.Count; i++)
        {
            // find corresponding skinTypes in the list of skinPieces (will not enter the if, if I'm using default --- check tempList size count 0)
            if (skinPiecesForBodyX.MySkinPieces[i].Data.MySkinType == skinPieceElement.Data.MySkinType)
            {
                tempList.Add(skinPiecesForBodyX.MySkinPieces[i]);
                tempListUI.Add(skinPiecesForBodyUIX.MySkinPieces[i]);

                // link the score that was set on the Closet
                skinPiecesForBodyX.MySkinPieces[i].ScoreValue = skinPieceElement.ScoreValue;
                skinPiecesForBodyUIX.MySkinPieces[i].ScoreValue =
                    skinPieceElement.ScoreValue; // 1 of these can be removed (as long as we check correct list for score)

                //break; // this break BREAKS equiping pieces (explained below)

                // with break -> If I find Ostrick Knee, I will not equip Ostrich LegUpper, LegLower !
                // Ideally, skinpieces like |kneeLeft, LegLeftLower, LegLeftUpper| should all use the same Type_Body (cleaner code, more performant)
            }
        }

        // indicating that we're trying to equip a Sir Mouse default skinPiece
        if (tempList.Count == 0)
        {
            SetSirMouseGeometryState(skinPieceElement.Data.MyBodyType, true);
        }

        // iterate through the temp lists
        for (int i = 0; i < tempList.Count; i++)
        {
            // this logic is what applies the skins on the player character
            tempList[i].gameObject.SetActive(true);
            // this logic is what applies the skins on the Closet mouse
            tempListUI[i].gameObject.SetActive(true);

            //Debug.Log("Equiping piece " + tempList[i]);
            
            if (tempList[i].Data.MyBodyType == Type_Body.Sword || tempList[i].Data.MyBodyType == Type_Body.Shield)
            {
                // if i have equipment out...
                var hasInstrumentEquiped = InstrumentController.Instance.EquipedInstrument != Type_Instrument.None;
                tempList[i].gameObject.SetActive(!hasInstrumentEquiped);
            }
            else
            {
                tempList[i].gameObject.SetActive(true);        
            }

            // this logic is what applies the skins on the closet character
            tempListUI[i].gameObject.SetActive(true);

            // add to list of equiped skinpieces
            EquipedSkinPieces.Add(tempList[i]);
            EquipedSkinPiecesUI.Add(tempListUI[i]);

            // de-activate old geo if Hide == true
            if (tempList[i].HidesSirMouseGeometry == true)
            {
                SetSirMouseGeometryState(skinPieceElement.Data.MyBodyType, false, tempList[i].ShowUpper, tempList[i].ShowLower);
            }
            else
            {
                SetSirMouseGeometryState(skinPieceElement.Data.MyBodyType, true);
            }
        }

        CountTotalScore();
    }

    private void FindCorrectSkinPieceRig(SkinPiecesForThisBodyType skinPiecesForBodyX,
        SkinPiecesForThisBodyType skinPiecesForBodyUIX, Type_Body bodyType, Type_Skin skinType)
    {
        SkinPieceElement tempSkinPiece = null;
        SkinPieceElement tempSkinPieceUI = null;

        for (int i = 0; i < skinPiecesForBodyX.MySkinPieces.Count; i++)
        {
            // find corresponding skinTypes in the list of skinPieces (will not enter the if, if I'm using default --- check tempList size count 0)
            if (skinPiecesForBodyX.MySkinPieces[i].Data.MySkinType == skinType)
            {
                tempSkinPiece = skinPiecesForBodyX.MySkinPieces[i];
                tempSkinPieceUI = skinPiecesForBodyUIX.MySkinPieces[i];

                // TODO: do this once and in the editor 28/03/23
                // link the score that was set on the Closet
                 skinPiecesForBodyX.MySkinPieces[i].ScoreValue = tempSkinPiece.ScoreValue; 
                 skinPiecesForBodyUIX.MySkinPieces[i].ScoreValue = tempSkinPiece.ScoreValue; // 1 of these can be removed (as long as we check correct list for score)

                // this logic is what applies the skins on the player character
                tempSkinPiece.gameObject.SetActive(true);

                // this logic is what applies the skins on the Closet mouse
                tempSkinPieceUI.gameObject.SetActive(true);

                // add to list of equiped skinpieces
                EquipedSkinPieces.Add(tempSkinPiece);
                EquipedSkinPiecesUI.Add(tempSkinPieceUI);

                // de-activate old geo if Hide == true
                SetSirMouseGeometryState(bodyType, !tempSkinPiece.HidesSirMouseGeometry, tempSkinPiece.ShowUpper, tempSkinPiece.ShowLower);

                //Debug.Log("Equiping piece " + tempSkinPiece);
                CountTotalScore();
            }
        }

        // indicating that we're trying to equip a Sir Mouse default skinPiece
        if (tempSkinPiece == null)
        {
            SetSirMouseGeometryState(bodyType, true);
        }
    }

    private void UnEquipOtherSkinPieces(Type_Body bodyTypeToCheck)
    {
        // 1) iterate through equiped pieces, 2) find corresponding bodytypes
        List<SkinPieceElement> tempListToClear = new List<SkinPieceElement>();
        List<SkinPieceElement> tempListToClearUI = new List<SkinPieceElement>();

        for (int i = 0; i < EquipedSkinPieces.Count; i++)
        {
            var dataOfInterest = EquipedSkinPieces[i].Data;
            // if my type is any part of a leg or arm -> take all those pieces

            // if (bodyTypeToCheck == ArmRLower || ArmRUpper)...
            // if (bodyTypeToCheck == LegRLower || LegRUpper || KneeRLeft)
            // --> tempList.add(ArmRLower)
            // --> tempList.add(ArmRUpper)

            // first check all the kegs and arms...
            if (bodyTypeToCheck == Type_Body.ArmLeftLower || bodyTypeToCheck == Type_Body.ArmLeftUpper)
            {
                if (dataOfInterest.MyBodyType == Type_Body.ArmLeftLower || dataOfInterest.MyBodyType == Type_Body.ArmLeftUpper)
                {
                    tempListToClear.Add(EquipedSkinPieces[i]);
                    tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
                }

            }
            else if (bodyTypeToCheck == Type_Body.ArmRightLower || bodyTypeToCheck == Type_Body.ArmRightUpper)
            {
                if (dataOfInterest.MyBodyType == Type_Body.ArmRightLower || dataOfInterest.MyBodyType == Type_Body.ArmRightUpper)
                {
                    tempListToClear.Add(EquipedSkinPieces[i]);
                    tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
                }
            }
            else if (bodyTypeToCheck == Type_Body.LegLeftLower || bodyTypeToCheck == Type_Body.LegLeftUpper || bodyTypeToCheck == Type_Body.KneeLeft)
            {
                if (dataOfInterest.MyBodyType == Type_Body.LegLeftLower || dataOfInterest.MyBodyType == Type_Body.LegLeftUpper || dataOfInterest.MyBodyType == Type_Body.KneeLeft)
                {
                    tempListToClear.Add(EquipedSkinPieces[i]);
                    tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
                }
            }
            else if (bodyTypeToCheck == Type_Body.LegRightLower || bodyTypeToCheck == Type_Body.LegRightUpper || bodyTypeToCheck == Type_Body.KneeRight)
            {
                if (dataOfInterest.MyBodyType == Type_Body.LegRightLower || dataOfInterest.MyBodyType == Type_Body.LegRightUpper || dataOfInterest.MyBodyType == Type_Body.KneeRight)
                {
                    tempListToClear.Add(EquipedSkinPieces[i]);
                    tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
                }
            }
            else // then the rest...(Should be the only behavior in polish --> knee, legUpper,legLower all have TypeBody.Leg !)
            {
                if (EquipedSkinPieces[i].Data.MyBodyType == bodyTypeToCheck)
                {
                    tempListToClear.Add(EquipedSkinPieces[i]);
                    tempListToClearUI.Add(EquipedSkinPiecesUI[i]);
                }
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
    }

    // makes old geometry visible/invisible
    private void SetSirMouseGeometryState(Type_Body bodyType, bool state, bool shouldShowUpper = false, bool shouldShowLower = false) // false = invisible
    {
        // If state == true => show bodypieces so also show upper and lower bodypieces
        if (state)
        {
            shouldShowUpper = state;
            shouldShowLower = state;
        }

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
            case Type_Body.ArmLeftLower:
                characterGeoReferences.ElbowL.gameObject.SetActive(state);

                characterGeoReferences.ShoulderL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.ArmUpL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.HandL.gameObject.SetActive(shouldShowLower);
                // ui
                characterGeoReferencesUI.ElbowL.gameObject.SetActive(state);

                characterGeoReferencesUI.ShoulderL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.ArmUpL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.HandL.gameObject.SetActive(shouldShowLower);
                break;
            case Type_Body.ArmRightLower:
                characterGeoReferences.ElbowR.gameObject.SetActive(state);

                characterGeoReferences.ShoulderR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.ArmUpR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.HandR.gameObject.SetActive(shouldShowLower);

                characterGeoReferencesUI.ElbowR.gameObject.SetActive(state);

                characterGeoReferencesUI.ShoulderR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.ArmUpR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.HandR.gameObject.SetActive(shouldShowLower);
                break;
            case Type_Body.LegLeftLower:
                characterGeoReferences.KneeL.gameObject.SetActive(state);
                characterGeoReferences.LegUpL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.LegLowL.gameObject.SetActive(shouldShowLower);

                characterGeoReferencesUI.KneeL.gameObject.SetActive(state);
                characterGeoReferencesUI.LegUpL.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.LegLowL.gameObject.SetActive(shouldShowLower);
                break;
            case Type_Body.LegRightLower:
                characterGeoReferences.KneeR.gameObject.SetActive(state);
                characterGeoReferences.LegUpR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferences.LegLowR.gameObject.SetActive(shouldShowLower);

                characterGeoReferencesUI.KneeR.gameObject.SetActive(state);
                characterGeoReferencesUI.LegUpR.gameObject.SetActive(shouldShowUpper);
                characterGeoReferencesUI.LegLowR.gameObject.SetActive(shouldShowLower);
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
                if (InstrumentController.Instance.EquipedInstrument != Type_Instrument.None) // if i have equipment out...
                {
                    if (state == true) // if i want to show visuals of the sword...
                    {
                        characterGeoReferences.Sword.gameObject.SetActive(!state);
                    }
                    else
                    {
                        characterGeoReferences.Sword.gameObject.SetActive(state);
                    }
                }
                else
                {
                    characterGeoReferences.Sword.gameObject.SetActive(state);
                }

                characterGeoReferencesUI.Sword.gameObject.SetActive(state);
                break;
            case Type_Body.Shield:
                if (InstrumentController.Instance.EquipedInstrument != Type_Instrument.None) // if i have equipment out...
                {
                    if (state == true) // if i want to show visuals of the sword...
                    {
                        characterGeoReferences.Shield.gameObject.SetActive(!state);
                    }
                    else
                    {
                        characterGeoReferences.Shield.gameObject.SetActive(state);
                    }
                }
                else
                {
                    characterGeoReferences.Shield.gameObject.SetActive(state);
                }

                characterGeoReferencesUI.Shield.gameObject.SetActive(state);
                break;
        }
    }


    private void CountTotalScore()
    {
        int totalScore = 0;

        // Set correct score for each body part in dictionary
        for (int i = 0; i < EquipedSkinPieces.Count; i++)
        {
            _bodyPiecesScore[EquipedSkinPieces[i].Data.MyBodyType] = EquipedSkinPieces[i].ScoreValue;
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

    public void LoadData(GameData data)
    {
        // Load the Equiped Pieces
        foreach (var skinPieceData in data.EquipedSkinPiecesData)
        {
            //EquipSkinPiece(skinPieceData.MyBodyType, skinPieceData.MySkinType);
            var skinPiece = FindSkinpiece(skinPieceData.MyBodyType, skinPieceData.MySkinType);
            if (skinPiece)
            {
                EquipSkinPiece(skinPiece);
            }
        }

        // Load the Unlocked Pieces 
        // calculate all elements in 2d list
        int totalElements = 0;
        for (int i = 0; i < _listsOfButtons.Count; i++) totalElements += _listsOfButtons[i].MySkinPiecesButtons.Count;
        if (data.ButtonsSkinPieceData.Count != totalElements)
        {
            Debug.LogWarning(data.ButtonsSkinPieceData.ToString() + " Data is not correct");
            return;
        }

        int index = 0;
        for (int i = 0; i < _listsOfButtons.Count; i++)
        {
            for (int j = 0; j < _listsOfButtons[i].MySkinPiecesButtons.Count; j++)
            {
                _listsOfButtons[i].MySkinPiecesButtons[j].Data = data.ButtonsSkinPieceData[index];
                if (data.ButtonsSkinPieceData[index].Found)
                {
                    UnlockSkinPiece(_listsOfButtons[i].MySkinPiecesButtons[j].MySkinPieceElement);
                }
                index++;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // Save the Equiped Pieces
        data.EquipedSkinPiecesData.Clear();
        foreach (var skinPiece in EquipedSkinPieces)
        {
            data.EquipedSkinPiecesData.Add(skinPiece.Data);
        }

        // Save the Unlocked Pieces
        data.ButtonsSkinPieceData.Clear();
        foreach (SkinPiecesForThisBodyTypeButton skinPieceButton in _listsOfButtons)
        {
            foreach (ButtonSkinPiece skinPiece in skinPieceButton.MySkinPiecesButtons)
            {
                data.ButtonsSkinPieceData.Add(skinPiece.Data);
            }
        }
    }
}