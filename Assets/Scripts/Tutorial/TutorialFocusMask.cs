using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TutorialFocusMask
{
    private const string _tutorialFocusTag = "TutorialFocus";

    /// <summary>
    /// Will initialize focusmask in case it has not been set in inspecor. 
    /// Will look for an object with tag "tutorial focus" which has a RectTransform component
    /// </summary>
    public void Initialize(ref RectTransform focusMask)
    {
        if (!focusMask)
        {
            try
            {
                SearchFocusMask(ref focusMask);
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

    private void SearchFocusMask(ref RectTransform focusMask)
    {
        var go = GameObject.FindGameObjectWithTag(_tutorialFocusTag);
        RectTransform transform = go.GetComponent<RectTransform>();
        if (transform == null)
            throw new MissingComponentException();
        focusMask = transform;
    }
}
