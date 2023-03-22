using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonSkinPiece : ButtonBaseNew
{
    public SkinPieceElement MySkinPieceElement;

    public GameObject MySpriteToActivateWhenFound;
    public GameObject MySpriteToDuplicateAndMove;

    [Header("Status-es")]
     
    [SerializeField, FormerlySerializedAs("Found")]
    private bool _found; // set -> GiveReward()
    
    
    public bool TriedThisOut; // set -> when clicking the button
    public bool HasBeenNotified; // set -> GiveReward() (but later on)

    public bool Found 
    {
        get { return _found; }
        set
        {
            _found = value;
            DataPersistenceManager.Instance.SaveGame();
        }
    }
    
    public GameObject NotificationObject;


    public override void ClickedButton()
    {
        // if this is a skinPiece I have found...
        if (_found == true)  
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
