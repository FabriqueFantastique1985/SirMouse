using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; private set; }


    [Header("Backpack Resource slots")]
    [SerializeField]
    private List<SlotResource> _slotsResources;


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




    public void AddResource()
    {

    }


    public void RemoveResource()
    {

    }
}
