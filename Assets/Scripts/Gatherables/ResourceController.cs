using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;

public class ResourceController : MonoBehaviour, IDataPersistence
{
    public static ResourceController Instance { get; private set; }


    [Header("Backpack Resource slots")]
    [SerializeField]
    private List<SlotResource> _slotsResources = new List<SlotResource>();
    private List<SlotResource> _slotsResourcesTaken = new List<SlotResource>();

    [Header("Number sprites")]
    [SerializeField]
    private List<Sprite> _spritesNumbers = new List<Sprite>();
    
    private bool _isLoaded = false;
    
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
    
    public void AddResource(Type_Resource resourceToAdd, int amount = 1)
    {
        // check if we're adding nothing
        if (resourceToAdd == Type_Resource.None)
        {
            Debug.LogError("Trying to add a resource of type None");
            return;
        }
        
        // then check for taken slots...
        SlotResource slotResourceToOccupy = CheckIfIStillHaveMoreOfThisResourceSlot(resourceToAdd);
        if (slotResourceToOccupy != null)
        {
            slotResourceToOccupy.Data.Amount += amount;
            if (slotResourceToOccupy.Data.Amount >= 9)
            {
                slotResourceToOccupy.Data.Amount = 9;
            }

            // change the sprite number
            slotResourceToOccupy.ImageAmount.sprite = _spritesNumbers[slotResourceToOccupy.Data.Amount - 1];
        }
        else
        {
            // then the other ones...
            for (int i = 0; i < _slotsResources.Count; i++)
            {
                SlotResource slotOfInterest = _slotsResources[i];
                if (slotOfInterest.Data.SlotTaken == false)
                {
                    slotOfInterest.Data.ResourceType = resourceToAdd;

                    slotOfInterest.Data.Amount += amount;
                    if (slotOfInterest.Data.Amount >= 9)
                    {
                        slotOfInterest.Data.Amount = 9;
                    }

                    _slotsResourcesTaken.Add(slotOfInterest);

                    // change the sprite number
                    slotOfInterest.ImageAmount.gameObject.SetActive(true);
                    slotOfInterest.ImageAmount.enabled = true;
                    slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Data.Amount - 1];

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

            if (slotOfInterest.Data.ResourceType == resourceToRemove)
            {
                slotOfInterest.Data.Amount -= 1;

                // change the sprite number
                if (slotOfInterest.Data.Amount > 0)
                {
                    slotOfInterest.ImageAmount.sprite = _spritesNumbers[slotOfInterest.Data.Amount - 1];
                }
                else
                {
                    // remove the visual of the number
                    slotOfInterest.ImageAmount.gameObject.SetActive(false);
                }
                

                if (slotOfInterest.Data.Amount <= 0)
                {
                    slotOfInterest.Data.ResourceType = Type_Resource.None;

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
                    if (merchantButtons[i].ResourceToDeliver.ResourceType == _slotsResourcesTaken[j].Data.ResourceType)
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
            if (_slotsResourcesTaken[i].Data.ResourceType == resourceToCheck)
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
            if (_slotsResourcesTaken[i].Data.ResourceType == resourceToCheck)
            {
                return _slotsResourcesTaken[i];
            }
        }
        return null;
    }
    
    public void LoadData(GameData data)
    {
        if (_isLoaded) return;
        _slotsResourcesTaken.Clear();
        foreach (var slot in _slotsResources)
        {
            if (data.SlotResourceDatas.ContainsKey(slot.ID) == false) continue;
            
            var slotData = data.SlotResourceDatas[slot.ID];
            if (slotData.SlotTaken == false) continue;
            
            AddResource(slotData.ResourceType, slotData.Amount);
        }
        
        _isLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        // SlotData gets saved via the slots themselves.
    }
}
