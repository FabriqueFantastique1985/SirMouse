using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityCore.Menus;
using UnityEngine;
using UnityEngine.UI;

public class ClosetController : MonoBehaviour
{
    public static ClosetController Instance { get; private set; }

    public LayerMask LayersToCastOn;
    public GameObject PanelToInstantiateClosetImagesOn;

    public List<Page> PagesWithinCloset = new List<Page>();

    public ButtonClosetNew ButtonCloset;
    public List<ButtonClosetOpenSpecificPieces> ButtonsClosetPagers = new List<ButtonClosetOpenSpecificPieces>();
    public SkinPieceElement CurrentlyHeldSkinPiece;

    [HideInInspector]
    public PageType PageTypeOpenedInClosetLastTime;
    [HideInInspector]
    public GameObject CurrentlyHeldObject;
    [HideInInspector]
    public Animation AnimationSpawnedObject;

    //[HideInInspector]
    public List<ButtonSkinPiece> ButtonsWithNotifications = new List<ButtonSkinPiece>();
    //[HideInInspector]
    public List<ButtonClosetOpenSpecificPieces> ButtonsClosetPagerWithNotifs = new List<ButtonClosetOpenSpecificPieces>();

    private float _closetPageArmCounter, _closetPageLegCounter, _closetPageFootCounter;

    [HideInInspector]
    public bool ActivatedFollowMouse;

    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;

    private RaycastHit _hit;

    // below is for chugging things in the closet ...(am i sure bout this?)
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

        // check whether I'm currently in the field where letting go would equip the piece
        SkinsMouseController.Instance.EquipSkinPiece(CurrentlyHeldSkinPiece);

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




    public void ClickedSkinPieceButton(GameObject objectToSpawn, SkinPieceElement skinPieceElement, Vector3 spawnPosition)
    {
        CurrentlyHeldObject = Instantiate(objectToSpawn, PanelToInstantiateClosetImagesOn.transform);
        CurrentlyHeldObject.transform.position = spawnPosition;

        CurrentlyHeldSkinPiece.MyBodyType = skinPieceElement.MyBodyType;  // null ref
        CurrentlyHeldSkinPiece.MySkinType = skinPieceElement.MySkinType;
        CurrentlyHeldSkinPiece.HidesSirMouseGeometry = skinPieceElement.HidesSirMouseGeometry;
        CurrentlyHeldSkinPiece.ScoreValue = skinPieceElement.ScoreValue;

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
    public void OpenCloset(PageType typeToOpen)
    {
        //AudioController.Instance.TurnDownVolumeForOSTAndWorld();

        // turn off all other pages, except for the closet
        PageController.Instance.TurnAllPagesOffExcept(typeToOpen);

        // open up the last page that was opened within the closet
        if (PageTypeOpenedInClosetLastTime == PageType.None)
        {
            PageController.Instance.TurnPageOn(PageType.ClosetHats);
        }
        else
        {
            PageController.Instance.TurnPageOn(PageTypeOpenedInClosetLastTime);
        }

        // update images
        PageController.Instance.OpenClosetImage(true);
        PageController.Instance.OpenBagImage(false);

        // turn on the UI player things
        SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(true);

        // sleeping ILLEGAL
        GameManager.Instance.Player.Character.SetBoolSleeping(true);
    }
    public void CloseCloset()
    {
        //AudioController.Instance.TurnDownVolumeForOSTAndWorld(false);

        // close closet page
        PageController.Instance.TurnPageOff(PageType.Closet);

        // update ui images
        PageController.Instance.OpenClosetImage(false);
        PageController.Instance.OpenBagImage(false);

        // this still needed ?
        SkinsMouseController.Instance.ClosetWrapInsideCamera.gameObject.SetActive(false);

        // sleeping allowed
        GameManager.Instance.Player.Character.SetBoolSleeping(false);
    }

    // called on GiveReward() in RewardController
    public void AddNotificationToList(ButtonSkinPiece buttonSkinPiece)
    {       
        // found will always be true, so cannot use it here...
        if (ButtonsWithNotifications.Contains(buttonSkinPiece) == false)
        {
            if (buttonSkinPiece.HasBeenNotified == false)
            {
                ButtonsWithNotifications.Add(buttonSkinPiece);
            }          
        }      
    }
    public void NotificationActivater()
    {
        for (int i = 0; i< ButtonsWithNotifications.Count; i++) // looping over the list of buttons with notifications 
        {
            // only do the following if List[i] NotifObject is not ON
            if (ButtonsWithNotifications[i].NotificationObject.activeSelf == false)
            {
                // activate notif on button skinpiece
                ButtonsWithNotifications[i].NotificationObject.SetActive(true);
                ButtonsWithNotifications[i].HasBeenNotified = true;

                // figure out what pager has a similar BodyType to the ButtonWithNotif...
                for (int j = 0; j < ButtonsClosetPagers.Count; j++)
                {
                    if (ButtonsWithNotifications[i].MySkinPieceElement.MyBodyType == ButtonsClosetPagers[j].BodyType)
                    {
                        // activate notif on found button
                        ButtonsClosetPagers[j].NotificationObject.SetActive(true);
                        // list additions (if it has not already been added)
                        if (ButtonsClosetPagerWithNotifs.Contains(ButtonsClosetPagers[j]) == false)
                        {
                            ButtonsClosetPagerWithNotifs.Add(ButtonsClosetPagers[j]);
                        }

                        ButtonsClosetPagers[j].IHaveButtonsWithNotificationOn = true;
                        ButtonsClosetPagers[j].ButtonsWithNotifsOnOnMyPage.Add(ButtonsWithNotifications[i]); // this is not adding both arms right now                    
                        break;
                    }
                    else if ((ButtonsWithNotifications[i].MySkinPieceElement.MyBodyType == Type_Body.FootRight && ButtonsClosetPagers[j].BodyType == Type_Body.FootLeft) ||
                         (ButtonsWithNotifications[i].MySkinPieceElement.MyBodyType == Type_Body.LegRight && ButtonsClosetPagers[j].BodyType == Type_Body.LegLeft) ||
                         (ButtonsWithNotifications[i].MySkinPieceElement.MyBodyType == Type_Body.ArmRight && ButtonsClosetPagers[j].BodyType == Type_Body.ArmLeft))
                    {
                        // activate notif on found button
                        ButtonsClosetPagers[j].NotificationObject.SetActive(true);
                        // list additions (if it has not already been added)
                        if (ButtonsClosetPagerWithNotifs.Contains(ButtonsClosetPagers[j]) == false)
                        {
                            ButtonsClosetPagerWithNotifs.Add(ButtonsClosetPagers[j]);
                        }

                        ButtonsClosetPagers[j].IHaveButtonsWithNotificationOn = true;
                        ButtonsClosetPagers[j].ButtonsWithNotifsOnOnMyPage.Add(ButtonsWithNotifications[i]);
                        break;
                    }
                }
            }
        }

        // activate notif on closet button
        if (ButtonsWithNotifications.Count > 0)
        {          
            ButtonCloset.NotificationObject.SetActive(true);
            ButtonCloset.IhaveNotificationsReadyInTheCloset = true;
        }
    }

    // called when clicking on a piece
    public void NotificationRemover(ButtonSkinPiece buttonSkinPiece)
    {
        // only jump into the removing logic if this was no TriedOutYet
        if (buttonSkinPiece.TriedThisOut == false)
        {
            buttonSkinPiece.TriedThisOut = true;
            buttonSkinPiece.NotificationObject.SetActive(false);

            // remove it from the list "skinPiece buttons" ... 
            ButtonsWithNotifications.Remove(buttonSkinPiece);

            // AND some other lists...
            for (int i = 0; i < ButtonsClosetPagerWithNotifs.Count; i++)
            {
                // prior IF will not be true if clicking a "Right" limb, hence the extra ELSE IF
                if (buttonSkinPiece.MySkinPieceElement.MyBodyType == ButtonsClosetPagerWithNotifs[i].BodyType)
                {
                    CascadeNotificationLogic(buttonSkinPiece, i);
                    break;
                }
                else if ((buttonSkinPiece.MySkinPieceElement.MyBodyType == Type_Body.FootRight && ButtonsClosetPagerWithNotifs[i].BodyType == Type_Body.FootLeft) ||
                         (buttonSkinPiece.MySkinPieceElement.MyBodyType == Type_Body.LegRight && ButtonsClosetPagerWithNotifs[i].BodyType == Type_Body.LegLeft) ||
                         (buttonSkinPiece.MySkinPieceElement.MyBodyType == Type_Body.ArmRight && ButtonsClosetPagerWithNotifs[i].BodyType == Type_Body.ArmLeft))
                {
                    CascadeNotificationLogic(buttonSkinPiece, i);
                    break;
                }
            }
        }
    }
    private void CascadeNotificationLogic(ButtonSkinPiece buttonSkinPiece, int i)
    {
        ButtonsClosetPagerWithNotifs[i].ButtonsWithNotifsOnOnMyPage.Remove(buttonSkinPiece);

        // check if the ButtonClosetSpecificPieces still has any buttons remaining with an active notif
        if (ButtonsClosetPagerWithNotifs[i].ButtonsWithNotifsOnOnMyPage.Count == 0)
        {
            ButtonsClosetPagerWithNotifs[i].IHaveButtonsWithNotificationOn = false;
            ButtonsClosetPagerWithNotifs[i].NotificationObject.SetActive(false);

            ButtonsClosetPagerWithNotifs.Remove(ButtonsClosetPagerWithNotifs[i]);

            // check if there are any ButtonClosetPagerWithNotifs in the list
            if (ButtonsClosetPagerWithNotifs.Count == 0)
            {
                ButtonCloset.IhaveNotificationsReadyInTheCloset = false;
                ButtonCloset.NotificationObject.SetActive(false);
            }
        }
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
