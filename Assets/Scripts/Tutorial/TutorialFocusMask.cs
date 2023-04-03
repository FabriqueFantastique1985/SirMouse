using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TutorialFocusMask
{
    [SerializeField] private RectTransform _focusMask;
    private const string _tutorialFocusTag = "TutorialFocus";

    public RectTransform Mask
    {
        get { return _focusMask; }
        set { _focusMask = value; }
    }

    /// <summary>
    /// Will initialize focusmask in case it has not been set in inspecor. 
    /// Will look for an object with tag "tutorial focus" which has a RectTransform component
    /// </summary>
    public void Initialize()
    {
        if (!_focusMask)
        {
            try
            {
                SearchFocusMask();
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

    private void SearchFocusMask()
    {
        var go = GameObject.FindGameObjectWithTag(_tutorialFocusTag);
        RectTransform transform = go.GetComponent<RectTransform>();
        if (transform == null)
            throw new MissingComponentException();
        _focusMask = transform;
    }
}
