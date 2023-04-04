using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRequest : MonoBehaviour, IDataPersistence
{
    [Header("Merchant parent")]
    [SerializeField]
    private Merchant _merchant;

    [Header("Appoint resources here")]
    [SerializeField]
    private List<DeliverableResource> _requestedResources = new List<DeliverableResource>(); // save this

    private MerchantRequestBalloon _myBalloon;

    [Header("Reward")]
    [SerializeField]
    private List<SkinPieceElement> _skinRewards = new List<SkinPieceElement>();

    [Header("Assign object in which the deliver balloon gets instantiated")]
    [SerializeField]
    private GameObject BalloonDeliveryParent;

    [Header("Statuses")]
    public bool CompletedRequest; 
    public bool ShowingRequest;

    /// <summary>
    /// THIS START NEEDS TO HAPPEN BEFORE THE OTHER STARTS related to the merchant !!!
    /// </summary>
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
        for (int i = 0; i < _requestedResources.Count; i++)
        {
            if (_requestedResources[i].ResourceType == resourceType && _requestedResources[i].Delivered == false)
            {
                _requestedResources[i].Delivered = true;              
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
    public void ClickedButtonOfResourceX(Type_Resource resourceType)
    {
        // check the buttons of the same resourceType...
        // if there's more than 0, check if their resource is still something the player has...
        // if not, disable the pulsing and the collider


        // create temp list of buttons with a same type as the one clicked...
        List<MerchantRequestButton> buttonsToDisablePulsing = new List<MerchantRequestButton>();
        for (int i = 0; i < _myBalloon.MerchantRequestButtons.Count; i++)
        {
            var chosenButton = _myBalloon.MerchantRequestButtons[i];
            if (chosenButton.ResourceToDeliver.Delivered == false )
            {
                if (chosenButton.ResourceToDeliver.ResourceType == resourceType)
                {
                    buttonsToDisablePulsing.Add(chosenButton);
                }
            }
        }
        // if I do not have any of this resource left -> disable the similar buttons
        if (ResourceController.Instance.CheckIfIStillHaveMoreOfThisResource(resourceType) == false)
        {
            if (buttonsToDisablePulsing.Count > 0)
            {
                for (int i = 0; i < buttonsToDisablePulsing.Count; i++)
                {
                    buttonsToDisablePulsing[i].ActivatePulsing(false);
                    buttonsToDisablePulsing[i].ActivateCollider(false);
                }
            }
        }
    }



    private void CreatDeliveryBalloon()
    {
        GameObject createdBalloon = Instantiate(_merchant.BalloonDeliveryPrefabs[_requestedResources.Count - 1], BalloonDeliveryParent.transform);
        MerchantRequestBalloon merchantBalloon = createdBalloon.GetComponent<MerchantRequestBalloon>();
        _myBalloon = merchantBalloon;

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



    public void LoadData(GameData data)
    {

    }
    public void SaveData(ref GameData data)
    {

    }
}
