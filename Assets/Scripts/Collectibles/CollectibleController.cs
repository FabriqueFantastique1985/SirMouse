using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public static CollectibleController Instance;

    [SerializeField]
    private List<CollectibleReferences> _collectibleReferences = new List<CollectibleReferences>();

    private CollectibleReferences _chosesCollectibleRefs;

    private const string _popIn = "PopIn";
    private const string _popOut = "PopOut";
    private const string _popExtra = "PopExtra";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        // making sure they're inactive on start
        for (int i = 0; i < _collectibleReferences.Count; i++)
        {
            _collectibleReferences[i].ObjectToSlotIn.gameObject.SetActive(false);
            _collectibleReferences[i].ShelfToUse.gameObject.SetActive(false);
        }
    }



    public void FoundCollectible(CollectibleType typeCollectible, CollectibleSpecificType typeOfSlot, Sprite spriteToAssign)
    {
        StartCoroutine(CollectTheCollectible(typeCollectible, typeOfSlot, spriteToAssign));
    }

    private IEnumerator CollectTheCollectible(CollectibleType typeCollectible, CollectibleSpecificType typeOfSlot, Sprite spriteToAssign)
    {
        // 1) block the input, poof away the interactable with the collectible (scale down or poof / animate)
        GameManager.Instance.BlockInput = true;

        // 2) poof appear the Shelf and shortly after the ObjectToSlotIn
        // assign typeCollectible
        for (int i = 0; i < _collectibleReferences.Count; i++)
        {
            if (_collectibleReferences[i].TypeOfCollectible == typeCollectible)
            {
                _chosesCollectibleRefs = _collectibleReferences[i];
                break;
            }
        }
        // assign typeOfSlot
        for (int i = 0; i < _chosesCollectibleRefs.CollectibleSlots.Count; i++)
        {
            if (_chosesCollectibleRefs.CollectibleSlots[i].CollectibleSpecificType == typeOfSlot)
            {
                _chosesCollectibleRefs.SlotToUse = _chosesCollectibleRefs.CollectibleSlots[i].gameObject;
            }
        }
        // assign spriteToAssign
        _chosesCollectibleRefs.ImageComponentObjectToSlotIn.sprite = spriteToAssign;
        _chosesCollectibleRefs.ImageComponentObjectToSlotIn.SetNativeSize();

        yield return new WaitForSeconds(0.5f);
        // poof appear the Shelf    
        _chosesCollectibleRefs.ShelfToUse.gameObject.SetActive(true);
        _chosesCollectibleRefs.ShelfToUse.Play(_popIn);
        yield return new WaitForSeconds(0.5f);
        // poof appear the ObjectToSlotIn
        _chosesCollectibleRefs.ObjectToSlotIn.gameObject.SetActive(true);
        _chosesCollectibleRefs.ObjectToSlotIn.Play(_popIn);

        yield return new WaitForSeconds(0.3f);
        // CHANGE THIS ---> make object just appear in slot, don't waste time moving it !!!!
        // 3) move ObjectToSlotIn to correct slot
        while (Vector3.Distance(_chosesCollectibleRefs.ObjectToSlotIn.transform.position, _chosesCollectibleRefs.SlotToUse.transform.position) > 0.01f)
        {
            _chosesCollectibleRefs.ObjectToSlotIn.transform.position = Vector3.MoveTowards(_chosesCollectibleRefs.ObjectToSlotIn.transform.position, _chosesCollectibleRefs.SlotToUse.transform.position, _chosesCollectibleRefs.Step);

            yield return new WaitForEndOfFrame();
        }
        // once arrived at the desired location, activate the slotted in sprite on the shelf
        _chosesCollectibleRefs.SlotToUse.transform.GetChild(0).gameObject.SetActive(true);       
        // reset position _objectToSlotIn
        _chosesCollectibleRefs.ObjectToSlotIn.transform.position = _chosesCollectibleRefs.StartPosition;
        _chosesCollectibleRefs.ObjectToSlotIn.Play(_popExtra);

        yield return new WaitForSeconds(0.3f);
        _chosesCollectibleRefs.ObjectToSlotIn.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.8f);
        // 4) after succesfull move, poof away the crown, return control to player
        _chosesCollectibleRefs.ShelfToUse.Play(_popOut);

        GameManager.Instance.BlockInput = false;
    }


    private IEnumerator MoveToSlotInObject()
    {
        while (Vector3.Distance(_chosesCollectibleRefs.ObjectToSlotIn.transform.position, _chosesCollectibleRefs.SlotToUse.transform.position) > 0.01f)
        {
            _chosesCollectibleRefs.ObjectToSlotIn.transform.position = Vector3.MoveTowards(_chosesCollectibleRefs.ObjectToSlotIn.transform.position, _chosesCollectibleRefs.SlotToUse.transform.position, _chosesCollectibleRefs.Step);

            yield return new WaitForEndOfFrame();
        }

        // once arrived at the desired location, activate the slotted in sprite on the shelf
        _chosesCollectibleRefs.SlotToUse.transform.GetChild(0).gameObject.SetActive(true);
        _chosesCollectibleRefs.ObjectToSlotIn.gameObject.SetActive(false);

        // reset position _objectToSlotIn
        _chosesCollectibleRefs.ObjectToSlotIn.transform.position = _chosesCollectibleRefs.StartPosition;

        Debug.Log(" goal reached ");
    }
}
