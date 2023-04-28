using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinControllerMainMenu : MonoBehaviour
{
    public static SkinControllerMainMenu Instance;

    [SerializeField]
    private SkinPiecesForThisBodyType _skinListHats;
    [SerializeField]
    private SkinPiecesForThisBodyType _skinListHeads;


    private SkinPieceElement _equipedSkinPiece;

    [SerializeField]
    private AudioSource _sourceFX;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void EquipSkinMenu(SkinPieceElement skinClicked)
    {
        // unequip older one
        UnequipSkinMenu();

        // enable correct skinpiece 
        if (skinClicked.Data.MyBodyType == Type_Body.Hat)
        {
            for (int i =0 ; i < _skinListHats.MySkinPieces.Count; i++)
            {
                if (_skinListHats.MySkinPieces[i].Data.MySkinType == skinClicked.Data.MySkinType)
                {
                    _skinListHats.MySkinPieces[i].gameObject.SetActive(true);
                    _equipedSkinPiece = _skinListHats.MySkinPieces[i];

                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < _skinListHeads.MySkinPieces.Count; i++)
            {
                if (_skinListHeads.MySkinPieces[i].Data.MySkinType == skinClicked.Data.MySkinType)
                {
                    _skinListHeads.MySkinPieces[i].gameObject.SetActive(true);
                    _equipedSkinPiece = _skinListHeads.MySkinPieces[i];

                    break;
                }
            }
        }

        // play sound effect
        _sourceFX.Play();
    }

    private void UnequipSkinMenu()
    {
        if (_equipedSkinPiece != null)
        {
            _equipedSkinPiece.gameObject.SetActive(false);
        }      
    }
}
