using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class QuickBackpackButton : MonoBehaviour
{
    [SerializeField]
    PageType TurnOnThisPage;

    // do the animator differently !!!
    [SerializeField]
    private Animator ButtonAnimator;

    public void Clicked()
    {
        if (PageController.Instance.PageIsOn(TurnOnThisPage) == true)
        {
            PageController.Instance.TurnPageOff(TurnOnThisPage);
        }
        else
        {
            PageController.Instance.TurnPageOn(TurnOnThisPage);
        }

        ButtonAnimator.Play("Popout_Backpack");
    }
    
}
