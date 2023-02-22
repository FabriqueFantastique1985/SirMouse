using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menus;

public class ButtonBackpack : ButtonPaging, IClickable
{
    protected override void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            _pageInstance.OpenBagImage(false);
            _pageInstance.OpenClosetImage(false);
            
            if (DoIHaveActivePages() == false)
            {
                GameManager.Instance.BlockInput = false;
            }
        }
        else
        {
            // turn off the camera wrap for closet
            SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);
            //SkinController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);
            SkinsMouseController.Instance.characterGeoReferencesUI.DefaultTextureSirMouse();

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);
            //_pageInstance.TurnPageOn(_turnThisPage);

            _pageInstance.OpenBagImage(true);
            _pageInstance.OpenClosetImage(false);

        //    GameManager.Instance.BlockInput = true;
        }
    }

    public void Click(Player player)
    {
        ClickedButton();
    }
}
