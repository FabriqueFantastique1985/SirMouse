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

    private Transform _chosenBackpackGroup;
    [SerializeField]
    private Transform _backpackGroup0, _backpackGroup1, _backpackGroup2, _backpackGroup3;

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

    #endregion


    #region Public Functions

    // instantly called on interaction click
    public void AddItemToBackpackFromFloor(Interactable interactableComp, GameObject interactableObj, 
        Type_Pickup typeOfPickup, SpriteRenderer pickupSpriteRender, float scaleImage = 1)
    {
        ParentPickupToGameManager(interactableObj); 

        ThrowPickupIntoBackpack(interactableComp, interactableObj, scaleImage, pickupSpriteRender); // only for pickups on the floor

        BackpackListAddition(interactableObj, typeOfPickup);
        BackpackButtonCreation(interactableComp, typeOfPickup, pickupSpriteRender);
    }

    // not instant! should be called on interaction ended in BackpackExtractionState
    public void AddItemToBackpackFromHands(Interactable interactableComp, GameObject interactableObj, 
        Type_Pickup typeOfPickup, SpriteRenderer pickupSpriteRender, float scaleImage = 1)
    {
        ParentPickupToGameManager(interactableObj); 
        interactableObj.SetActive(false);

        BackpackListAddition(interactableObj, typeOfPickup);
        BackpackButtonCreation(interactableComp, typeOfPickup, pickupSpriteRender);
    }

    public void RemoveItemFromBackpackThroughButton(Interactable interactable, Type_Pickup typeOfPickup, GameObject pickupButton)
    {
        StartCoroutine(GetObjectOutOfBag(interactable.gameObject, interactable, typeOfPickup, pickupButton));
    }

    #endregion




    #region Private Functions

    private static void ParentPickupToGameManager(GameObject interactableObj)
    {
        interactableObj.transform.SetParent(GameManager.Instance.transform);
    }
    private void ThrowPickupIntoBackpack(Interactable interactableComp, GameObject interactableObj, float scaleImage, SpriteRenderer spriteRenderer)
    {
        // disable the sprite + collider immediately, disable the full interactable after a bit
        GameObject spriteParent = DisableVisualsPickup(interactableObj, spriteRenderer);
        // throwing into UI element and enabling some children after delay
        StartCoroutine(ForceObjectInBag(interactableObj, scaleImage));
        StartCoroutine(SetObjectToFalseAfterDelay(interactableComp, interactableObj, spriteParent, 0.25f));
    }
    private static GameObject DisableVisualsPickup(GameObject interactableObj, SpriteRenderer pickupSpriteRender)
    {
        interactableObj.GetComponent<Collider>().enabled = false; // turn this on again when dropping it (+ hide the balloon ?)
        var spriteParent = pickupSpriteRender.gameObject.transform.parent.gameObject;
        spriteParent.SetActive(false);
        return spriteParent;
    }
    private static void BackpackListAddition(GameObject interactableObj, Type_Pickup typeOfPickup)
    {
        ItemsInBackpack.Add(typeOfPickup);
        InteractablesInBackpack.Add(interactableObj);
    }
    private void BackpackButtonCreation(Interactable interactableComp, Type_Pickup typeOfPickup, SpriteRenderer pickupSpriteRender)
    {
        // depending on the quantity of items in the backpack, a different child should be selected
        DecideOnBackpackGroup();

        // create the button
        var newButton = Instantiate(ButtonPrefab, _chosenBackpackGroup);
        //var buttonScript = newButton.GetComponent<BackpackPickupButton>();
        var buttonScript = newButton.GetComponent<ButtonPickupBackpack>();

        buttonScript.MyImage.sprite = pickupSpriteRender.sprite;
        buttonScript.MyInteractable = interactableComp;
        buttonScript.MyPickupType = typeOfPickup;
    }


    IEnumerator SetObjectToFalseAfterDelay(Interactable interactableComp, GameObject interactableObj, GameObject spriteParent, float delay)
    {
        yield return new WaitForSeconds(delay);

        interactableObj.SetActive(false);
        spriteParent.SetActive(true);
        interactableComp.HideBalloonBackpack();
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

        PageController.Instance.TurnPageOff(PageType.Backpack);  

        // SirMouse State change  // --> plays animation ---> set equiped item
        var player = GameManager.Instance.Player;
        var pickupInteraction = interactableComponent.GetComponent<PickupInteraction>();
        player.PushState(new BackpackExtractionState(player, interactableComponent, typeOfPickup, pickupInteraction.IsTwoHandPickup, pickupButton));
        
        // update lists
        ItemsInBackpack.Remove(typeOfPickup);
        InteractablesInBackpack.Remove(interactable);

        // destroy the current button
        Destroy(pickupButton);

        // re-organize the backpack
        ReOriganizeBackpack();
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


    private void DecideOnBackpackGroup()
    {
        var interactableCount = InteractablesInBackpack.Count;
        if (interactableCount > 32)
        {
            // TOO MANY ITEMS (figure out a fix)
        }
        else if (interactableCount > 24)
        {
            _chosenBackpackGroup = _backpackGroup3;
        }
        else if (interactableCount > 16)
        {
            _chosenBackpackGroup = _backpackGroup2;
        }
        else if (interactableCount > 8)
        {
            _chosenBackpackGroup = _backpackGroup1;
        }
        else
        {
            _chosenBackpackGroup = _backpackGroup0;
        }
    }
    private void ReOriganizeBackpack()
    {
        // when I click a button in Group0, and I'm already using another Group ...
        // - get the highest group being used
        // - get a child in it
        // - set this child a different parent

        var interactableCount = InteractablesInBackpack.Count;
        if (interactableCount > 24)
        {
            _backpackGroup3.GetChild(0).transform.SetParent(_backpackGroup2);
        }
        else if (interactableCount > 16)
        {
            _backpackGroup2.GetChild(0).transform.SetParent(_backpackGroup1);
        }
        else if (interactableCount > 8)
        {
            _backpackGroup1.GetChild(0).transform.SetParent(_backpackGroup0);
        }
    }

    #endregion
}

