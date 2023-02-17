using System.Collections;
using System.Collections.Generic;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class ClosetController : MonoBehaviour
{
    public static ClosetController Instance { get; private set; }

    public LayerMask LayersToCastOn;

    public List<Page> PagesWithinCloset = new List<Page>();

    public ButtonClosetNew ButtonCloset;
    public SkinPieceElement CurrentlyHeldSkinPiece;

    [HideInInspector]
    public PageType PageTypeOpenedInClosetLastTime;
    [HideInInspector]
    public GameObject CurrentlyHeldObject;
    [HideInInspector]
    public Animation AnimationSpawnedObject;

    [HideInInspector]
    public bool ActivatedFollowMouse;

    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;

    private RaycastHit _hit;

    [SerializeField]
    private GameObject _emptyGameObject;

    [SerializeField]
    private GameObject _panelInstantiatedUI;

    float _speed;
    float _arcHeight;
    float _stepScale;
    float _progress;
    GameObject _objectToMove;
    Vector2 _startPos, _endPos;


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
    private void Start()
    {
        this.enabled = false;
    }
    private void Update()
    {
        FollowMouseLogic();
    }



    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {
            LetGoOfMouse();
        }
        else if (ActivatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    private void LetGoOfMouse()
    {
        ActivatedFollowMouse = false;

        // remove the object 
        Destroy(CurrentlyHeldObject);
        CurrentlyHeldSkinPiece.MyBodyType = Type_Body.None;
        CurrentlyHeldSkinPiece.MySkinType = Type_Skin.None;

        this.enabled = false;
    }
    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        CurrentlyHeldObject.transform.position = _mouseWorldPosXY;

        if (_mouseWorldPosXY.y > 0)
        {
            if (Physics.Raycast(CurrentlyHeldObject.transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, LayersToCastOn))
            {
                _mouseWorldPositionXYZ = _hit.point;
                CurrentlyHeldObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            if (Physics.Raycast(CurrentlyHeldObject.transform.position, -Camera.main.transform.forward, out _hit, Mathf.Infinity, LayersToCastOn))
            {
                _mouseWorldPositionXYZ = _hit.point;
                CurrentlyHeldObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
    }




    public void ClickedSkinPieceButton(GameObject objectToSpawn, SkinPieceElement skinPieceElement)
    {
        CurrentlyHeldObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

        CurrentlyHeldSkinPiece.MyBodyType = skinPieceElement.MyBodyType;  // null ref
        CurrentlyHeldSkinPiece.MySkinType = skinPieceElement.MySkinType;
        CurrentlyHeldSkinPiece.HidesSirMouseGeometry = skinPieceElement.HidesSirMouseGeometry;

        //        AudioController.Instance.PlayAudio(AudioElements[0]);

        //        _animationSpawnedObject.Play(_animPop);
        //        _animationSpawnedObject.PlayQueued(_animIdle);

        ActivatedFollowMouse = true;

        this.enabled = true;
    }
    public void ClickedSkinPiecePageButton(PageType turnToThisPage)
    {
        foreach (Page page in PagesWithinCloset)
        {
            if (page.Type != turnToThisPage)
            {
                PageController.Instance.TurnPageOff(page.Type);
            }
        }

        PageController.Instance.TurnPageOn(turnToThisPage);

        PageTypeOpenedInClosetLastTime = turnToThisPage;
    }
    public void CloseCloset()
    {
        // close closet page
        PageController.Instance.TurnPageOff(PageType.ClosetNew);

        // update ui images
        PageController.Instance.OpenClosetImage(false);
        PageController.Instance.OpenBagImage(false);

        // this still eneded ?
        SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);
    }




    // duplicate logic from BackpackController
    public IEnumerator ForceObjectInCloset(InteractionClosetAdd interactCloset, float scaleForImage = 1)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactCloset.transform.position);
        Sprite interactableSprite = interactCloset.SkinSpriteRenderer.sprite;

        var uiImageForCloset = Instantiate(_emptyGameObject, _panelInstantiatedUI.transform);
        var uiImageComponent = uiImageForCloset.AddComponent<Image>();
        uiImageComponent.sprite = interactableSprite;

        // fix size of sprite
        uiImageComponent.SetNativeSize();

        uiImageForCloset.transform.localScale = new Vector3(scaleForImage, scaleForImage, scaleForImage);
        uiImageForCloset.SetActive(true);

        // the position of the bag
        var targetPosition = ButtonCloset.transform.position;
        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);

        _speed = 400f;
        _arcHeight = 0.5f;
        _stepScale = 0f;
        _progress = 0f;
        _stepScale = _speed / target_Distance;
        _arcHeight = _arcHeight * target_Distance;

        _startPos = screenPosition;
        _endPos = targetPosition;
        _objectToMove = uiImageForCloset;


        StartCoroutine(ChugObjectIntoBag(_objectToMove));

        yield return null;
    }
    public IEnumerator SetObjectToFalseAfterDelay(GameObject interactable, GameObject spriteParent)
    {
        yield return new WaitForSeconds(0.25f);

        interactable.SetActive(false);
        spriteParent.SetActive(true);
        interactable.GetComponent<Interactable>().HideBalloonBackpack();
    }






    // duplicate logic from BackpackController
    private void ImageArrivedInCloset(GameObject objectChugged)
    {
        // activates animation bag
        ButtonCloset.PlayAnimationPress();

        // destroy the UI image
        Destroy(objectChugged);
    }
    private IEnumerator ChugObjectIntoBag(GameObject objectChugged)
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

            yield return new WaitForEndOfFrame();
        }

        ImageArrivedInCloset(objectChugged);
    }
}
