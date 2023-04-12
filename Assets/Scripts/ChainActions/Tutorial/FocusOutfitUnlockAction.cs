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
        Focus = buttonSkinpieceType.transform;
    }

    private void OnDestroy()
    {
        ClosetController.Instance.OnSkinpieceUnlocked -= SetFocus;        
    }
}
