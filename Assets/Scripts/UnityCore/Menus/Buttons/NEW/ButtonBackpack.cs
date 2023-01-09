using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menus;

public class ButtonBackpack : ButtonPaging
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
            SkinController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);
            //_pageInstance.TurnPageOn(_turnThisPage);

            _pageInstance.OpenBagImage(true);
            _pageInstance.OpenClosetImage(false);

            GameManager.Instance.BlockInput = true;
        }
    }
}
