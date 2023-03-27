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

    [Header("Prefabs for UI visuals")]
    [SerializeField]
    private List<GameObject> _prefabsUI = new List<GameObject>();

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
            SlotResource slotOfInterest = _slotsResources[i];

            if (slotOfInterest.ResourceType == resourceToAdd)
            {
                slotOfInterest.Amount += 1;
                if (slotOfInterest.Amount >= 9)
                {
                    slotOfInterest.Amount = 9;
                }

                // change the sprite number
                slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Amount - 1];

                break;
            }

            if(slotOfInterest.SlotTaken == false)
            {
                slotOfInterest.ResourceType = resourceToAdd;
                slotOfInterest.SlotTaken = true;

                slotOfInterest.Amount += 1;
                if (slotOfInterest.Amount >= 9)
                {
                    slotOfInterest.Amount = 9;
                }

                _slotsResourcesTaken.Add(slotOfInterest);

                // change the sprite number
                slotOfInterest.ImageAmount.enabled = true;
                slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Amount - 1];

                // create the sprite visual
                Instantiate(_prefabsUI[((int)resourceToAdd) - 1], slotOfInterest.ParentInstantiatedPrefab.transform);

                break;
            }
        }
    }


    public void RemoveResource(Type_Resource resourceToRemove)
    {
        for (int i = 0; i < _slotsResourcesTaken.Count; i++)
        {
            SlotResource slotOfInterest = _slotsResourcesTaken[i];

            if (slotOfInterest.ResourceType == resourceToRemove)
            {
                slotOfInterest.Amount -= 1;

                // change the sprite number
                slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Amount - 1];

                if (slotOfInterest.Amount <= 0)
                {
                    slotOfInterest.ResourceType = Type_Resource.None;
                    slotOfInterest.SlotTaken = false;

                    // remove the sprite visual
                    Destroy(slotOfInterest.ParentInstantiatedPrefab.transform.GetChild(0).gameObject);

                    _slotsResourcesTaken.Remove(slotOfInterest);
                }

                break;
            }
        }
    }


}
