using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    public static SkinController Instance { get; private set; }

    #region Lists of Skins (old)
    //private List<GameObject> _skinsHats = new List<GameObject>();
    //private List<GameObject> _skinsHeads = new List<GameObject>();
    //private List<GameObject> _skinsBody = new List<GameObject>();
    //private List<GameObject> _skinsArmUpper = new List<GameObject>();
    //private List<GameObject> _skinsArmLower = new List<GameObject>();
    //private List<GameObject> _skinsHand = new List<GameObject>();
    //private List<GameObject> _skinsTail = new List<GameObject>();
    //private List<GameObject> _skinsLegUpper = new List<GameObject>();
    //private List<GameObject> _skinsKnee = new List<GameObject>();
    //private List<GameObject> _skinsLegLower = new List<GameObject>();
    //private List<GameObject> _skinsFoot = new List<GameObject>();
    #endregion

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
    private Animator _buttons_UI_Group;
    [SerializeField]
    private GameObject _buttonCloset;
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

    #region Public Functions

    public void AddSkinPieceToCloset(SkinType skinType, GameObject skinObject, string nameSpriteObj)  // this is called from the InteractionClosetAdd
    {
        switch ((int)skinType)
        {
            case 0:
                AddSkinPieceToCorrectList(_skinsHats, _skinsUIHats, skinObject, skinType, nameSpriteObj);
                break;
            case 1:
                AddSkinPieceToCorrectList(_skinsHeads, _skinsUIHeads, skinObject, skinType, nameSpriteObj);
                break;
            case 2:
                AddSkinPieceToCorrectList(_skinsBody, _skinsUIBody, skinObject, skinType, nameSpriteObj);
                break;
            case 3:
                AddSkinPieceToCorrectList(_skinsArmUpperLeft, _skinsUIArmUpperLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 4:
                AddSkinPieceToCorrectList(_skinsArmUpperRight, _skinsUIArmUpperRight, skinObject, skinType, nameSpriteObj);
                break;
            case 5:
                AddSkinPieceToCorrectList(_skinsArmLowerLeft, _skinsUIArmLowerLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 6:
                AddSkinPieceToCorrectList(_skinsArmLowerRight, _skinsUIArmLowerRight, skinObject, skinType, nameSpriteObj);
                break;
            case 7:
                AddSkinPieceToCorrectList(_skinsHandLeft, _skinsUIHandLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 8:
                AddSkinPieceToCorrectList(_skinsHandRight, _skinsUIHandRight, skinObject, skinType, nameSpriteObj);
                break;
            case 9:
                AddSkinPieceToCorrectList(_skinsTail, _skinsUITail, skinObject, skinType, nameSpriteObj);
                break;
            case 10:
                AddSkinPieceToCorrectList(_skinsLegUpperLeft, _skinsUILegUpperLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 11:
                AddSkinPieceToCorrectList(_skinsLegUpperRight, _skinsUILegUpperRight, skinObject, skinType, nameSpriteObj);
                break;
            case 12:
                AddSkinPieceToCorrectList(_skinsKneeLeft, _skinsUIKneeLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 13:
                AddSkinPieceToCorrectList(_skinsKneeRight, _skinsUIKneeRight, skinObject, skinType, nameSpriteObj);
                break;
            case 14:
                AddSkinPieceToCorrectList(_skinsLegLowerLeft, _skinsUILegLowerLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 15:
                AddSkinPieceToCorrectList(_skinsLegLowerRight, _skinsUILegLowerRight, skinObject, skinType, nameSpriteObj);
                break;
            case 16:
                AddSkinPieceToCorrectList(_skinsFootLeft, _skinsUIFootLeft, skinObject, skinType, nameSpriteObj);
                break;
            case 17:
                AddSkinPieceToCorrectList(_skinsFootRight, _skinsUIFootRight, skinObject, skinType, nameSpriteObj);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    public void CycleSkinPiece(SkinType skinType, SkinOrientation skinOrient)
    {
        switch ((int)skinType)
        {
            case 0:
                CycleCorrectSkinPiece(_skinsHats, _skinsUIHats, skinOrient);
                break;
            case 1:
                CycleCorrectSkinPiece(_skinsHeads, _skinsUIHeads, skinOrient);
                break;
            case 2:
                CycleCorrectSkinPiece(_skinsBody, _skinsUIBody, skinOrient);
                break;
            case 3:
                CycleCorrectSkinPiece(_skinsArmUpperLeft, _skinsUIArmUpperLeft, skinOrient);
                break;
            case 4:
                CycleCorrectSkinPiece(_skinsArmUpperRight, _skinsUIArmUpperRight, skinOrient);
                break;
            case 5:
                CycleCorrectSkinPiece(_skinsArmLowerLeft, _skinsUIArmLowerLeft, skinOrient);
                break;
            case 6:
                CycleCorrectSkinPiece(_skinsArmLowerRight, _skinsUIArmLowerRight, skinOrient);
                break;
            case 7:
                CycleCorrectSkinPiece(_skinsHandLeft, _skinsUIHandLeft, skinOrient);
                break;
            case 8:
                CycleCorrectSkinPiece(_skinsHandRight, _skinsUIHandRight, skinOrient);
                break;
            case 9:
                CycleCorrectSkinPiece(_skinsTail, _skinsUITail, skinOrient);
                break;
            case 10:
                CycleCorrectSkinPiece(_skinsLegUpperLeft, _skinsUILegUpperLeft, skinOrient);
                break;
            case 11:
                CycleCorrectSkinPiece(_skinsLegUpperRight, _skinsUILegUpperRight, skinOrient);
                break;
            case 12:
                CycleCorrectSkinPiece(_skinsKneeLeft, _skinsUIKneeLeft, skinOrient);
                break;
            case 13:
                CycleCorrectSkinPiece(_skinsKneeRight, _skinsUIKneeRight, skinOrient);
                break;
            case 14:
                CycleCorrectSkinPiece(_skinsLegLowerLeft, _skinsUILegLowerLeft, skinOrient);
                break;
            case 15:
                CycleCorrectSkinPiece(_skinsLegLowerRight, _skinsUILegLowerRight, skinOrient);
                break;
            case 16:
                CycleCorrectSkinPiece(_skinsFootLeft, _skinsUIFootLeft, skinOrient);
                break;
            case 17:
                CycleCorrectSkinPiece(_skinsFootRight, _skinsUIFootRight, skinOrient);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    // duplicate logic from BackpackController
    public IEnumerator ForceObjectInCloset(GameObject interactable, float scaleForImage = 1)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        Sprite interactableSprite = interactable.GetComponent<InteractionClosetAdd>().SkinSpriteRenderer.sprite;

        _uiImageForCloset = Instantiate(_emptyGameObject, _panelInstantiatedUI.transform);
        var uiImageComponent = _uiImageForCloset.AddComponent<Image>();
        uiImageComponent.sprite = interactableSprite;
        _uiImageForCloset.transform.localScale = new Vector3(scaleForImage, scaleForImage, scaleForImage);

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
    private void AddSkinPieceToCorrectList(List<GameObject> listToExpand, List<GameObject> listToExpandUI, GameObject skinObject, SkinType skinType, string nameSpriteObj)
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
            // position/scale/rotate the pieces during runtime so they fit..
            // copy the transform values that makes it fit..
            // assign these values to the clothing piece itself in an extra value that is given through

            // add to list
            toAddSkinObjectSirMouse.name = nameSpriteObj;

            listToExpandUI.Add(toAddSkinObjectSirMouseUI);
            listToExpand.Add(toAddSkinObjectSirMouse);
        }
    }
    private void CycleCorrectSkinPiece(List<GameObject> listToCycleThrough, List<GameObject> listToCycleThroughUI, SkinOrientation skinOrient)
    {
        bool foundActiveSkin = false;

        for (int i = 0; i < listToCycleThrough.Count; i++)
        {
            if (listToCycleThrough[i].activeSelf == true && foundActiveSkin == false)
            {
                Debug.Log("COUNTING " + listToCycleThrough.Count + " and this is the i " + i);

                listToCycleThrough[i].SetActive(false);
                listToCycleThroughUI[i].SetActive(false);
                // if i am at the end of the list -> active 0
                if ((i + 1) == listToCycleThrough.Count)
                {
                    listToCycleThrough[0].SetActive(true);
                    listToCycleThroughUI[0].SetActive(true);
                    Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[0].name);
                }
                else // else active the next list object
                {
                    listToCycleThrough[i + 1].SetActive(true);
                    listToCycleThroughUI[i + 1].SetActive(true);
                    Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[i + 1].name);
                }

                foundActiveSkin = true;
            }
        }
    }

    // duplicate logic from BackpackController
    private void ImageArrivedInCloset()
    {
        // activates animation bag
        _buttons_UI_Group.Play("POP_Closet");

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
