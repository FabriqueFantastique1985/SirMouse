using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonInstrumentSelect : ButtonPaging
{
    [Header("Notification related")]
    public GameObject NotificationObject;


    protected override void TurnOnPage()
    {
        // if I'm closing the instruments
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            PageController.Instance.TurnAllPagesOffExcept(PageType.BackpackButtons);
        }
        else // if I'm opening the instruments...
        {
            PageController.Instance.TurnAllPagesOffExcept(_turnThisPage);

            NotificationObject.SetActive(false);
            PageController.Instance.ButtonBackpackSuper.IhaveNotificationsLeftInstruments = false;
            PageController.Instance.NotifyBackpackSuper();
        }
    }
}
