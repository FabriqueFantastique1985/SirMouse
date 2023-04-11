﻿using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
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
        ClickNew(player, this);
    }
    public void ClickNew(Player player, MerchantRequestButton buttonClicked)
    {
        ResourceController.Instance.RemoveResource(ResourceToDeliver.ResourceType);

        CompletedThisButton(_spriteFullParent);

        _myMerchant.CurrentRequest.DeliverResource(ResourceToDeliver.ResourceType, buttonClicked);

        _myMerchant.CurrentRequest.ClickedButtonOfResourceX(ResourceToDeliver.ResourceType);

        _myMerchant.CurrentRequest.CheckRequestFinished();

        AudioController.Instance.PlayAudio(_myMerchant.AudioMerchantButtonClicked);
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
        //Debug.Log("Activated pulsing with state = " + state + " for button " + this.gameObject.name);
        _spriteTransparentParentAnimation.enabled = state;
        if (_spriteTransparentParentAnimation.transform.GetChild(0).TryGetComponent(out Animation animationComponentColorPulse))
        {
            animationComponentColorPulse.enabled = state;
        }

        // reset alphas
        if (state == false)
        {
            for (int i = 0; i < _spriteTransparentParentAnimation.transform.childCount; i++)
            {
                var chosenTransparentButton = _spriteTransparentParentAnimation.transform.GetChild(i);
                // get all sprite render children
                SpriteRenderer[] spriteChildren = chosenTransparentButton.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < spriteChildren.Length; j++)
                {
                    spriteChildren[j].color = new Color(255, 255, 255, 0.5f);
                }

            }
        }
        
    }
    public void ActivateCollider(bool state)
    {
        _buttonCollider.enabled = state;
    }



    private void SetMerchantRequestButtonValues(DeliverableResource deliverableResource)
    {
        //ResourceToDeliver = deliverableResource; // does this work ?
        ResourceToDeliver.ResourceType = deliverableResource.ResourceType;
        ResourceToDeliver.Delivered = deliverableResource.Delivered;
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
            Debug.Log("finished delivering on button " +  this.gameObject.name + ", cuzz my resource is set on -> " + ResourceToDeliver.ResourceType + " --- " + ResourceToDeliver.Delivered);
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
