using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Touch_Ingredient : Touch_Physics
{
    public delegate void TouchMoveIngredientDelegate(Touch_Physics_Object_Ingredient thisTouchMove);
    public event TouchMoveIngredientDelegate OnDrop;


    [SerializeField]
    private Type_Ingredient _typeOfIngredient;

    private Touch_Physics_Object_Ingredient _touchPhysicIngr;

    [SerializeField]
    private GameObject _raycastTestObject;

    [SerializeField]
    private RecipeController _recipeController;


    protected override void AddPhysicsScript(GameObject spawnedObj = null)
    {
        _touchPhysicIngr = spawnedObj.AddComponent<Touch_Physics_Object_Ingredient>();

        _touchPhysicIngr.TypeOfIngredient = _typeOfIngredient;
        _touchPhysicIngr.SourceIComeFrom = this;

        // assign this in the entrance trigger
        _recipeController.EntranceIngredients.CurrentlyDroppedIngredient = _touchPhysicIngr;

        OnDrop += OnDropping;

        _physicsScriptOnSpawnedObject = spawnedObj.GetComponent<Touch_Physics_Object>();      
    }


    private void OnDropping(Touch_Physics_Object_Ingredient obj)
    {
        OnDrop -= OnDropping;

        // Raycast to check if mouse is above chest
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("IngredientTriggers");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.gameObject == _recipeController.CauldronRaycast.gameObject) // GIVE PROPER IF STATEMENT
            {
                // absorb ingredient
                _recipeController.EntranceIngredients.AbsorbIngredient(_recipeController.EntranceIngredients.CurrentlyDroppedIngredient);
            }
        }
    }


    protected override void FollowMouseCalculations()
    {
        //base.FollowMouseCalculations();

        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = GameManager.Instance.CurrentCamera.ScreenToWorldPoint(_mousePosition);

        // puts the object from screenXY into world
        _spawnedObject.transform.position = _mouseWorldPosXY;
        if (_raycastTestObject != null)
        {
            _raycastTestObject.transform.position = _mouseWorldPosXY;
        }

        GameManager.Instance.FollowCamera.PointForRaycasting.transform.position = new Vector3(_mouseWorldPosXY.x, _mouseWorldPosXY.y, GameManager.Instance.FollowCamera.PointForRaycasting.transform.position.z);
        var newDirection = (_mouseWorldPosXY - GameManager.Instance.FollowCamera.PointForRaycasting.transform.position).normalized;

        Debug.DrawRay(GameManager.Instance.FollowCamera.PointForRaycasting.transform.position, newDirection * 40, Color.red);

        Ray ray = GameManager.Instance.CurrentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
        {
            Debug.DrawLine(Camera.main.transform.position, hit.point);

            _mouseWorldPositionXYZ = hit.point;
            _spawnedObject.transform.position = _mouseWorldPositionXYZ;

            if (_raycastTestObject != null)
            {
                _raycastTestObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
    }


    protected override void LetGoOfMouse()
    {
        PlayAudio(Drop);

        _animationSpawnedObject.Play(_animPop);

        _activatedFollowMouse = false;
        _acted = false;

        _spawnedObject.transform.position = _spawnedObject.transform.position;

        _physicsScriptOnSpawnedObject.LetGo = true;
        _physicsScriptOnSpawnedObject.enabled = true;

        _rigidSpawnedObject.useGravity = true;
        _rigidSpawnedObject.isKinematic = false;

        _rigidSpawnedObject.AddForce(Camera.main.transform.right * Input.GetAxis("Mouse X") * 10f, ForceMode.Impulse);

        StartCoroutine(StopPhysicsUpdate(4f, _rigidSpawnedObject, _colSpawnedObject));

        GameManager.Instance.BlockInput = false;

        OnDrop?.Invoke(_recipeController.EntranceIngredients.CurrentlyDroppedIngredient);

        this.enabled = false;
    }

    protected override IEnumerator StopPhysicsUpdate(float timeActive, Rigidbody rigidSpawnedobject, Collider collSpawnedObject)
    {
        float currentTime = 0f;
        SortingGroup sortingGroup = rigidSpawnedobject.gameObject.GetComponent<SortingGroup>();

        // Wait until time active has passed or velocity is 0
        do
        {
            yield return null;
            currentTime += Time.deltaTime;
        } while ( currentTime < timeActive && rigidSpawnedobject && Mathf.Abs(rigidSpawnedobject.velocity.y) > 0f);
        
        // Stop physics update and set sorting layer
        StartCoroutine(base.StopPhysicsUpdate(0f, rigidSpawnedobject, collSpawnedObject));
        if (sortingGroup != null)
        {
            sortingGroup.sortingOrder = 0;
        }
        
    }

}
