using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;

public class UIFlyingToBackpackController : MonoBehaviour
{
    public static UIFlyingToBackpackController Instance { get; private set; }

    [SerializeField]
    private AudioElement _clipLandInBackpack;

    [Header("References UI-Overlay")]
    [SerializeField]
    private GameObject _panelInstantiatedUI;
    [SerializeField]
    private GameObject _backpackSuperRef;

    [Header("Reference BackpackSuper Button")]
    [SerializeField]
    private ButtonBaseNew _buttonBackpackSuper;

    [Header("prefabs-UI of Resources - SAME STRUCTURE AS ENUM !")]
    public List<GameObject> PrefabsResourcesUI = new List<GameObject>();
    [Header("prefabs-UI of Instruments - SAME STRUCTURE AS ENUM !")]
    [SerializeField]
    private List<GameObject> _prefabsInstrumentsUI = new List<GameObject>();

    // vars for throwing in bag
    float _speed;
    float _arcHeight;
    float _stepScale;
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



    // DEF: create a prefab instance on the location of the interactable, move it to UI-bag untill it arrives, then gets destroyed
    public void ThrowItemIntoBackpack(GatherableObject gatherableObject, Type_Resource resourceType = Type_Resource.None, Type_Instrument instrumentType = Type_Instrument.None)
    {
        // disable the sprite + collider immediately, disable the full interactable after a bit
        //GameObject spriteParent = DisableVisualsPickup(interactableObj, spriteRenderer);


        /////

        if (resourceType != Type_Resource.None)
        {
            var gameObjectToUse = PrefabsResourcesUI[((int)resourceType) - 1];
            // throwing into UI element and enabling some children after delay
            StartCoroutine(SetupThrowIntoBag(gatherableObject, gameObjectToUse));
            //StartCoroutine(DeactivateInteractableAfterDelay(interactableComp, interactableObj, spriteParent, 0.25f));
        }
        else if (instrumentType != Type_Instrument.None)
        {
            var gameObjectToUse = _prefabsInstrumentsUI[((int)instrumentType) - 1];
            StartCoroutine(SetupThrowIntoBag(gatherableObject, gameObjectToUse));
        }
        else
        {
            Debug.Log("Could not find the prefab I should instantiate due to enum error");
        }

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
    IEnumerator SetupThrowIntoBag(GatherableObject gatherablePickedUp, GameObject prefabToMove)
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = GameManager.Instance.CurrentCamera.WorldToScreenPoint(gatherablePickedUp.transform.position); //
        //Debug.Log(screenPosition + " screen pos");
 
        _objectToMove = Instantiate(prefabToMove, _panelInstantiatedUI.transform);
        _objectToMove.transform.position = screenPosition;

        // the position of the bag
        var targetPosition = _backpackSuperRef.transform.position;
        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);

        _speed = 400f;
        _arcHeight = 0.5f;
        _stepScale = 0f;
        _stepScale = _speed / target_Distance;
        _arcHeight = _arcHeight * target_Distance;

        _startPos = screenPosition;
        _endPos = targetPosition;

        StartCoroutine(MoveThrownObject(_objectToMove, _startPos, _endPos));

        yield return null;
    }
    private IEnumerator MoveThrownObject(GameObject objectChugged, Vector2 startPos, Vector2 endPos)
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
            Vector3 nextPos = Vector3.Lerp(startPos, endPos, progress);
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
        _buttonBackpackSuper.PlayAnimationPress();

        // play sound
        AudioController.Instance.PlayAudio(_clipLandInBackpack);

        // destroy the UI image
        Destroy(imageObj);
    }
}
