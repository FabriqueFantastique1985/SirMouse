using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonClosetNew : ButtonPaging
{
    [SerializeField]
    private GameObject _closetWrap;

    [SerializeField]
    private ButtonClosetMouseCycle _buttonMouseCycle;

    public override void ClickedButton()
    {
        base.ClickedButton();
    }


    protected override void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);
            _buttonMouseCycle.ResetTextureSirMouse();
            _closetWrap.SetActive(false);
        }
        else
        {
            _pageInstance.TurnPageOn(_turnThisPage);
            // turn on the the correct page within the closet...
            OpenCorrectPageInCloset();

            _closetWrap.SetActive(true);
        }
    }


    private void OpenCorrectPageInCloset()
    {
        switch (_buttonMouseCycle.Index)
        {
            case 0:
                _pageInstance.TurnPageOn(PageType.ClosetHeadTorsoTail);
                _buttonMouseCycle.ChangeTextureMouse();
                break;
            case 1:
                _pageInstance.TurnPageOn(PageType.ClosetLegs);
                _buttonMouseCycle.ChangeTextureMouse();
                break;
            case 2:
                _pageInstance.TurnPageOn(PageType.ClosetArms);
                _buttonMouseCycle.ChangeTextureMouse();
                break;
            case 3:
                _pageInstance.TurnPageOn(PageType.ClosetHeadTorsoTail);
                _buttonMouseCycle.ChangeTextureMouse();
                break;
        }
    }
}
