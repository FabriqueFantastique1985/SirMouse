using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButtonMenu : MonoBehaviour
{
    [SerializeField]
    private SkinPieceElement _mySkin;

    [SerializeField]
    private Animation _myAnimation;

    public void ClickedButton()
    {
        // use controller to equip the skinpiece element on this
        SkinControllerMainMenu.Instance.EquipSkinMenu(_mySkin);

        // animate click
        _myAnimation.Play();
    }

    private void OnEnable()
    {
        // pop into existence

    }
}
