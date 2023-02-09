using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonClosetNew : ButtonPaging
{
    [SerializeField]
    private ButtonClosetMouseCycle _buttonMouseCycle;

    protected override void TurnOnPage()
    {
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            // i should also turn off the specific pages inside the closet page (bigfix)
            _pageInstance.TurnPageOff(PageType.ClosetHeadTorsoTail);
            _pageInstance.TurnPageOff(PageType.ClosetLegs);
            _pageInstance.TurnPageOff(PageType.ClosetArms);

            _pageInstance.OpenClosetImage(false);
            _pageInstance.OpenBagImage(false);
            
            if (DoIHaveActivePages() == false)
            {
                GameManager.Instance.BlockInput = false;
            }

            _buttonMouseCycle.ResetTextureSirMouse(); 

            SkinController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);
        }
        else
        {
            //_pageInstance.TurnPageOn(_turnThisPage);
            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            _pageInstance.OpenClosetImage(true);
            _pageInstance.OpenBagImage(false);

            /*GameManager.Instance.BlockInput = true;*/

            // turn on the the correct page within the closet...
            OpenCorrectPageInCloset();

            SkinController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(true);
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
