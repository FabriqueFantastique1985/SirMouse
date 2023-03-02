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

            // sleeping allowed
            GameManager.Instance.Player.Character.SetBoolSleeping(false);

            //if (DoIHaveActivePages() == false) // not needed ?
            //{
            //    GameManager.Instance.BlockInput = false;
            //}
        }
        else
        {
            // turn off the camera wrap for closet
            SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            _pageInstance.OpenBagImage(true);
            _pageInstance.OpenClosetImage(false);

            // sleeping ILLEGAL
            GameManager.Instance.Player.Character.SetBoolSleeping(true);

            //    GameManager.Instance.BlockInput = true;
        }
    }
}
