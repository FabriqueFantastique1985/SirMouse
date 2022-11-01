using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClosetNew : ButtonPaging
{
    [SerializeField]
    private GameObject _closetWrap;

    public override void ClickedButton()
    {
        base.ClickedButton();
    }


    protected override void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);
            _closetWrap.SetActive(false);
        }
        else
        {
            _pageInstance.TurnPageOn(_turnThisPage);
            _closetWrap.SetActive(true);
        }
    }
}
