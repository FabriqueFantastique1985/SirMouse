using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityCore.Scene;
using UnityEngine;

public class ButtonQuit : ButtonBaseNew
{
    public override void ClickedButton()
    {
        base.ClickedButton();

        // hide close the page, hide ui
        PageController.Instance.TurnPageOff(PageType.Pause);
        PageController.Instance.FullyHideUIButtons(true);

        // use scene controller to load main menu
        SceneController.Instance.StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(0.2f);
        SceneController.Instance.Load(SceneType.MainMenu);
    }
}
