using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsMouseController : MonoBehaviour
{
    public static SkinsMouseController Instance { get; private set; }
    public Animator ClosetWrapInsideCamera;

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
    //

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
    //


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
        }

        FindCorrectSkinPieceButton(skinPieceForBodyX, skinType, bodyType);
    }

    public void EquipSkinPiece(Type_Body bodyType, Type_Skin skinType)
    {
        SkinPiecesForThisBodyType skinPieceForBodyX = null;

        switch (bodyType)
        {
            case Type_Body.None:
                skinPieceForBodyX = null;
                break;
            case Type_Body.Hat:
                skinPieceForBodyX = SkinPiecesHat;
                break;
            case Type_Body.Head:
                skinPieceForBodyX = SkinPiecesHead;
                break;
            case Type_Body.Chest:
                skinPieceForBodyX = SkinPiecesChest;
                break;
            case Type_Body.ArmLeft:
                skinPieceForBodyX = SkinPiecesArmLeft;
                break;
            case Type_Body.ArmRight:
                skinPieceForBodyX = SkinPiecesArmRight;
                break;
            case Type_Body.LegLeft:
                skinPieceForBodyX = SkinPiecesLegLeft;
                break;
            case Type_Body.LegRight:
                skinPieceForBodyX = SkinPiecesLegRight;
                break;
            case Type_Body.FootLeft:
                skinPieceForBodyX = SkinPiecesFootLeft;
                break;
            case Type_Body.FootRight:
                skinPieceForBodyX = SkinPiecesFootRight;
                break;
        }

        FindCorrectSkinPieceRig(skinPieceForBodyX, skinType, bodyType);
    }


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
    private void FindCorrectSkinPieceRig(SkinPiecesForThisBodyType skinPiecesForBodyX, Type_Skin skinType, Type_Body bodyType)
    {
        for (int i = 0; i < skinPiecesForBodyX.MySkinPieces.Count; i++)
        {
            if (skinPiecesForBodyX.MySkinPieces[i].MySkinType == skinType)
            {
                // activate the object
                skinPiecesForBodyX.MySkinPieces[i].gameObject.SetActive(true);

                // de-activate old geo if Hide == true
                if (skinPiecesForBodyX.MySkinPieces[i].HidesSirMouseGeometry == true)
                {
                    SetSirMouseGeometryState(bodyType, false);
                }
                else
                {
                    SetSirMouseGeometryState(bodyType, true);
                }

                break;
            }
        }
    }

    // makes old geometry visible/invisible
    private void SetSirMouseGeometryState(Type_Body bodyType, bool state)
    {
        switch (bodyType)
        {
            case Type_Body.None:
                break;
            case Type_Body.Hat:
                GameManager.Instance.characterGeoReferences.EarL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.EarR.gameObject.SetActive(state);
                break;
            case Type_Body.Head:
                GameManager.Instance.characterGeoReferences.Head.gameObject.SetActive(state);
                break;
            case Type_Body.Chest:
                GameManager.Instance.characterGeoReferences.Chest.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.Skirt.gameObject.SetActive(state);
                break;
            case Type_Body.ArmLeft:
                GameManager.Instance.characterGeoReferences.ArmUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.HandL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ElbowL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ShoulderL.gameObject.SetActive(state);
                break;
            case Type_Body.ArmRight:
                GameManager.Instance.characterGeoReferences.ArmUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.HandR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ElbowR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.ShoulderR.gameObject.SetActive(state);
                break;
            case Type_Body.LegLeft:
                GameManager.Instance.characterGeoReferences.LegUpL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.KneeL.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.LegLowL.gameObject.SetActive(state);
                break;
            case Type_Body.LegRight:
                GameManager.Instance.characterGeoReferences.LegUpR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.KneeR.gameObject.SetActive(state);
                GameManager.Instance.characterGeoReferences.LegLowR.gameObject.SetActive(state);
                break;
            case Type_Body.FootLeft:
                GameManager.Instance.characterGeoReferences.FootL.gameObject.SetActive(state);
                break;
            case Type_Body.FootRight:
                GameManager.Instance.characterGeoReferences.FootR.gameObject.SetActive(state);
                break;
            case Type_Body.Tail:
                GameManager.Instance.characterGeoReferences.Tail.gameObject.SetActive(state);
                break;

        }
    }

    /*
    public SkinnedMeshRenderer Head;
    public SkinnedMeshRenderer EarL;
    public SkinnedMeshRenderer EarR;
    public SkinnedMeshRenderer ShoulderL;
    public SkinnedMeshRenderer ShoulderR;
    public SkinnedMeshRenderer ArmUpL;
    public SkinnedMeshRenderer ArmUpR;
    public SkinnedMeshRenderer ElbowL;
    public SkinnedMeshRenderer ElbowR;
    public SkinnedMeshRenderer HandL;
    public SkinnedMeshRenderer HandR;
    public SkinnedMeshRenderer Chest;
    public SkinnedMeshRenderer Skirt;
    public SkinnedMeshRenderer Tail;
    public SkinnedMeshRenderer LegUpL;
    public SkinnedMeshRenderer LegUpR;
    public SkinnedMeshRenderer KneeL;
    public SkinnedMeshRenderer KneeR;
    public SkinnedMeshRenderer LegLowL;
    public SkinnedMeshRenderer LegLowR;
    public SkinnedMeshRenderer FootL;
    public SkinnedMeshRenderer FootR;
    */
}
