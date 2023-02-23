using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonClosetNew : ButtonPaging
{
    protected override void TurnOnPage()
    {
        // if I'm closing the closet...
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            ClosetController.Instance.CloseCloset();
        }
        else // if I'm opening the closet...
        {
            // turn off all other pages, except for the closet
            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            // open up the last page that was opened within the closet
            if (ClosetController.Instance.PageTypeOpenedInClosetLastTime == PageType.None)
            {
                _pageInstance.TurnPageOn(PageType.ClosetHats);
            }
            else
            {
                _pageInstance.TurnPageOn(ClosetController.Instance.PageTypeOpenedInClosetLastTime);
            }
            
            // update images
            _pageInstance.OpenClosetImage(true);
            _pageInstance.OpenBagImage(false);

            // turn on the UI player things
            SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(true);
        }
    }



}
