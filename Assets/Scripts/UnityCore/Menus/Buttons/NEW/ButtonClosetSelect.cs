using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonClosetSelect : ButtonPaging
{
    [Header("Notification related")]
    public bool IhaveNotificationsReadyInTheCloset;
    public GameObject NotificationObject;


    protected override void TurnOnPage()
    {
        // if I'm closing the closet...
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            ClosetController.Instance.CloseCloset();
        }
        else // if I'm opening the closet...
        {
            ClosetController.Instance.OpenCloset(_turnThisPage);
        }
    }



}
