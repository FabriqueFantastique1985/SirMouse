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
    [SerializeField]
    private GameObject _panelBackpackEatingPickups;
    [SerializeField]
    private Animator _buttons_Backpack_FastTravel_Group;
    [SerializeField]
    private GameObject _buttonBackpack;
    [SerializeField]
    private GameObject _emptyGameobject;

    private GameObject _uiImageForBag;

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

        this.enabled = false;
    }

    private void Update() // only enable this script once a pickup is thrown into bag
    {
        // Increment our progress from 0 at the start, to 1 when we arrive.
        _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);
        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);
        // Travel in a straight line from our start position to the target.        
        Vector3 nextPos = Vector3.Lerp(_startPos, _endPos, _progress);
        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * _arcHeight;
        // Continue as before.
        _objectToMove.transform.position = nextPos;
        // if at destination...
        if (_progress == 1.0f)
        {
            ImageArrivedInBag();
        }       
    }

    #endregion


    #region Public Functions

    public void AddItemToBackpack(GameObject interactable, Type_Pickup typeOfPickup, SpriteRenderer pickupSpriteRender, float scaleImage = 1)
    {
        interactable.transform.SetParent(GameManager.Instance.transform);
        //interactable.gameObject.SetActive(false);
        interactable.GetComponent<Collider>().enabled = false;
        //interactable.GetComponent<Interactable>().HideBalloonBackpack();

        StartCoroutine(ForceObjectInBag(interactable, scaleImage));

        ItemsInBackpack.Add(typeOfPickup);
        InteractablesInBackpack.Add(interactable);

        // depending on the quantity of items in the backpack, a different child should be selected
        var newButton = Instantiate(ButtonPrefab, _pageBackpack.transform.GetChild(0));
        var buttonScript = newButton.GetComponent<BackpackPickupButton>();

        buttonScript.MyImage.sprite = pickupSpriteRender.sprite; 
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


    IEnumerator ForceObjectInBag(GameObject interactable, float scaleForImage)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        Sprite interactableSprite = interactable.GetComponent<InteractionPickup>().SpriteRenderPickup.sprite;

        _uiImageForBag = Instantiate(_emptyGameobject, _panelBackpackEatingPickups.transform);
        var uiImageComponent = _uiImageForBag.AddComponent<Image>();
        uiImageComponent.sprite = interactableSprite;
        _uiImageForBag.transform.localScale = new Vector3(scaleForImage, scaleForImage, scaleForImage);

        // the position of the bag
        var targetPosition = _buttonBackpack.transform.position;
        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);
        float speed = 400f;
        float arcHeight = 0.5f;
        float _stepScale = 0f;
        float _progress = 0f;
        _stepScale = speed / target_Distance;
        arcHeight = arcHeight * target_Distance;

        AllocateValues(speed, arcHeight, _stepScale, _progress, screenPosition, targetPosition, _uiImageForBag);

        this.enabled = true;

        yield return null;
    }
    private void ImageArrivedInBag()
    {
        // activates animation bag
        _buttons_Backpack_FastTravel_Group.Play("POP_Backpack");

        // destroy the UI image
        Destroy(_uiImageForBag);

        this.enabled = false;
    }


    // this was assigned (and disabled) on the pointer with with backpack interaction //
    float _speed;
    float _arcHeight;
    float _stepScale;
    float _progress;

    GameObject _objectToMove;

    Vector2 _startPos, _endPos;

    public void AllocateValues(float speed, float arcHeight, float stepScale, float progress, Vector2 startPos, Vector2 endPos, GameObject uICopy)
    {
        _speed = speed;
        _progress = progress;
        _stepScale = stepScale;
        _arcHeight = arcHeight;

        _startPos = startPos;
        _endPos = endPos;

        _objectToMove = uICopy;
    }

    



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
