using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackController : MonoBehaviour
{
    public static BackpackController BackpackInstance;

    public static List<PickupType> ItemsInBackpack = new List<PickupType>();
    public static List<GameObject> InteractablesInBackpack = new List<GameObject>();


    #region Unity Functions

    private void Awake()
    {
        if (BackpackInstance == null)
        {
            //Configure();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        //Dispose();
    }

    #endregion


    #region Public Functions



    #endregion


    #region Private Functions

    //private void Configure()
    //{
    //    BackpackInstance = this;
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}
    //private void Dispose()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    #endregion



    public static void AddItemToBackpack(GameObject interactable, PickupType typeOfPickup)
    {
        ItemsInBackpack.Add(typeOfPickup);

        /*
         * - instantiate a prefab button pn the panel of the inventory (panel should have a layout group)
         * - this button has need the knowdledge that it should activate certain interactable
         *   - assign it a script BackpackReference
         * */
    }

    public static void RemoveItemFromBackpack(PickupType typeOfPickup)
    {
        ItemsInBackpack.Remove(typeOfPickup);
    }


    /* 
     * OnInteractableBalloonClicked()
     * - use the logic from prototype to chug the sprite into the backpack
     *   - this sprite is instantiated as a copy of whatever the sprite parent is on of the interactable
     *   
     * - parent the interactable to gamemanager (or any other part of dont destroyonload)
     * - setActive(false) the interactable
     * - add the itemType to the list
     * 
     * - instantiate a prefab button pn the panel of the inventory (panel should have a layout group)
     * - this button has need the knowdledge that it should activate certain interactable
     *   - assign it a script BackpackReference
     * 
     * 
     * 
     * OnButtonClicked()
     * - disable ui for backpack
     * - setActive(true) the interactable
     * 
     * - parent it to sir mouse 
     * - assign it to sir mouses EquipedItem
     * - remove the itemType from the list
     * 

    */
    
}
