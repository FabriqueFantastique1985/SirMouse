using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    public static SkinController Instance { get; private set; }
    public Animator ClosetWrapInsideCamera;

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

    [SerializeField]
    private GameObject _emptyGameObject;

    #region Variables For Throwing To UI

    [SerializeField]
    private GameObject _panelInstantiatedUI;
    [SerializeField]
    private ButtonBaseNew _buttonCloset;

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
    // this is called from the InteractionClosetAdd
    public void AddSkinPieceToCloset(SkinType skinType, GameObject skinObject, string nameSpriteObj, SkinTransform transformSkin)  
    {
        switch (skinType)
        {
            case SkinType.Hat:
                AddSkinPieceToCorrectList(_skinsHats, _skinsUIHats, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.Head:
                AddSkinPieceToCorrectList(_skinsHeads, _skinsUIHeads, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.Body:
                AddSkinPieceToCorrectList(_skinsBody, _skinsUIBody, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.ArmUpperLeft:
                AddSkinPieceToCorrectList(_skinsArmUpperLeft, _skinsUIArmUpperLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.ArmUpperRight:
                AddSkinPieceToCorrectList(_skinsArmUpperRight, _skinsUIArmUpperRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.ElbowLeft:
                AddSkinPieceToCorrectList(_skinsArmLowerLeft, _skinsUIArmLowerLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.ElbowRight:
                AddSkinPieceToCorrectList(_skinsArmLowerRight, _skinsUIArmLowerRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.HandLeft:
                AddSkinPieceToCorrectList(_skinsHandLeft, _skinsUIHandLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.HandRight:
                AddSkinPieceToCorrectList(_skinsHandRight, _skinsUIHandRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.Tail:
                AddSkinPieceToCorrectList(_skinsTail, _skinsUITail, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.LegUpperLeft:
                AddSkinPieceToCorrectList(_skinsLegUpperLeft, _skinsUILegUpperLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.LegUpperRight:
                AddSkinPieceToCorrectList(_skinsLegUpperRight, _skinsUILegUpperRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.KneeLeft:
                AddSkinPieceToCorrectList(_skinsKneeLeft, _skinsUIKneeLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.KneeRight:
                AddSkinPieceToCorrectList(_skinsKneeRight, _skinsUIKneeRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.LegLowerLeft:
                AddSkinPieceToCorrectList(_skinsLegLowerLeft, _skinsUILegLowerLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.LegLowerRight:
                AddSkinPieceToCorrectList(_skinsLegLowerRight, _skinsUILegLowerRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.FootLeft:
                AddSkinPieceToCorrectList(_skinsFootLeft, _skinsUIFootLeft, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            case SkinType.FootRight:
                AddSkinPieceToCorrectList(_skinsFootRight, _skinsUIFootRight, skinObject, skinType, nameSpriteObj, transformSkin);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    public void CycleSkinPiece(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                CycleCorrectSkinPiece(_skinsHats, _skinsUIHats);
                break;
            case SkinType.Head:
                CycleCorrectSkinPiece(_skinsHeads, _skinsUIHeads);
                break;
            case SkinType.Body:
                CycleCorrectSkinPiece(_skinsBody, _skinsUIBody);
                break;
            case SkinType.ArmUpperLeft:
                CycleCorrectSkinPiece(_skinsArmUpperLeft, _skinsUIArmUpperLeft);
                break;
            case SkinType.ArmUpperRight:
                CycleCorrectSkinPiece(_skinsArmUpperRight, _skinsUIArmUpperRight);
                break;
            case SkinType.ElbowLeft:
                CycleCorrectSkinPiece(_skinsArmLowerLeft, _skinsUIArmLowerLeft);
                break;
            case SkinType.ElbowRight:
                CycleCorrectSkinPiece(_skinsArmLowerRight, _skinsUIArmLowerRight);
                break;
            case SkinType.HandLeft:
                CycleCorrectSkinPiece(_skinsHandLeft, _skinsUIHandLeft);
                break;
            case SkinType.HandRight:
                CycleCorrectSkinPiece(_skinsHandRight, _skinsUIHandRight);
                break;
            case SkinType.Tail:
                CycleCorrectSkinPiece(_skinsTail, _skinsUITail);
                break;
            case SkinType.LegUpperLeft:
                CycleCorrectSkinPiece(_skinsLegUpperLeft, _skinsUILegUpperLeft);
                break;
            case SkinType.LegUpperRight:
                CycleCorrectSkinPiece(_skinsLegUpperRight, _skinsUILegUpperRight);
                break;
            case SkinType.KneeLeft:
                CycleCorrectSkinPiece(_skinsKneeLeft, _skinsUIKneeLeft);
                break;
            case SkinType.KneeRight:
                CycleCorrectSkinPiece(_skinsKneeRight, _skinsUIKneeRight);
                break;
            case SkinType.LegLowerLeft:
                CycleCorrectSkinPiece(_skinsLegLowerLeft, _skinsUILegLowerLeft);
                break;
            case SkinType.LegLowerRight:
                CycleCorrectSkinPiece(_skinsLegLowerRight, _skinsUILegLowerRight);
                break;
            case SkinType.FootLeft:
                CycleCorrectSkinPiece(_skinsFootLeft, _skinsUIFootLeft);
                break;
            case SkinType.FootRight:
                CycleCorrectSkinPiece(_skinsFootRight, _skinsUIFootRight);
                break;
            default:
                Debug.Log("could not find the SkinType");
                break;
        }
    }
    public bool IsListSizeGreaterThan1(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                if (_skinsHats.Count >= 2) return true; else return false;
            case SkinType.Head:
                if (_skinsHeads.Count >= 2) return true; else return false;
            case SkinType.Body:
                if (_skinsBody.Count >= 2) return true; else return false;
            case SkinType.ArmUpperLeft:
                if (_skinsArmUpperLeft.Count >= 2) return true; else return false;
            case SkinType.ArmUpperRight:
                if (_skinsArmUpperRight.Count >= 2) return true; else return false;
            case SkinType.ElbowLeft:
                if (_skinsArmLowerLeft.Count >= 2) return true; else return false;
            case SkinType.ElbowRight:
                if (_skinsArmLowerRight.Count >= 2) return true; else return false;
            case SkinType.HandLeft:
                if (_skinsHandLeft.Count >= 2) return true; else return false;
            case SkinType.HandRight:
                if (_skinsHandRight.Count >= 2) return true; else return false;
            case SkinType.Tail:
                if (_skinsTail.Count >= 2) return true; else return false;
            case SkinType.LegUpperLeft:
                if (_skinsLegUpperLeft.Count >= 2) return true; else return false;
            case SkinType.LegUpperRight:
                if (_skinsLegUpperRight.Count >= 2) return true; else return false;
            case SkinType.KneeLeft:
                if (_skinsKneeLeft.Count >= 2) return true; else return false;
            case SkinType.KneeRight:
                if (_skinsKneeRight.Count >= 2) return true; else return false;
            case SkinType.LegLowerLeft:
                if (_skinsLegLowerLeft.Count >= 2) return true; else return false;
            case SkinType.LegLowerRight:
                if (_skinsLegLowerRight.Count >= 2) return true; else return false;
            case SkinType.FootLeft:
                if (_skinsFootLeft.Count >= 2) return true; else return false;
            case SkinType.FootRight:
                if (_skinsFootRight.Count >= 2) return true; else return false;
            default:
                Debug.Log("could not find the SkinType");
                return false;
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
        _allLists.Add(_skinsTail);
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
        _allLists.Add(_skinsUITail);
        _allLists.Add(_skinsUILegUpperLeft);
        _allLists.Add(_skinsUILegUpperRight);
        _allLists.Add(_skinsUIKneeLeft);
        _allLists.Add(_skinsUIKneeRight);
        _allLists.Add(_skinsUILegLowerLeft);
        _allLists.Add(_skinsUILegLowerRight);
        _allLists.Add(_skinsUIFootLeft);
        _allLists.Add(_skinsUIFootRight);
    }
    private void AddEmptyGameObjectToEachList() // empty gameObject == the default look of SirMouse
    {
        for (int i = 0; i < _allLists.Count; i++)
        {
            var newEmptyObject = Instantiate(_emptyGameObject);

            _allLists[i].Add(newEmptyObject);
            // set the empty object to true
            _allLists[i][0].SetActive(true);
        }
    }
    private void AddSkinPieceToCorrectList(List<GameObject> listToExpand, List<GameObject> listToExpandUI, GameObject skinObject, SkinType skinType, string nameSpriteObj, SkinTransform transformSkin)
    {
        // loop through all items in the list - if an item has the exact same name -> it will have alrdy been added (change this maybe from String check to Enum check ?)
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
        for (int i = 0; i < listToCycleThrough.Count; i++)  
        {
            if (listToCycleThrough[i].activeSelf == true) 
            {
                // disable the current one
                listToCycleThrough[i].SetActive(false);  
                listToCycleThroughUI[i].SetActive(false);  

                // if i am at the end of the list -> activate 0
                if ((i + 1) == listToCycleThrough.Count)
                {
                    listToCycleThrough[0].SetActive(true);
                    listToCycleThroughUI[0].SetActive(true);
                    //Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[0].name);
                    break;
                }
                else // else activate the next object
                {
                    listToCycleThrough[i + 1].SetActive(true);
                    listToCycleThroughUI[i + 1].SetActive(true);
                    Debug.Log("disabled " + listToCycleThrough[i].name + " to enable " + listToCycleThrough[i + 1].name);
                    break;
                }
            }
        }
    }
    private void PositionSkinPieceProperly(GameObject object1, GameObject object2, SkinTransform transfSkin)
    {
        object1.transform.localPosition = transfSkin.Position;
        object1.transform.localRotation = Quaternion.Euler(transfSkin.Rotation);
        object1.transform.localScale = transfSkin.Scale;

        object2.transform.localPosition = transfSkin.Position;
        object2.transform.localRotation = Quaternion.Euler(transfSkin.Rotation);
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
