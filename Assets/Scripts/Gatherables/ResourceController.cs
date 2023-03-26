using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; private set; }


    [Header("Backpack Resource slots")]
    [SerializeField]
    private List<SlotResource> _slotsResources = new List<SlotResource>();
    private List<SlotResource> _slotsResourcesTaken = new List<SlotResource>();

    //[Header("Prefabs for UI visuals")]

    [Header("Number sprites")]
    [SerializeField]
    private List<Sprite> _spritesNumbers = new List<Sprite>();


    private void Awake()
    {
        // Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }




    public void AddResource(Type_Resource resourceToAdd)
    {
        for (int i = 0; i < _slotsResources.Count; i++)
        {
            SlotResource SlotOfInterest = _slotsResources[i];

            if (SlotOfInterest.ResourceType == resourceToAdd)
            {
                SlotOfInterest.Amount += 1;                
                // change the sprite number


                break;
            }

            if(SlotOfInterest.SlotTaken == false)
            {
                SlotOfInterest.ResourceType = resourceToAdd;
                SlotOfInterest.SlotTaken = true;

                SlotOfInterest.Amount += 1;
                _slotsResourcesTaken.Add(SlotOfInterest);
                // change the sprite number

                // create the sprite visual


                break;
            }
        }
    }


    public void RemoveResource(Type_Resource resourceToRemove)
    {
        for (int i = 0; i < _slotsResourcesTaken.Count; i++)
        {
            SlotResource SlotOfInterest = _slotsResourcesTaken[i];

            if (SlotOfInterest.ResourceType == resourceToRemove)
            {
                SlotOfInterest.Amount -= 1;
                // change the sprite number

                if (SlotOfInterest.Amount <= 0)
                {
                    SlotOfInterest.ResourceType = Type_Resource.None;
                    SlotOfInterest.SlotTaken = false;

                    // remove the sprite visual


                    _slotsResourcesTaken.Remove(SlotOfInterest);
                }

                break;
            }
        }
    }


}
