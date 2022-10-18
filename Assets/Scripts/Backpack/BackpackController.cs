using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class BackpackController : MonoBehaviour
{
    public static BackpackController BackpackInstance { get; private set; }

    public static List<Type_Pickup> ItemsInBackpack = new List<Type_Pickup>();
    public static List<GameObject> InteractablesInBackpack = new List<GameObject>();

    public GameObject ButtonPrefab;

    [SerializeField]
    private GameObject _pageBackpack;

    [Header("testing backpack")]

    [SerializeField]
    private bool _playerHasEquipedItem;
    [SerializeField]
    private GameObject _interactableEquiped;


    #region Unity Functions

    private void Awake()
    {
        // Singleton 
        if (BackpackInstance != null && BackpackInstance != this)
        {
            Destroy(this);
            return;
        }
        BackpackInstance = this;
        DontDestroyOnLoad(BackpackInstance);
    }

    #endregion


    #region Public Functions

    public void AddItemToBackpack(GameObject interactable, Type_Pickup typeOfPickup)
    {
        interactable.transform.SetParent(GameManager.Instance.transform);
        interactable.gameObject.SetActive(false);
        interactable.GetComponent<Collider>().enabled = false;

        ItemsInBackpack.Add(typeOfPickup);
        InteractablesInBackpack.Add(interactable);

        // depending on the quantity of items in the backpack, a different child should be selected
        var newButton = Instantiate(ButtonPrefab, _pageBackpack.transform.GetChild(0));
        var buttonScript = newButton.GetComponent<BackpackPickupButton>();

        //var newButtonImage = Instantiate(interactable.transform.GetChild(0).gameObject, newButton.transform); // this was done in case the pickup exists out of multiple sprite (unfinished method)

        buttonScript.MyImage.sprite = interactable.transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite; // update interactable structure so this is less ugly
        buttonScript.MyInteractable = interactable;
        buttonScript.MyPickupType = typeOfPickup;
    }

    public void RemoveItemFromBackpack(GameObject interactable, Type_Pickup typeOfPickup, GameObject pickupButton)
    {
        // check if the player alrdy has an item..
        //  -> if true, the alrdy held item goes into the backpack (AddItemToBackpack function)
        if (_playerHasEquipedItem == true)
        {
            // get the player equiped interactable & type
            var interactableScript = _interactableEquiped.GetComponent<Interactable_PickupBackpack>(); // assign _interactacbleEquiped for testing
            AddEquipedItemToBackpack(interactableScript.gameObject, interactableScript.PickupType);
        }

        PageController.Instance.TurnPageOff(PageType.Backpack);

        interactable.SetActive(true);
        interactable.transform.SetParent(GameManager.Instance.Player.transform); // update this with sirmouse's hand
        interactable.transform.localPosition = new Vector3(0,0,0);

        // assign to EquipedItem
        _playerHasEquipedItem = true;
        _interactableEquiped = interactable;

        ItemsInBackpack.Remove(typeOfPickup);
        InteractablesInBackpack.Remove(interactable);

        Destroy(pickupButton);
    }

    #endregion


    #region Private Functions

    private void AddEquipedItemToBackpack(GameObject interactable, Type_Pickup typeOfPickup)
    {
        interactable.transform.SetParent(GameManager.Instance.transform);
        interactable.gameObject.SetActive(false);

        ItemsInBackpack.Add(typeOfPickup);
        InteractablesInBackpack.Add(interactable);

        // depending on the quantity of items in the backpack, a different child should be selected (could be children on a different page !)
        var newButton = Instantiate(ButtonPrefab, _pageBackpack.transform.GetChild(0));
        var buttonScript = newButton.GetComponent<BackpackPickupButton>();

        buttonScript.MyImage.sprite = interactable.transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite; // update interactable structure so this is less ugly
        buttonScript.MyInteractable = interactable;
        buttonScript.MyPickupType = typeOfPickup;
    }

    #endregion

    /// adding an interactable to my backpack ///
    /*
 * - instantiate a prefab button pn the panel of the inventory (panel should have a layout group)
 * - this button has need the knowdledge that it should activate certain interactable
 *   - assign it a script BackpackReference
 * - parent the interactable to gamemanager (or any other part of dont destroyonload)
 * - setActive(false) the interactable
 * - add the itemType to the list
 */

    /// taking an interactable out of the backpack ///
    /*
 * - disable ui for backpack
 * - setActive(true) the interactable
 * - parent it to sir mouse 
 * - assign it to sir mouses EquipedItem
 * - remove the itemType from the list
 * - destroy the button
 */

}
