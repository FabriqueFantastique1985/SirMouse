using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // assign these in the inspector
    public Animation MyAnimationComponent;

    public GameObject ObjectIngredient;
    public GameObject ObjectStatus;

    public SpriteRenderer SpriteRendererIngredient;
    //public SpriteRenderer SpriteRendererStatus;

    // these are assigned at runtime
    public Type_Ingredient TypeOfIngredient;

    public bool Hidden = false;
    public bool HasBeenGiven = false;

    

}
