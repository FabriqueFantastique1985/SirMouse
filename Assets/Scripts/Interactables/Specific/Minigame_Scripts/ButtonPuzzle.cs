using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPuzzle : ButtonBaseNew
{
    [SerializeField]
    private DragAndDrop _dragAndDropScript;

    [SerializeField]
    private bool _iCloseTheGame;

    [SerializeField]
    private GameObject ObjectToActivate;

    public override void ClickedButton()
    {
        base.ClickedButton();

        if (ObjectToActivate.activeSelf == true)
        {
            ObjectToActivate.SetActive(false);

            if (_iCloseTheGame == true)
            {
                _dragAndDropScript.EndMiniGame();
            }
        }
        else
        {
            ObjectToActivate.SetActive(true);
        }
    }
}
