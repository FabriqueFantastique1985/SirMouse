using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkinCycle : ButtonBaseNew
{
    public SkinType SkinType;

    [SerializeField]
    private Button _buttonComponent;

    private bool _listHasSkins;

    public override void ClickedButton()
    {
        base.ClickedButton();

        SkinController.Instance.CycleSkinPiece(SkinType);
    }


    private void OnEnable()
    {
        if (_listHasSkins == false)
        {
            if (SkinController.Instance.IsListSizeGreaterThan1(SkinType) == true)
            {
                _listHasSkins = true;
                _buttonComponent.interactable = true;
            }
            else
            {
                // disable this button
                _buttonComponent.interactable = false;
            }
        }
    }
}
