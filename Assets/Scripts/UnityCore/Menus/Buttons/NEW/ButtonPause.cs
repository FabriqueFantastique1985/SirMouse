using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : ButtonPaging
{
    [SerializeField]
    protected string _animationNameAppear;


    protected override void TurnOnPage()
    {
        // if the page is on...TURN IT OFF
        if (_pageInstance.PageIsOn(_turnThisPage) == true)
        {
            _pageInstance.TurnPageOff(_turnThisPage);

            // sleeping allowed
            GameManager.Instance.Player.Character.SetBoolSleeping(false);

            _pageInstance.ShowBottomLeftButtons(true);
        }
        else // else TURN ON
        {
            // turn off the camera wrap for closet
            SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);

            _pageInstance.TurnAllPagesOffExcept(_turnThisPage);

            // sleeping ILLEGAL
            GameManager.Instance.Player.Character.SetBoolSleeping(true);

            _pageInstance.ShowBottomLeftButtons(false);
        }
    }

    private void OnEnable()
    {
        if (_animationNameAppear != string.Empty)
        {
            _animationComponent.Play(_animationNameAppear);
        }      
    }
}
