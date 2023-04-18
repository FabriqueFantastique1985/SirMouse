using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class BackpackController : MonoBehaviourSingleton<BackpackController>
{
    public static List<Type_Pickup> ItemsInBackpack = new List<Type_Pickup>();
    public static List<ButtonPickupBackpack> ItemButtonsInBackpack = new List<ButtonPickupBackpack>();
    public static List<GameObject> InteractablesInBackpack = new List<GameObject>();

    public GameObject ButtonPrefab;

    [SerializeField]
    private GameObject _panelInstantiatedUI;    
    [SerializeField]
    private GameObject _emptyGameobject;

    private Transform _chosenBackpackGroup;
    [SerializeField]
    private Transform _backpackGroup0, _backpackGroup1, _backpackGroup2, _backpackGroup3;

    // vars for throwing in bag
    float _speed;
    float _arcHeight;
    float _stepScale;
    //float _progress;
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
    //[SerializeField]
    //private ButtonBaseNew _buttonCloset;
    [Header("Reference UI Overlay")]
    [SerializeField]
    private GameObject _buttonBackpackReference;

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

    public void RemoveItemFromBackpackThroughButton(Interactable interactable, Type_Pickup typeOfPickup, ButtonPickupBackpack pickupButton)
    {
        StartCoroutine(GetObjectOutOfBag(interactable.gameObject, interactable, typeOfPickup, pickupButton));
    }

    // methods called related to Needy Interactables
    public void RemoveSingularItemFromBackpack(ButtonPickupBackpack buttonPickup) // called from clicking the balloon of interactable needy
    {
        //-update Type list
        ItemsInBackpack.Remove(buttonPickup.MyPickupType);

        //-update Interactable Obj list (+ destroys the interactable (maybe don't do this... -> could fix needy Interactables)
        InteractablesInBackpack.Remove(buttonPickup.MyInteractable.gameObject);
        Destroy(buttonPickup.MyInteractable.gameObject);

        //-update Button list
        ItemButtonsInBackpack.Remove(buttonPickup);
        // destroy the current button
        Destroy(buttonPickup.gameObject);
        // re-organize the backpack
        ReOriganizeBackpack();
    }
    public void RemoveSingularItemFromHands() // called from clicking the balloon of interactable needy
    {
        var player = GameManager.Instance.Player;
        // particle poof

        // destroy the item
        Destroy(player.EquippedItem.gameObject);

        // enter drop state (quickfix for now as this state does what I want )
        player.PushState(new DropState(player));
    }
    public bool PlayerHasItemOfInterest(Type_Pickup pickupTypeINeed) // checked for when entering trigger interactableNeedyPickups
    {
        // check if the backpack has 1 of the items of interest
        for (int i = 0; i < ItemsInBackpack.Count; i++)
        {
            if (ItemsInBackpack[i] == pickupTypeINeed)
            {
                return true;
            }
        }

        // check if the player has even 1 item of interest -> true
        if (GameManager.Instance.Player.EquippedPickupType == pickupTypeINeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion




    #region Private Functions

    // adding pickup to Backpack ...
    private static void ParentPickupToGameManager(GameObject interactableObj)
    {
        interactableObj.transform.SetParent(GameManager.Instance.transform);
    }
    private void ThrowPickupIntoBackpack(Interactable interactableComp, GameObject interactableObj, float scaleImage, SpriteRenderer spriteRenderer)
    {
        // disable the sprite + collider immediately, disable the full interactable after a bit
        GameObject spriteParent = DisableVisualsPickup(interactableObj, spriteRenderer);

        // throwing into UI element and enabling some children after delay
        StartCoroutine(SetupThrowIntoBag(interactableObj, scaleImage));
        StartCoroutine(DeactivateInteractableAfterDelay(interactableComp, interactableObj, spriteParent, 0.25f));
    }
    private static GameObject DisableVisualsPickup(GameObject interactableObj, SpriteRenderer pickupSpriteRender)
    {
        interactableObj.GetComponent<Collider>().enabled = false; // turn this on again when dropping it (+ hide the balloon ?)
        var spriteParent = pickupSpriteRender.gameObject.transform.parent.gameObject;
        spriteParent.SetActive(false);
        return spriteParent;
    }
    IEnumerator DeactivateInteractableAfterDelay(Interactable interactableComp, GameObject interactableObj, GameObject spriteParent, float delay)
    {
        yield return new WaitForSeconds(delay);

        interactableObj.SetActive(false);
        spriteParent.SetActive(true);
        interactableComp.HideBalloonBackpack();
    }
    IEnumerator SetupThrowIntoBag(GameObject interactable, float scaleForImage)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = GameManager.Instance.CurrentCamera.WorldToScreenPoint(interactable.transform.position); //
        Debug.Log(screenPosition + " screen pos");
        Sprite interactableSprite = interactable.GetComponent<PickupInteraction>().SpriteRenderPickup.sprite; //

        var uiImageForBag = Instantiate(_emptyGameobject, _panelInstantiatedUI.transform);

        _objectToMove = uiImageForBag;

        var uiImageComponent = uiImageForBag.AddComponent<Image>();
        uiImageComponent.sprite = interactableSprite; //

        uiImageComponent.SetNativeSize();

        uiImageForBag.transform.localScale = new Vector3(scaleForImage, scaleForImage, scaleForImage);
        uiImageForBag.transform.position = screenPosition;

        // the position of the bag
        var targetPosition = _buttonBackpackReference.transform.position;
        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);

        _speed = 400f;
        _arcHeight = 0.5f;
        _stepScale = 0f;
        //_progress = 0f;
        _stepScale = _speed / target_Distance;
        _arcHeight = _arcHeight * target_Distance;

        _startPos = screenPosition;
        _endPos = targetPosition;
        


        StartCoroutine(MoveThrownObject(_objectToMove));

        yield return null;
    }
    private IEnumerator MoveThrownObject(GameObject objectChugged)
    {
        float progress = 0;
        float arcHeight = _arcHeight;

        while (progress < 1.0f)
        {
            // Increment our progress from 0 at the start, to 1 when we arrive.
            progress = Mathf.Min(progress + Time.deltaTime * _stepScale, 1.0f);
            // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
            float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
            // Travel in a straight line from our start position to the target.        
            Vector3 nextPos = Vector3.Lerp(_startPos, _endPos, progress);
            // Then add a vertical arc in excess of this.
            nextPos.y += parabola * arcHeight;

            // Continue as before.                            
            objectChugged.transform.position = nextPos;
            //objectChugged.transform.localPosition = nextPos;

            yield return new WaitForEndOfFrame();
        }

        ImageArrivedInBag(objectChugged);
    }
    private void ImageArrivedInBag(GameObject imageObj)
    {
        // activates animation bag
        _buttonBackpack.PlayAnimationPress();

        // destroy the UI image
        Destroy(imageObj);
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

        ItemButtonsInBackpack.Add(buttonScript);
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


    // removing pickup from Backpack ...
    IEnumerator GetObjectOutOfBag(GameObject interactable, Interactable interactableComponent ,Type_Pickup typeOfPickup, ButtonPickupBackpack pickupButton)
    {
        yield return new WaitForSeconds(0.25f);

        PageController.Instance.TurnPageOff(PageType.BackpackResources);  

        // SirMouse State change  // --> plays animation ---> set equiped item
        var player = GameManager.Instance.Player;
        var pickupInteraction = interactableComponent.GetComponent<PickupInteraction>();

        player.PushState(new BackpackExtractionState(player, interactableComponent, typeOfPickup, pickupInteraction.IsTwoHandPickup)); //null ???!!!! (i do this twice?)

        // update backpack buttons and list of got items
        RemoveSingularItemFromBackpack(interactable, typeOfPickup, pickupButton);
    }
    private void RemoveSingularItemFromBackpack(GameObject interactable, Type_Pickup typeOfPickup, ButtonPickupBackpack pickupButton)
    {
        // update lists
        ItemsInBackpack.Remove(typeOfPickup);
        InteractablesInBackpack.Remove(interactable);
        ItemButtonsInBackpack.Remove(pickupButton);

        // destroy the current button
        Destroy(pickupButton.gameObject); // nullref here

        // re-organize the backpack
        ReOriganizeBackpack();
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

