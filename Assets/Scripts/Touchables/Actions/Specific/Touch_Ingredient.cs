using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Ingredient : Touch_Physics
{
    [SerializeField]
    private Type_Ingredient _typeOfIngredient;

    private Touch_Physics_Object_Ingredient _touchPhysicIngr;

    [SerializeField]
    private GameObject _raycastTestObject;

    protected override void AddPhysicsScript(GameObject spawnedObj = null)
    {
        _touchPhysicIngr = spawnedObj.AddComponent<Touch_Physics_Object_Ingredient>();
        _touchPhysicIngr.TypeOfIngredient = _typeOfIngredient;
        _touchPhysicIngr.SourceIComeFrom = this;

        _physicsScriptOnSpawnedObject = spawnedObj.GetComponent<Touch_Physics_Object>();      
    }


    protected override void FollowMouseCalculations()
    {
        //base.FollowMouseCalculations();

        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = GameManager.Instance.MainCamera.ScreenToWorldPoint(_mousePosition);

        // puts the object from screenXY into world
        _spawnedObject.transform.position = _mouseWorldPosXY;
        if (_raycastTestObject != null)
        {
            _raycastTestObject.transform.position = _mouseWorldPosXY;
        }

        GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position = new Vector3(_mouseWorldPosXY.x, _mouseWorldPosXY.y, GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position.z);
        var newDirection = (_mouseWorldPosXY - GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position).normalized;

        Debug.DrawRay(GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position, newDirection * 40, Color.red);

        Ray ray = GameManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (Physics.Raycast(GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position, newDirection, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn, QueryTriggerInteraction.Collide))
        //{
        //    Debug.Log(_hit.collider + " collider hit");

        //    Debug.DrawRay(GameManager.Instance.MainCameraScript.PointForRaycasting.transform.position, newDirection * _hit.distance, Color.yellow);

        //    _mouseWorldPositionXYZ = _hit.point;
        //    _spawnedObject.transform.position = _mouseWorldPositionXYZ;
        //    if (_raycastTestObject != null)
        //    {
        //        _raycastTestObject.transform.position = _mouseWorldPositionXYZ;
        //    }

        //}

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

        this.enabled = false;
    }
}
