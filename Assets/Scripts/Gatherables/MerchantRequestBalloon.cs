using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRequestBalloon : MonoBehaviour
{
    [Header("Button references")]
    public List<MerchantRequestButton> MerchantRequestButtons = new List<MerchantRequestButton>();


    // called on MerchantRequest when creating a balloon
    public void SetButtonValues(Merchant merchant, DeliverableResource resourceDeliverable, int index)
    {
        MerchantRequestButtons[index].SetMerchantRequestButtonValuesAndSprites(merchant, resourceDeliverable);
    }
}
