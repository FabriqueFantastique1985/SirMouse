using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRequestButton : MonoBehaviour, IClickable
{
    [Header("Values automatically set on Start")]
    public DeliverableResource ResourceToDeliver;

    [Header("Collider component")]
    [SerializeField]
    private Collider _buttonCollider;

    [Header("References in children")]
    [SerializeField]
    private Animation _spriteTransparentParentAnimation;
    [SerializeField]
    private GameObject _spriteFullParent;
    [SerializeField]
    private GameObject _notificationObject;

    private Merchant _myMerchant;


    // * button collider should be DISABLED unless the transparent sprite is hovering
    // * enable the animation component of the sprite visual when the players has it available 
    // * pulse transparent sprites if Players has them in backpack * DONE


    public void Click(Player player)
    {
        ResourceController.Instance.RemoveResource(ResourceToDeliver.ResourceType);
        CompletedThisButton(_spriteFullParent);

        _myMerchant.CurrentRequest.DeliverResource(ResourceToDeliver.ResourceType);
        _myMerchant.CurrentRequest.ClickedButtonOfResourceX(ResourceToDeliver.ResourceType);

        _myMerchant.CurrentRequest.CheckRequestFinished();
    }


    // called in MerchantRequestBalloon balloon creation
    public void SetMerchantRequestButtonValuesAndSprites(Merchant merchant, DeliverableResource deliverableResource)
    {
        _myMerchant = merchant;
        SetMerchantRequestButtonValues(deliverableResource);
        SetMerchantRequestButtonSprites(merchant, deliverableResource);
    }

    public void ActivatePulsing(bool state)
    {
        _spriteTransparentParentAnimation.enabled = state;
    }
    public void ActivateCollider(bool state)
    {
        _buttonCollider.enabled = state;
    }


    private void SetMerchantRequestButtonValues(DeliverableResource deliverableResource)
    {
        ResourceToDeliver = deliverableResource; // does this work ?
    }
    private void SetMerchantRequestButtonSprites(Merchant merchant, DeliverableResource deliverableResource)
    {
        GameObject spriteFull = merchant.ResourceButtonVisualPrefabsFull[((int)deliverableResource.ResourceType) - 1];
        GameObject spriteTransparent = merchant.ResourceButtonVisualPrefabsTransparent[((int)deliverableResource.ResourceType) - 1];

        GameObject spriteFullObj = Instantiate(spriteFull, _spriteFullParent.transform);
        GameObject spriteTransparentObj = Instantiate(spriteTransparent, _spriteTransparentParentAnimation.transform);

        CreateTransparentVisual(spriteTransparentObj);

        if (ResourceToDeliver.Delivered == true)
        {
            Debug.Log("finished delivering " + deliverableResource + ", cuzz my resource is set on -> " + ResourceToDeliver.ResourceType + " --- " + ResourceToDeliver.Delivered);
            CompletedThisButton(spriteFullObj);
        }
    }

    private void CreateTransparentVisual(GameObject spriteTransparent)
    {
        spriteTransparent.SetActive(true);
    }
    private void CompletedThisButton(GameObject spriteFull)
    {
        // disable the collider   
        _buttonCollider.enabled = false;

        // enable the notif
        _notificationObject.SetActive(true);

        // disable the pulsing object
        _spriteTransparentParentAnimation.gameObject.SetActive(false);

        // enable the sprite
        _spriteFullParent.SetActive(true);       
        spriteFull.SetActive(true);
    }



}
