using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRequest : MonoBehaviour
{
    [Header("Merchant parent")]
    [SerializeField]
    private Merchant _merchant;

    [Header("Appoint resources here")]
    [SerializeField]
    private List<DeliverableResource> _requestedResources = new List<DeliverableResource>(); // save this

    [Header("Reward")]
    private List<SkinPieceElement> _skinRewards = new List<SkinPieceElement>();

    [Header("Assign object in which the deliver balloon gets instantiated")]
    [SerializeField]
    private GameObject BalloonDeliveryParent;

    [Header("Statuses")]
    public bool CompletedRequest; 
    public bool ShowingRequest;


    protected virtual void Start()
    {
        // save data first


        CompletedRequest = CheckRequestCompletion();
        if (CompletedRequest == false)
        {
            // instantiate the balloon, assign values
            CreatDeliveryBalloon();
        }
    }





    // called on the button press of the resource
    public void DeliverResource(Type_Resource resourceType)
    {
        // find non-delivered resource to give
        bool foundResourceToFill = false;
        for (int i = 0; i < _requestedResources.Count; i++)
        {
            if (_requestedResources[i].ResourceType == resourceType && _requestedResources[i].Delivered == false)
            {
                _requestedResources[i].Delivered = true;
                foundResourceToFill = true;                
                break;
            }
        }

        // debug lines for help
        if (foundResourceToFill == true)
        {
            Debug.Log("succesfully filled resource");
        }
        else
        {
            Debug.Log("FAILED to fill resource");
        }

        // check for completion
        if (CheckRequestCompletion() == true)
        {
            // set bool
            CompletedRequest = true;

            // give rewards
            RewardController.Instance.GiveReward(_skinRewards);

            // show other request
            _merchant.ShowNewRequest();
        }
    }


    // called on Merchant
    public void ShowRequest()
    {
        BalloonDeliveryParent.SetActive(true);

        CheckWhetherPlayerHasResourcesOfInterest();

        ShowingRequest = true;
    }
    public void HideRequest()
    {
        BalloonDeliveryParent.SetActive(false);
        ShowingRequest = false;
    }

    private void CheckWhetherPlayerHasResourcesOfInterest()
    {
        // if I have resources of interest, pulse the transparent buttons
        ResourceController.Instance.CheckAquiredResources(_requestedResources);
    }


    private void CreatDeliveryBalloon()
    {
        GameObject createdBalloon = Instantiate(_merchant.BalloonDeliveryPrefabs[_requestedResources.Count - 1], BalloonDeliveryParent.transform);
        MerchantRequestBalloon merchantBalloon = createdBalloon.GetComponent<MerchantRequestBalloon>();

        for (int i = 0; i < _requestedResources.Count; i++)
        {
            merchantBalloon.SetButtonValues(_merchant, _requestedResources[i], i);
        }

    }
    private bool CheckRequestCompletion()
    {
        for (int i = 0; i < _requestedResources.Count; i++)
        {
            if (_requestedResources[i].Delivered == false)
            {
                return false;
            }
        }
        return true;
    }
}
