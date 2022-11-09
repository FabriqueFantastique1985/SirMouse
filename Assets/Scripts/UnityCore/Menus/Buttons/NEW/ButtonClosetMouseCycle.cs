using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonClosetMouseCycle : ButtonBaseNew
{
    // turn on correct page for mouse skin cycle
    // turn off other ones

    protected PageController _pageInstance;

    [SerializeField]
    private CharacterGeoReferences _geoRefsInCamera;

    public int Index = 3;
    private int _limit = 3;

    protected override void Start()
    {
        base.Start();
        _pageInstance = PageController.Instance;
    }

    public override void ClickedButton()
    {
        base.ClickedButton();

        // perhaps add logic here that makes the character zoom in on bodyparts of interest

        Debug.Log(Index + "current index");
        Index = (Index + 1) % _limit;
        Debug.Log(Index + "after index");

        ChangeTextureMouse();
        TurnOnPage(Index);
    }

    protected void TurnOnPage(int index)
    {
        // turn on the next one depending on index
        switch (index)
        {
            case 0:
                _pageInstance.TurnPageOff(PageType.ClosetArms);
                _pageInstance.TurnPageOn(PageType.ClosetHeadTorsoTail);
                break;
            case 1:
                _pageInstance.TurnPageOff(PageType.ClosetHeadTorsoTail);
                _pageInstance.TurnPageOn(PageType.ClosetLegs);
                break;
            case 2:
                _pageInstance.TurnPageOff(PageType.ClosetLegs);
                _pageInstance.TurnPageOn(PageType.ClosetArms);
                break;
        }
    }

    public void ChangeTextureMouse()
    {
        _geoRefsInCamera.ChangeTextureSirMouse(Index);
    }

    public void ResetTextureSirMouse()
    {
        _geoRefsInCamera.DefaultTextureSirMouse();
    }

}
