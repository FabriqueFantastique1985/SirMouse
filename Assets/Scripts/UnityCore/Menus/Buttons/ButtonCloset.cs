using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ButtonCloset : ButtonBase
{
    [SerializeField]
    private GameObject _closetWrapInsideCamera;

    public override void ExtraLogicPageOn()
    {
        base.ExtraLogicPageOn();

        _closetWrapInsideCamera.SetActive(true);
    }

    public override void ExtraLogicPageOff()
    {
        base.ExtraLogicPageOff();

        _closetWrapInsideCamera.SetActive(false);
    }
}
