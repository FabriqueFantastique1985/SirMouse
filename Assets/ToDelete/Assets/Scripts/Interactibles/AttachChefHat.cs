using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachChefHat : MonoBehaviour
{
    public InteractionCooking InteractionCooking;

    public void AttachHat()
    {
        InteractionCooking.AttachChefHat();
    }
}
