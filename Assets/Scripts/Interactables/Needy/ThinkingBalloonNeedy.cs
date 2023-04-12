using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThinkingBalloonNeedy : MonoBehaviour
{
    #region EditorFields

    [Header("Needy Balloon sprites")]
    public ListNeedyBalloons Needy_Sprites_Wrap;

    #endregion




    #region Methods

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }



    public bool CheckIfOneBalloonHasAllItems(ListNeedyObjectsInMe balloonOfInterest)
    {
        for (int i = 0; i < balloonOfInterest.NeedyObjects.Count; i++)
        {
            if (balloonOfInterest.NeedyObjects[i].Delivered == false)
            {
                return false;
            }
        }
        balloonOfInterest.CompletedMe = true;
        return true;
    }
    public void UpdateOneRequiredTouchable()
    {
        var needyBalloons = Needy_Sprites_Wrap.NeedyBalloons;

        // get the 1 balloon of interest...(always is 1 balloon for touchables currently -> index 0 needyBlaloons)
        // update 1 of the nondelivered object within it...
        for (int i = 0; i < needyBalloons[0].NeedyObjects.Count; i++)
        {
            if (needyBalloons[0].NeedyObjects[i].Delivered == false)
            {
                needyBalloons[0].NeedyObjects[i].Delivered = true;
                needyBalloons[0].NeedyObjects[i].SpriteFull.SetActive(true);

                break;
            }
        }
    }
    public void ResetMyNeedyObjects()
    {
        // get all componentsChildren of NeedyBalloons, add all of them to a the NeedyBalloons list again
        // go through all my balloons, and all of their NeedyObjects , reset bools balloon and needyobject, reset spriteFull
        Needy_Sprites_Wrap.NeedyBalloons = Needy_Sprites_Wrap.GetComponentsInChildren<ListNeedyObjectsInMe>().ToList();
        for (int i = 0; i < Needy_Sprites_Wrap.NeedyBalloons.Count; i++)
        {
            var balloonToReset = Needy_Sprites_Wrap.NeedyBalloons[i];
            for (int j = 0; j < balloonToReset.NeedyObjects.Count; j++)
            {
                var needyToReset = balloonToReset.NeedyObjects[j];
                needyToReset.SpriteFull.SetActive(false);
                needyToReset.Delivered = false;
            }
            balloonToReset.CompletedMe = false;
        }
    }

    #endregion
}
