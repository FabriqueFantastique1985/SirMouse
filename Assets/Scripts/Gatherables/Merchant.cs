using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [Header("Merchant of Interest child")]
    [SerializeField]
    private MerchantOfInterest _merchantOfInterest;

    [Header("Merchant Request Children")]
    [SerializeField]
    private List<MerchantRequest> _merchantRequests = new List<MerchantRequest>();

    [Header("Audio")]
    public AudioElement AudioMerchantBalloonPop;
    public AudioElement AudioMerchantButtonClicked;

    [HideInInspector]
    public MerchantRequest CurrentRequest;

    [Header("Balloon Delivery prefabs")]
    public List<GameObject> BalloonDeliveryPrefabs = new List<GameObject>();

    [Header("Prefabs visuals in buttons")]
    // prefabs of the premade visuals of the resources
    public List<GameObject> ResourceButtonVisualPrefabsFull = new List<GameObject>();
    public List<GameObject> ResourceButtonVisualPrefabsTransparent = new List<GameObject>();


    protected virtual void Start()
    {

        DecideCurrentRequest();
    }


    // called when completing a request
    public void ShowNewRequest()
    {
        // hide the current one
        CurrentRequest.HideRequest();
        // find the new request
        DecideCurrentRequest();
        // show the new request
        ShowCorrectRequest();
    }


    // called in merchantOfInterest
    public void ShowCorrectRequest()
    {
        if (CurrentRequest != null)
        {
            CurrentRequest.ShowRequest();

            // play sound effect
            AudioController.Instance.PlayAudio(AudioMerchantBalloonPop);
        }      
    }
    public void HideCurrentRequest()
    {
        if (CurrentRequest != null)
        {
            CurrentRequest.HideRequest();
        }   
    }


    private void DecideCurrentRequest()
    {
        CurrentRequest = null;
        for (int i = 0; i < _merchantRequests.Count; i++)
        {
            if (_merchantRequests[i].CompletedRequest == false)
            {
                CurrentRequest = _merchantRequests[i];
                break;
            }
        }

        if (CurrentRequest == null)
        {
            _merchantOfInterest.CompletedMe = true;
            _merchantOfInterest.HideIconPermanently();
        }
    }
}
