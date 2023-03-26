using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEquipToggle : ButtonBaseNew
{
    [Header("Components")]
    public Button ButtonComponent;
    public Collider ColliderComponent;

    [Header("Reference children")]
    public GameObject ImageInsideButtonState0;
    public GameObject ImageInsideButtonState1;

    public override void ClickedButton()
    {
        base.ClickedButton();

        // additional equip logic
    }
}
