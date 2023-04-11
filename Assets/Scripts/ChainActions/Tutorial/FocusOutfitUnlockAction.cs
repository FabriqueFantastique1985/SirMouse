using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusOutfitUnlockAction : FocusAction
{
    protected override void Start()
    {
        base.Start();

        ClosetController.Instance.OnSkinpieceUnlocked += SetFocus;
    }

    private void SetFocus(ButtonClosetOpenSpecificPieces buttonSkinpieceType)
    {
        ClosetController.Instance.OnSkinpieceUnlocked -= SetFocus;
        Focus = buttonSkinpieceType.transform;
    }
}
