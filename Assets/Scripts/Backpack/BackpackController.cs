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
    private GameObject _panelInstantiatedUI;
    [SerializeField]
    private GameObject _emptyGameobject;

    private GameObject _uiImageForBag;

    // vars for throwing in bag
    float _speed;
    float _arcHeight;
    float _stepScale;
    float _progress;
    GameObject _objectToMove;
    Vector2 _startPos, _endPos;
    //

    [Header("testing backpack")]
    [SerializeField]
    private bool _playerHasEquipedItem;
    [SerializeField]
    private GameObject _interactableEquiped;

    [Header("Button Animation Components")]
    [SerializeField]
    private ButtonBaseNew _buttonBackpack;
    [SerializeField]
    private ButtonBaseNew _buttonCloset;


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
        ChugObjectIntoBag();      
    }

    private void ChugObjectIntoBag()
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

        // disable the sprite + collider immediately, disable the full interactable after a bit
        interactable.GetComponent<Collider>().enabled = false;
        var spriteParent = pickupSpriteRender.gameObject.transform.parent.gameObject;
        spriteParent.SetActive(false);

        StartCoroutine(ForceObjectInBag(interactable, scaleImage));
        StartCoroutine(SetObjectToFalseAfterDelay(interactable, spriteParent));

        ItemsInBackpack.Add(typeOfPickup);
        InteractablesInBackpack.Add(interactable);

        // depending on the quantity of items in the backpack, a different child should be selected
        var newButton = Instantiate(ButtonPrefab, _pageBackpack.transform.GetChild(0));
        //var buttonScript = newButton.GetComponent<BackpackPickupButton>();
        var buttonScript = newButton.GetComponent<ButtonPickupBackpack>();

        buttonScript.MyImage.sprite = pickupSpriteRender.sprite; 
        buttonScript.MyInteractable = interactable;
        buttonScript.MyPickupType = typeOfPickup;
    }
    public void RemoveItemFromBackpack(GameObject interactable, Type_Pickup typeOfPickup, GameObject pickupButton)
    {
        var interactableComponent = interactable.GetComponent<Interactable>();

        // check if the player alrdy has an item..
        //  -> if true, the alrdy held item goes into the backpack (AddItemToBackpack function)

        if (_playerHasEquipedItem == true)
        {
            // get the player equiped interactable & type
            var interactableScript = _interactableEquiped.GetComponent<Interactable_PickupBackpack>(); // assign _interactacbleEquiped for testing
            AddEquipedItemToBackpack(interactableScript.gameObject, interactableScript.PickupType);
        }

        StartCoroutine(GetObjectOutOfBag(interactable, interactableComponent, typeOfPickup, pickupButton));
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
        //var buttonScript = newButton.GetComponent<BackpackPickupButton>();
        var buttonScript = newButton.GetComponent<ButtonPickupBackpack>();

        buttonScript.MyImage.sprite = interactable.transform.GetChild(0).transform.GetComponent<SpriteRenderer>().sprite; // update interactable structure so this is less ugly
        buttonScript.MyInteractable = interactable;
        buttonScript.MyPickupType = typeOfPickup;
    }
    IEnumerator SetObjectToFalseAfterDelay(GameObject interactable, GameObject spriteParent)
    {
        yield return new WaitForSeconds(0.25f);

        interactable.SetActive(false);
        spriteParent.SetActive(true);
        interactable.GetComponent<Interactable>().HideBalloonBackpack();
    }
    IEnumerator ForceObjectInBag(GameObject interactable, float scaleForImage)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        Sprite interactableSprite = interactable.GetComponent<PickupInteraction>().SpriteRenderPickup.sprite;

        _uiImageForBag = Instantiate(_emptyGameobject, _panelInstantiatedUI.transform);
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
    IEnumerator GetObjectOutOfBag(GameObject interactable, Interactable interactableComponent ,Type_Pickup typeOfPickup, GameObject pickupButton)
    {
        yield return new WaitForSeconds(0.25f);

        PageController.Instance.TurnPageOff(PageType.Backpack);  // only do all of the bottom stuff after a delay (set up timer in this update)

        // actually putting the object into my hands
        GameManager.Instance.Player.PushState(new PickUpState(GameManager.Instance.Player, interactableComponent, typeOfPickup, true));

        //interactable.SetActive(true);
        //interactable.transform.SetParent(GameManager.Instance.Player.transform); // update this with sirmouse's hand
        //interactable.transform.localPosition = new Vector3(0, 0, 0);

        // state still needs to enable the object !!!
        // check equpiedItem status in states !!!

        ItemsInBackpack.Remove(typeOfPickup);
        InteractablesInBackpack.Remove(interactable);

        Destroy(pickupButton);
    }
    private void ImageArrivedInBag()
    {
        // activates animation bag
        _buttonBackpack.PlayAnimationPress();

        // destroy the UI image
        Destroy(_uiImageForBag);

        this.enabled = false;
    }
    private void AllocateValues(float speed, float arcHeight, float stepScale, float progress, Vector2 startPos, Vector2 endPos, GameObject uICopy)
    {
        _speed = speed;
        _progress = progress;
        _stepScale = stepScale;
        _arcHeight = arcHeight;

        _startPos = startPos;
        _endPos = endPos;

        _objectToMove = uICopy;
    }

    #endregion
}
