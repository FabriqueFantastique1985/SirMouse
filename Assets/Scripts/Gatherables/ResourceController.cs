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
        // first check for taken slots...
        SlotResource slotResourceToOccupy = CheckIfIStillHaveMoreOfThisResourceSlot(resourceToAdd);
        if (slotResourceToOccupy != null)
        {
            slotResourceToOccupy.Amount += 1;
            if (slotResourceToOccupy.Amount >= 9)
            {
                slotResourceToOccupy.Amount = 9;
            }

            // change the sprite number
            slotResourceToOccupy.ImageAmount.sprite = _spritesNumbers[slotResourceToOccupy.Amount - 1];
        }
        else
        {
            // then the other ones...
            for (int i = 0; i < _slotsResources.Count; i++)
            {
                SlotResource slotOfInterest = _slotsResources[i];
                if (slotOfInterest.SlotTaken == false)
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
                    Instantiate(UIFlyingToBackpackController.Instance.PrefabsResourcesUI[((int)resourceToAdd) - 1], slotOfInterest.ParentInstantiatedPrefab.transform);

                    break;
                }
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
                if (slotOfInterest.Amount > 0)
                {
                    slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Amount - 1];
                }
                else
                {
                    // remove the visual of the number
                }
                

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


 
    public List<MerchantRequestButton> CheckAquiredResources(List<MerchantRequestButton> merchantButtons)
    {
        List<MerchantRequestButton> buttonsToPulse = new List<MerchantRequestButton>();

        for (int i = 0; i < merchantButtons.Count; i++)
        {
            // if I have not been delivered yet...
            if (merchantButtons[i].ResourceToDeliver.Delivered == false)
            {
                // check in player resources for similar type...
                for (int j = 0; j < _slotsResourcesTaken.Count; j++)
                {
                    if (merchantButtons[i].ResourceToDeliver.ResourceType == _slotsResourcesTaken[j].ResourceType)
                    {
                        // found a type that is similar...
                        buttonsToPulse.Add(merchantButtons[i]);
                    }
                }
            }
        }

        return buttonsToPulse;
    }

    public bool CheckIfIStillHaveMoreOfThisResource(Type_Resource resourceToCheck)
    {
        for (int i = 0; i < _slotsResourcesTaken.Count; i++)
        {
            if (_slotsResourcesTaken[i].ResourceType == resourceToCheck)
            {
                //Debug.Log("I still have more of this resource -> " + _slotsResourcesTaken[i].ResourceType + ", so let them PULSE");
                return true;
            }
        }

        return false;

    }
    public SlotResource CheckIfIStillHaveMoreOfThisResourceSlot(Type_Resource resourceToCheck)
    {
        for (int i = 0; i < _slotsResourcesTaken.Count; i++)
        {
            if (_slotsResourcesTaken[i].ResourceType == resourceToCheck)
            {
                return _slotsResourcesTaken[i];
            }
        }
        return null;
    }


}
