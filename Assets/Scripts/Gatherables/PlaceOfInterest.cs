using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOfInterest : MonoBehaviour
{
    [Header("Icon Visual")]
    [SerializeField]
    private GameObject _icon;
    [SerializeField]
    private GameObject _iconSpriteParentAnimator;

    [Header("Colliders")]
    [SerializeField]
    private Collider _triggerToEnterToSeeRequest;
    [SerializeField]
    private Collider _triggerToExitToSeeIcon;

    [Header("Status")]
    public bool CompletedMe = false; 


    protected virtual void Start()
    {
        // initialize save data first


        if (CompletedMe == true)
        {
            HideIconPermanently();
        }
        else
        {
            ShowIcon();
        }
    }


    public virtual void ShowIcon()
    {
        _icon.SetActive(true);

        _triggerToEnterToSeeRequest.enabled = true;
        _triggerToExitToSeeIcon.enabled = false;
    }
    public virtual void HideIcon()
    {        
        _icon.SetActive(false);

        _triggerToEnterToSeeRequest.enabled = false;
        _triggerToExitToSeeIcon.enabled = true;
    }
    public virtual void HideIconPermanently()
    {
        CompletedMe = true;

        _icon.SetActive(false);

        _triggerToEnterToSeeRequest.enabled = false;
        _triggerToExitToSeeIcon.enabled = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        HideIcon();

        // figure out what request to show
    }
    private void OnTriggerExit(Collider other)
    {
        ShowIcon();
    }
}
