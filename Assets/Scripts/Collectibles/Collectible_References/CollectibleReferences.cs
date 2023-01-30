using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleReferences : MonoBehaviour
{
    [HideInInspector]
    public Vector3 StartPosition;

    public CollectibleType TypeOfCollectible;

    public Animator ShelfToUse;
    public Animator ObjectToSlotIn;
    public Image ImageComponentObjectToSlotIn;
  
    public float MoveSpeed;

    [HideInInspector]
    public float Step;

    public List<CollectibleElementBase> CollectibleSlots = new List<CollectibleElementBase>();

    [HideInInspector]
    public GameObject SlotToUse;


    private void Start()
    {
        Step = MoveSpeed * Time.deltaTime;

        StartPosition = ObjectToSlotIn.transform.position;
    }
}

