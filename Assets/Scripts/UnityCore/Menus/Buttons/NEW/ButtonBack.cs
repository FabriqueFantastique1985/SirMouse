using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonBack : ButtonPaging
{
    [SerializeField]
    protected string _animationNameAppear;

    public override void ClickedButton()
    {
        base.ClickedButton();

        // close all pages in backpack + enable buttons main gameplay
        _pageInstance.TurnPageOff(PageType.BackpackResources);
        _pageInstance.TurnPageOff(PageType.BackpackInstruments);
        _pageInstance.TurnPageOff(PageType.BackpackCloset);
        ClosetController.Instance.CloseCloset();
        _pageInstance.TurnPageOff(PageType.BackpackButtons);

        _pageInstance.ShowGameplayHUD(true);
    }

    private void OnEnable()
    {
        _animationComponent.Play(_animationNameAppear);
    }
}
