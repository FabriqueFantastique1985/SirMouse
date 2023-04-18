using System;
using UnityEngine;

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
        try
        {
            SearchFocusMask(ref focusMask);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Caught a null reference searching for the focus mask.");
            Debug.LogException(e);
        }
        catch (MissingComponentException e)
        {
            Debug.LogError("Caught a missing component searching for the focus mask.");
            Debug.LogException(e);
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

    public Vector3 GetWorldPosToCameraPos(Vector3 focusPosition)
    {
        //Ray ray = Camera.allCameras[0].ScreenPointToRay(Camera.allCameras[0].WorldToScreenPoint(focusPosition));
        //return ray.GetPoint(1f);

        return Camera.allCameras[0].WorldToScreenPoint(focusPosition);
    }
}
