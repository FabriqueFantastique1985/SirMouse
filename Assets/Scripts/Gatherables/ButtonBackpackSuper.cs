using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonBackpackSuper : ButtonPaging
{
    // I should show the page with 3 other buttons + disable buttons main gameplay
    [SerializeField]
    protected string _animationNameAppear;

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

            //_pageInstance.OpenBagImage(false); // changed sprite in past
            //_pageInstance.OpenClosetImage(false); // changed sprite in past

            // sleeping allowed
            GameManager.Instance.Player.Character.SetBoolSleeping(false);

            _pageInstance.ShowGameplayHUD(true);
        }
        else // else TURN ON
        {
            // turn off the camera wrap for closet
            SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);
            PageController.Instance.CameraUI_Backpack_Closet.enabled = false;

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            //_pageInstance.OpenBagImage(true); // changed sprite in past
            //_pageInstance.OpenClosetImage(false); // changed sprite in past

            // sleeping ILLEGAL
            GameManager.Instance.Player.Character.SetBoolSleeping(true);

            _pageInstance.ShowGameplayHUD(false);
        }
    }

    private void OnEnable()
    {
        _animationComponent.Play(_animationNameAppear);
    }
}
