using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonBackpackSuper : ButtonPaging
{
    // I should show the page with 3 other buttons

    [Header("Notification related")]
    public bool IhaveNotificationsLeftCloset;
    public bool IhaveNotificationsLeftInstruments;

    public GameObject NotificationObject;

    protected override void TurnOnPage()
    {
        // if the page is on...TURN IT OFF
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            _pageInstance.OpenBagImage(false);
            _pageInstance.OpenClosetImage(false);

            //AudioController.Instance.TurnDownVolumeForOSTAndWorld(false);

            // sleeping allowed
            GameManager.Instance.Player.Character.SetBoolSleeping(false);
        }
        else // else TURN ON
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
        }
    }
}
