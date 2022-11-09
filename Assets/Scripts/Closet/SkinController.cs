using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    public static SkinController Instance { get; private set; }

    #region Lists Of Skins
    private List<List<GameObject>> _allLists = new List<List<GameObject>>();

    private List<GameObject> _skinsHats = new List<GameObject>();
    private List<GameObject> _skinsHeads = new List<GameObject>();
    private List<GameObject> _skinsBody = new List<GameObject>();
    private List<GameObject> _skinsArmUpperLeft = new List<GameObject>();
    private List<GameObject> _skinsArmUpperRight = new List<GameObject>();
    private List<GameObject> _skinsArmLowerLeft = new List<GameObject>();
    private List<GameObject> _skinsArmLowerRight = new List<GameObject>();
    private List<GameObject> _skinsHandLeft = new List<GameObject>();
    private List<GameObject> _skinsHandRight = new List<GameObject>();
    private List<GameObject> _skinsTail = new List<GameObject>();
    private List<GameObject> _skinsLegUpperLeft = new List<GameObject>();
    private List<GameObject> _skinsLegUpperRight = new List<GameObject>();
    private List<GameObject> _skinsKneeLeft = new List<GameObject>();
    private List<GameObject> _skinsKneeRight = new List<GameObject>();
    private List<GameObject> _skinsLegLowerLeft = new List<GameObject>();
    private List<GameObject> _skinsLegLowerRight = new List<GameObject>();
    private List<GameObject> _skinsFootLeft = new List<GameObject>();
    private List<GameObject> _skinsFootRight = new List<GameObject>();

    // ui
    private List<GameObject> _skinsUIHats = new List<GameObject>();
    private List<GameObject> _skinsUIHeads = new List<GameObject>();
    private List<GameObject> _skinsUIBody = new List<GameObject>();
    private List<GameObject> _skinsUIArmUpperLeft = new List<GameObject>();
    private List<GameObject> _skinsUIArmUpperRight = new List<GameObject>();
    private List<GameObject> _skinsUIArmLowerLeft = new List<GameObject>();
    private List<GameObject> _skinsUIArmLowerRight = new List<GameObject>();
    private List<GameObject> _skinsUIHandLeft = new List<GameObject>();
    private List<GameObject> _skinsUIHandRight = new List<GameObject>();
    private List<GameObject> _skinsUITail = new List<GameObject>();
    private List<GameObject> _skinsUILegUpperLeft = new List<GameObject>();
    private List<GameObject> _skinsUILegUpperRight = new List<GameObject>();
    private List<GameObject> _skinsUIKneeLeft = new List<GameObject>();
    private List<GameObject> _skinsUIKneeRight = new List<GameObject>();
    private List<GameObject> _skinsUILegLowerLeft = new List<GameObject>();
    private List<GameObject> _skinsUILegLowerRight = new List<GameObject>();
    private List<GameObject> _skinsUIFootLeft = new List<GameObject>();
    private List<GameObject> _skinsUIFootRight = new List<GameObject>();
    #endregion

    #region Variables For Throwing To UI

    [SerializeField]
    private GameObject _panelInstantiatedUI;
    [SerializeField]
    private ButtonBaseNew _buttonCloset;
    [SerializeField]
    private GameObject _emptyGameObject;
    private GameObject _uiImageForCloset;

    float _speed;
    float _arcHeight;
    float _stepScale;
    float _progress;
    GameObject _objectToMove;
    Vector2 _startPos, _endPos;

    #endregion

    #region Unity Functions

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
        FillListAllLists();
        AddEmptyGameObjectToEachList();

        this.enabled = false;
    }
    private void Update()
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
            ImageArrivedInCloset();
        }
    }

    #endregion

    #region Public Functions

    public void AddSkinPieceToCloset(SkinType skinType, GameObject skinObject, string nameSpriteObj, SkinTransform transformSkin)  // this is called from the InteractionClosetAdd
    {
        switch ((int)skinType)
        {
            case 0:
                AddSkinPieceToCorrectList(_skinsHats, _skinsUIHats, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 1:
                AddSkinPieceToCorrectList(_skinsHeads, _skinsUIHeads, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 2:
                AddSkinPieceToCorrectList(_skinsBody, _skinsUIBody, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 3:
                AddSkinPieceToCorrectList(_skinsArmUpperLeft, _skinsUIArmUpperLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 4:
                AddSkinPieceToCorrectList(_skinsArmUpperRight, _skinsUIArmUpperRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 5:
                AddSkinPieceToCorrectList(_skinsArmLowerLeft, _skinsUIArmLowerLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 6:
                AddSkinPieceToCorrectList(_skinsArmLowerRight, _skinsUIArmLowerRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 7:
                AddSkinPieceToCorrectList(_skinsHandLeft, _skinsUIHandLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 8:
                AddSkinPieceToCorrectList(_skinsHandRight, _skinsUIHandRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 9:
                AddSkinPieceToCorrectList(_skinsTail, _skinsUITail, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 10:
                AddSkinPieceToCorrectList(_skinsLegUpperLeft, _skinsUILegUpperLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 11:
                AddSkinPieceToCorrectList(_skinsLegUpperRight, _skinsUILegUpperRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 12:
                AddSkinPieceToCorrectList(_skinsKneeLeft, _skinsUIKneeLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 13:
                AddSkinPieceToCorrectList(_skinsKneeRight, _skinsUIKneeRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 14:
                AddSkinPieceToCorrectList(_skinsLegLowerLeft, _skinsUILegLowerLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 15:
                AddSkinPieceToCorrectList(_skinsLegLowerRight, _skinsUILegLowerRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 16:
                AddSkinPieceToCorrectList(_skinsFootLeft, _skinsUIFootLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case 17:
                AddSkinPieceToCorrectList(_skinsFootRight, _skinsUIFootRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    public void CycleSkinPiece(SkinType skinType)
    {
        switch ((int)skinType)
        {
            case 0:
                CycleCorrectSkinPiece(_skinsHats, _skinsUIHats);
                break;
            case 1:
                CycleCorrectSkinPiece(_skinsHeads, _skinsUIHeads);
                break;
            case 2:
                CycleCorrectSkinPiece(_skinsBody, _skinsUIBody);
                break;
            case 3:
                CycleCorrectSkinPiece(_skinsArmUpperLeft, _skinsUIArmUpperLeft);
                break;
            case 4:
                CycleCorrectSkinPiece(_skinsArmUpperRight, _skinsUIArmUpperRight);
                break;
            case 5:
                CycleCorrectSkinPiece(_skinsArmLowerLeft, _skinsUIArmLowerLeft);
                break;
            case 6:
                CycleCorrectSkinPiece(_skinsArmLowerRight, _skinsUIArmLowerRight);
                break;
            case 7:
                CycleCorrectSkinPiece(_skinsHandLeft, _skinsUIHandLeft);
                break;
            case 8:
                CycleCorrectSkinPiece(_skinsHandRight, _skinsUIHandRight);
                break;
            case 9:
                CycleCorrectSkinPiece(_skinsTail, _skinsUITail);
                break;
            case 10:
                CycleCorrectSkinPiece(_skinsLegUpperLeft, _skinsUILegUpperLeft);
                break;
            case 11:
                CycleCorrectSkinPiece(_skinsLegUpperRight, _skinsUILegUpperRight);
                break;
            case 12:
                CycleCorrectSkinPiece(_skinsKneeLeft, _skinsUIKneeLeft);
                break;
            case 13:
                CycleCorrectSkinPiece(_skinsKneeRight, _skinsUIKneeRight);
                break;
            case 14:
                CycleCorrectSkinPiece(_skinsLegLowerLeft, _skinsUILegLowerLeft);
                break;
            case 15:
                CycleCorrectSkinPiece(_skinsLegLowerRight, _skinsUILegLowerRight);
                break;
            case 16:
                CycleCorrectSkinPiece(_skinsFootLeft, _skinsUIFootLeft);
                break;
            case 17:
                CycleCorrectSkinPiece(_skinsFootRight, _skinsUIFootRight);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    // duplicate logic from BackpackController
    public IEnumerator ForceObjectInCloset(InteractionClosetAdd interactCloset, float scaleForImage = 1)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactCloset.transform.position);
        Sprite interactableSprite = interactCloset.SkinSpriteRenderer.sprite;

        _uiImageForCloset = Instantiate(_emptyGameObject, _panelInstantiatedUI.transform);
        var uiImageComponent = _uiImageForCloset.AddComponent<Image>();
        uiImageComponent.sprite = interactableSprite;
        _uiImageForCloset.transform.localScale = new Vector3(scaleForImage, scaleForImage, scaleForImage);
        _uiImageForCloset.SetActive(true);

        // the position of the bag
        var targetPosition = _buttonCloset.transform.position;
        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);
        float speed = 400f;
        float arcHeight = 0.5f;
        float _stepScale = 0f;
        float _progress = 0f;
        _stepScale = speed / target_Distance;
        arcHeight = arcHeight * target_Distance;

        AllocateValues(speed, arcHeight, _stepScale, _progress, screenPosition, targetPosition, _uiImageForCloset);

        this.enabled = true;

        yield return null;
    }
    public IEnumerator SetObjectToFalseAfterDelay(GameObject interactable, GameObject spriteParent)
    {
        yield return new WaitForSeconds(0.25f);

        interactable.SetActive(false);
        spriteParent.SetActive(true);
        interactable.GetComponent<Interactable>().HideBalloonBackpack();
    }

    #endregion





    #region Private Functions

    private void FillListAllLists()
    {
        _allLists.Add(_skinsHats);
        _allLists.Add(_skinsHeads);
        _allLists.Add(_skinsBody);
        _allLists.Add(_skinsArmUpperLeft);
        _allLists.Add(_skinsArmUpperRight);
        _allLists.Add(_skinsArmLowerLeft);
        _allLists.Add(_skinsArmLowerRight);
        _allLists.Add(_skinsHandLeft);
        _allLists.Add(_skinsHandRight);
        _allLists.Add(_skinsLegUpperLeft);
        _allLists.Add(_skinsLegUpperRight);
        _allLists.Add(_skinsKneeLeft);
        _allLists.Add(_skinsKneeRight);
        _allLists.Add(_skinsLegLowerLeft);
        _allLists.Add(_skinsLegLowerRight);
        _allLists.Add(_skinsFootLeft);
        _allLists.Add(_skinsFootRight);
        // ui
        _allLists.Add(_skinsUIHats);
        _allLists.Add(_skinsUIHeads);
        _allLists.Add(_skinsUIBody);
        _allLists.Add(_skinsUIArmUpperLeft);
        _allLists.Add(_skinsUIArmUpperRight);
        _allLists.Add(_skinsUIArmLowerLeft);
        _allLists.Add(_skinsUIArmLowerRight);
        _allLists.Add(_skinsUIHandLeft);
        _allLists.Add(_skinsUIHandRight);
        _allLists.Add(_skinsUILegUpperLeft);
        _allLists.Add(_skinsUILegUpperRight);
        _allLists.Add(_skinsUIKneeLeft);
        _allLists.Add(_skinsUIKneeRight);
        _allLists.Add(_skinsUILegLowerLeft);
        _allLists.Add(_skinsUILegLowerRight);
        _allLists.Add(_skinsUIFootLeft);
        _allLists.Add(_skinsUIFootRight);
    }
    private void AddEmptyGameObjectToEachList()
    {
        for (int i = 0; i < _allLists.Count; i++)
        {
            _allLists[i].Add(_emptyGameObject);
            // set the empty object to true
            _allLists[i][0].SetActive(true);
        }
    }
    private void AddSkinPieceToCorrectList(List<GameObject> listToExpand, List<GameObject> listToExpandUI, GameObject skinObject, SkinType skinType, string nameSpriteObj, SkinTransform transformSkin)
    {
        // loop through all items in the list -- if an item has the exact same name -> it will have alrdy been added
        bool addedToList = false;

        for (int i = 0; i < listToExpand.Count; i++)
        {
            if (listToExpand[i].name == nameSpriteObj)
            {
                addedToList = true;
                Debug.Log("already added this to skins !");
                break;
            }
        }

        if (addedToList == false)
        {
            // make the object invisible first
            skinObject.SetActive(false);

            // create 2 instances of the object, parent them accordingly
            var transformSirMouse = GameManager.Instance.CharacterRigRefs.FindCorrectTransform(skinType);
            var transformSirMouseUI = GameManager.Instance.CharacterRigRefsUI.FindCorrectTransform(skinType);

            var toAddSkinObjectSirMouse = Instantiate(skinObject, transformSirMouse);
            var toAddSkinObjectSirMouseUI = Instantiate(skinObject, transformSirMouseUI);

            // give the instantiated skin object proper transform positions/rotations/scales
            toAddSkinObjectSirMouse.transform.localPosition = new Vector3(0, 0, 0);
            toAddSkinObjectSirMouseUI.transform.localPosition = new Vector3(0, 0, 0);
            // TO DO
            PositionSkinPieceProperly(toAddSkinObjectSirMouse, toAddSkinObjectSirMouseUI, transformSkin);

            // add to list (this string is what is checked for whether it's been added (change to different logic ?))
            toAddSkinObjectSirMouse.name = nameSpriteObj;

            listToExpandUI.Add(toAddSkinObjectSirMouseUI);
            listToExpand.Add(toAddSkinObjectSirMouse);
        }
    }
    private void CycleCorrectSkinPiece(List<GameObject> listToCycleThrough, List<GameObject> listToCycleThroughUI)
    {
        bool foundActiveSkin = false;

        for (int i = 0; i < listToCycleThrough.Count; i++)
        {
            if (listToCycleThrough[i].activeSelf == true && foundActiveSkin == false)
            {
                //Debug.Log("COUNTING " + listToCycleThrough.Count + " and this is the i " + i);

                Debug.Log("DISABLING " + listToCycleThrough[i] + " and " + listToCycleThroughUI[i]);

                listToCycleThrough[i].SetActive(false);  // HERE !!!!
                listToCycleThroughUI[i].SetActive(false);  // HERE !!!!
                // if i am at the end of the list -> active 0
                if ((i + 1) == listToCycleThrough.Count)
                {
                    listToCycleThrough[0].SetActive(true);
                    listToCycleThroughUI[0].SetActive(true);
                    //Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[0].name);
                }
                else // else active the next list object
                {
                    listToCycleThrough[i + 1].SetActive(true);
                    listToCycleThroughUI[i + 1].SetActive(true);
                    //Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[i + 1].name);
                }

                foundActiveSkin = true;
            }
        }
    }
    private void PositionSkinPieceProperly(GameObject object1, GameObject object2, SkinTransform transfSkin)
    {
        object1.transform.localPosition = transfSkin.Position;
        object1.transform.localRotation = Quaternion.Euler(transfSkin.Rotation);
        //object1.transform.localRotation = transfSkin.Rotation;
        object1.transform.localScale = transfSkin.Scale;

        object2.transform.localPosition = transfSkin.Position;
        object2.transform.localRotation = Quaternion.Euler(transfSkin.Rotation);
        //object2.transform.localRotation = transfSkin.Rotation;
        object2.transform.localScale = transfSkin.Scale;
    }
    // duplicate logic from BackpackController
    private void ImageArrivedInCloset()
    {
        // activates animation bag
        _buttonCloset.PlayAnimationPress();

        // destroy the UI image
        Destroy(_uiImageForCloset);

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
