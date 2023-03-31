using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [Header("Merchant Request Children")]
    [SerializeField]
    private List<MerchantRequest> _merchantRequests = new List<MerchantRequest>();
    private MerchantRequest _currentRequest;

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
        DecideCurrentRequest();
        ShowCorrectRequest();
    }


    // called in merchantOfInterest
    public void ShowCorrectRequest()
    {
        _currentRequest.ShowRequest();
    }
    public void HideCurrentRequest()
    {
        if (_currentRequest != null)
        {
            _currentRequest.HideRequest();
        }   
    }


    private void DecideCurrentRequest()
    {
        for (int i = 0; i < _merchantRequests.Count; i++)
        {
            if (_merchantRequests[i].CompletedRequest == false)
            {
                _currentRequest = _merchantRequests[i];
                break;
            }
        }
    }
}
