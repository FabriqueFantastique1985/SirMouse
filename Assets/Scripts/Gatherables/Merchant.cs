using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class MerchantData
{
    public int CurrentRequestIndex;
    public List<bool> DeliverableResourcesFound = new List<bool>();
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
    [HideInInspector]
    public int CurrentRequestIndex;

    [Header("Balloon Delivery prefabs")]
    public List<GameObject> BalloonDeliveryPrefabs = new List<GameObject>();

    [Header("Prefabs visuals in buttons")]
    // prefabs of the premade visuals of the resources
    public List<GameObject> ResourceButtonVisualPrefabsFull = new List<GameObject>();
    public List<GameObject> ResourceButtonVisualPrefabsTransparent = new List<GameObject>();


    protected virtual void Start()
    {
        // load...
        // 1) get the data.requestIndex, assign in to currentRequestIndex
        // 2) then access the correct merchantRequest through the index...
        //    ... update this requests' delivarables with new bools (create delivery balloon should automaticly fix the visual aspect)


        // save...
        // 1) on scene-loaded + onApplicationQuit (alrdy happens)

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

        if (CurrentRequestIndex < _merchantRequests.Count)
        {
            CurrentRequest = _merchantRequests[CurrentRequestIndex];
        }

        if (CurrentRequest == null)
        {
            _merchantOfInterest.CompletedMe = true;
            _merchantOfInterest.HideIconPermanently();
        }
    }
    public void FinishedMerchant()
    {
        _merchantOfInterest.CompletedMe = true;
        _merchantOfInterest.HideIconPermanently();
    }


    public void LoadData(GameData data)
    {
        if (data.MerchantSavedData.ContainsKey(gameObject.name))
        {
            // get the correct key...
            var merchantData = data.MerchantSavedData[gameObject.name];

            // get the value (MerchantData, assign the RequestIndex)

            // if my index is within range (uncompleted)
            if (merchantData.CurrentRequestIndex < _merchantRequests.Count)
            {
                CurrentRequestIndex = merchantData.CurrentRequestIndex;

                // get the value (MerchantData, assign the bools of the resources)
                CurrentRequest = _merchantRequests[CurrentRequestIndex];

                for (int i = 0; i < CurrentRequest.RequestedResources.Count; i++)
                {
                    CurrentRequest.RequestedResources[i].Delivered = merchantData.DeliverableResourcesFound[i];
                }
            }
            else // else if my index is out of range (completed)
            {
                CurrentRequestIndex = merchantData.CurrentRequestIndex - 1;

                // get the value (MerchantData, assign the bools of the resources)
                CurrentRequest = _merchantRequests[CurrentRequestIndex];

                for (int i = 0; i < CurrentRequest.RequestedResources.Count; i++)
                {
                    CurrentRequest.RequestedResources[i].Delivered = merchantData.DeliverableResourcesFound[i];
                }
            }

        }
    }

    public void SaveData(ref GameData data)
    {
        MerchantData merchantData = new MerchantData();

        // fill in RequestIndex
        merchantData.CurrentRequestIndex = CurrentRequestIndex;

        Debug.Log("current request index is " + CurrentRequestIndex + ", on merchant" + this.name);

        // if my index is within range -> normal behavior
        if (CurrentRequestIndex < _merchantRequests.Count)
        {
            CurrentRequest = _merchantRequests[CurrentRequestIndex];

            // fill in the bools
            // step (0) clear the list 
            merchantData.DeliverableResourcesFound.Clear();
            for (int i = 0; i < CurrentRequest.RequestedResources.Count; i++)
            {
                // first add to the list...
                merchantData.DeliverableResourcesFound.Add(false);

                // then assign its value
                merchantData.DeliverableResourcesFound[i] = CurrentRequest.RequestedResources[i].Delivered;
            }

            // update the data 
            data.MerchantSavedData[gameObject.name] = merchantData;
        }
        else
        {
            // else if my index is out of range --> completed the previous' index request

            CurrentRequest = _merchantRequests[CurrentRequestIndex - 1];

            // fill in the bools
            // step (0) clear the list 
            merchantData.DeliverableResourcesFound.Clear();
            for (int i = 0; i < CurrentRequest.RequestedResources.Count; i++)
            {
                // first add to the list...
                merchantData.DeliverableResourcesFound.Add(false);

                // then assign its value
                merchantData.DeliverableResourcesFound[i] = CurrentRequest.RequestedResources[i].Delivered;
            }

            // update the data 
            data.MerchantSavedData[gameObject.name] = merchantData;
        }
    }
}


