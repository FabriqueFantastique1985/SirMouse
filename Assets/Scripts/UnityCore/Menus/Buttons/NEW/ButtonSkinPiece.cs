using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonSkinPiece : ButtonBaseNew
{
    public SkinPieceElement MySkinPieceElement;

    public GameObject MySpriteToActivateWhenFound;
    public GameObject MySpriteToDuplicateAndMove;

    [FormerlySerializedAs("_data")]
    [Header("Status-es")]

    [HideInInspector]
    public ButtonSkinPieceData Data;
    
    public bool TriedThisOut; // set -> when clicking the button
    public bool HasBeenNotified; // set -> GiveReward() (but later on)

    public bool Found 
    {
        get { return Data.Found; }
        set
        {
            Data.Found = value;
        }
    }
    
    public GameObject NotificationObject;


    public override void ClickedButton()
    {
        // if this is a skinPiece I have found...
        if (Data.Found == true)  
        {
            // change notification status
            ClosetController.Instance.NotificationRemover(this);

            // if I don't already have a skinPiece on my finger...
            if (ClosetController.Instance.ActivatedFollowMouse == false)
            {
                // anim + sound on base
                base.ClickedButton();

                // create copy of skinPiece
                ClosetController.Instance.ClickedSkinPieceButton(MySpriteToDuplicateAndMove, MySkinPieceElement, this.transform.position);
            }
        }
    }

    public override void Click(Player player)
    {
        base.Click(player);
    }
}

[Serializable]
public class ButtonSkinPieceData
{
    public bool Found;
}
