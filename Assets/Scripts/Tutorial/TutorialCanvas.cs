using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas
{
    private const string _tutorialFocusTag = "TutorialFocus";

    /// <summary>
    /// Will initialize focusmask in case it has not been set in inspecor. 
    /// Will look for an object with tag "tutorial focus" which has a RectTransform component
    /// </summary>
    public void Initialize(ref Canvas canvas)
    {
        if (!canvas)
        {
            try
            {
                SearchFocusMask(ref canvas);
            }
            catch (NullReferenceException e)
            {
                Debug.LogException(e);
            }
            catch (MissingComponentException e)
            {
                Debug.LogException(e);
            }
        }
    }

    private void SearchFocusMask(ref Canvas canvas)
    {
        var go = GameObject.FindGameObjectWithTag(_tutorialFocusTag);
        Canvas foundCanvas = go.GetComponentInParent<Canvas>();
        if (foundCanvas == null)
            throw new MissingComponentException();
        canvas = foundCanvas;
    }
}
