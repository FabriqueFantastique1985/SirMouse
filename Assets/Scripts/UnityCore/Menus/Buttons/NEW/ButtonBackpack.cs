using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Menus;
using UnityCore.Audio;

public class ButtonBackpack : ButtonPaging
{
    protected override void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            _pageInstance.OpenBagImage(false);
            _pageInstance.OpenClosetImage(false);

            //AudioController.Instance.TurnDownVolumeForOSTAndWorld(false);

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
            PageController.Instance.CameraUI_Backpack_Closet.enabled = false;

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            //AudioController.Instance.TurnDownVolumeForOSTAndWorld();

            _pageInstance.OpenBagImage(true);
            _pageInstance.OpenClosetImage(false);

            // sleeping ILLEGAL
            GameManager.Instance.Player.Character.SetBoolSleeping(true);

            //    GameManager.Instance.BlockInput = true;
        }
    }
}
