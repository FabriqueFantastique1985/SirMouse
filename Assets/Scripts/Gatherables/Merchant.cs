using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class MerchantData
{
    public int CurrentRequestIndex;
    public List<bool> DeliverableResourcesFound;
}


public class Merchant : MonoBehaviour, IDataPersistence
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

    public void LoadData(GameData data)
    {
        var merchantData = data.MerchantData[gameObject.GetInstanceID()];
        //_merchantRequests
    }

    public void SaveData(ref GameData data)
    {
        MerchantData merchantData = new MerchantData();
        // fill in merchant data
        //merchantData.

        data.MerchantData[gameObject.GetInstanceID()] = merchantData;
    }
}


