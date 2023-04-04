using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonResourceSelect : ButtonPaging
{
    [Header("Notification related")]
    public bool IhaveNotificationsReadyInTheCloset;
    public GameObject NotificationObject;


    protected override void TurnOnPage()
    {
        // if I'm closing the resources...
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            PageController.Instance.TurnAllPagesOffExcept(PageType.BackpackButtons);
        }
        else // if I'm opening the resources...
        {
            PageController.Instance.TurnAllPagesOffExcept(_turnThisPage);
        }
    }
}
