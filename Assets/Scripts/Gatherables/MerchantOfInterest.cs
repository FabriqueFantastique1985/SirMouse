using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantOfInterest : PlaceOfInterest
{
    [Header("Merchant parent")]
    [SerializeField]
    private Merchant _merchant;


    public override void HideIcon()
    {
        base.HideIcon();

        _merchant.ShowCorrectRequest();
    }
    public override void ShowIcon()
    {
        base.ShowIcon();

        _merchant.HideCurrentRequest();
    }
}
