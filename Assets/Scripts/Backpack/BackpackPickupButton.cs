using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPickupButton : MonoBehaviour
{
    public GameObject MyInteractable;
    public Type_Pickup MyPickupType;

    public Image MyImage;


    public void Clicked()
    {        
        BackpackController.BackpackInstance.RemoveItemFromBackpack(MyInteractable, MyPickupType, this.gameObject);
    }
}
