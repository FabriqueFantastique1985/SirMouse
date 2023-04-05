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
    public List<DeliverableResource> RequestedResources => _requestedResources;

    private MerchantRequestBalloon _myBalloon;

    [Header("Reward")]
    [SerializeField]
    private RewardList _rewards;

    [Header("Assign object in which the deliver balloon gets instantiated")]
    [SerializeField]
    private GameObject BalloonDeliveryParent;

    [Header("Statuses")]
    public bool CompletedRequest; 
    public bool ShowingRequest;


    protected virtual void Start()
    {
        //CompletedRequest = CheckRequestCompletion();
        //if (CompletedRequest == false)
        //{
            // instantiate the balloon, assign values
            CreatDeliveryBalloon();
        //}

        CompletedRequest = CheckRequestCompletion();
        if (CompletedRequest == true)
        {
            // perma hide the icon/triggers
            _merchant.FinishedMerchant();
        }
    }





    // called on the button press of the resource
    public void DeliverResource(Type_Resource resourceType, MerchantRequestButton buttonPressed)
    {
        // find non-delivered resource to give
        for (int i = 0; i < _requestedResources.Count; i++)
        {
            if (_requestedResources[i].ResourceType == resourceType && _requestedResources[i].Delivered == false)
            {
                _requestedResources[i].Delivered = true;
               // _myBalloon.MerchantRequestButtons[i].ResourceToDeliver.Delivered = true;
                buttonPressed.ResourceToDeliver.Delivered = true;


                break;
            }
        }

    }
    public void CheckRequestFinished()
    {
        // check for completion
        if (CheckRequestCompletion() == true)
        {
            // set bool
            CompletedRequest = true;

            // increase index of merchantRequests on Merchant
            _merchant.CurrentRequestIndex += 1;

            // give rewards
            RewardController.Instance.GiveReward(_rewards.Rewards);

            // show other request
            _merchant.ShowNewRequest();
        }
    }
    public void ClickedButtonOfResourceX(Type_Resource resourceType)
    {
        // check the buttons of the same resourceType...
        // if there's more than 0, check if their resource is still something the player has...
        // if not, disable the pulsing and the collider


        // create temp list of buttons with a same type as the one clicked...
        List<MerchantRequestButton> buttonsToCheckDisablePulsing = new List<MerchantRequestButton>();
        for (int i = 0; i < _myBalloon.MerchantRequestButtons.Count; i++)
        {
            var chosenButton = _myBalloon.MerchantRequestButtons[i];
            if (chosenButton.ResourceToDeliver.Delivered == false)
            {
                if (chosenButton.ResourceToDeliver.ResourceType == resourceType)
                {
                    Debug.Log(chosenButton.gameObject.name + " button selected to disable pulse");
                    buttonsToCheckDisablePulsing.Add(chosenButton);
                }
            }
        }
        // if I do not have any of this resource left -> disable the similar buttons
        if (ResourceController.Instance.CheckIfIStillHaveMoreOfThisResource(resourceType) == false)
        {
            if (buttonsToCheckDisablePulsing.Count > 0)
            {
                for (int i = 0; i < buttonsToCheckDisablePulsing.Count; i++)
                {
                    buttonsToCheckDisablePulsing[i].ActivatePulsing(false);
                    buttonsToCheckDisablePulsing[i].ActivateCollider(false);
                }
            }
        }
    }



    // called on Merchant
    public void ShowRequest()
    {
        if (ShowingRequest == false)
        {
            BalloonDeliveryParent.SetActive(true);

            CheckWhetherPlayerHasResourcesOfInterest();

            ShowingRequest = true;
        }
    }
    public void HideRequest()
    {
        BalloonDeliveryParent.SetActive(false);
        ShowingRequest = false;

        // extra nullcheck
        if (_myBalloon == null)
        {
            _myBalloon = BalloonDeliveryParent.GetComponentInChildren<MerchantRequestBalloon>(true);
        }

        for (int i =0; i < _myBalloon.MerchantRequestButtons.Count; i++)
        {
            _myBalloon.MerchantRequestButtons[i].ActivatePulsing(false);
            _myBalloon.MerchantRequestButtons[i].ActivateCollider(false);
        }
    }



    private void CheckWhetherPlayerHasResourcesOfInterest()
    {
        // if I have resources of interest, pulse the transparent buttons...
        // check for the buttons, in the merchantballoon, of the chosen request
        var buttonsToPulse = ResourceController.Instance.CheckAquiredResources(_myBalloon.MerchantRequestButtons);

        for (int i = 0; i < buttonsToPulse.Count; i++)
        {
            buttonsToPulse[i].ActivatePulsing(true);
            buttonsToPulse[i].ActivateCollider(true);
        }
    }
    private void CreatDeliveryBalloon()
    {
        GameObject createdBalloon = Instantiate(_merchant.BalloonDeliveryPrefabs[_requestedResources.Count - 1], BalloonDeliveryParent.transform);
        MerchantRequestBalloon merchantBalloon = createdBalloon.GetComponent<MerchantRequestBalloon>();
        _myBalloon = merchantBalloon;

        //Debug.Log("CREATED BALLOON FOR + " + _merchant.name);

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
