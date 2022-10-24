using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touch_Spawn : Touch_Action
{
    [SerializeField]
    private GameObject _objectToSpawn; // drag in the Sprite_Parent 
    private GameObject _spawnedObject;

    [SerializeField]
    private GameObject _prefabParticlePoof;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    [SerializeField]
    private int _spawnLimit = 5;

    private bool _acted;

    // values for following mouse  
    private bool _activatedFollowMouse;

    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;

    private RaycastHit _hit;


    protected override void Start()
    {
        base.Start();
        this.enabled = false;
    }
    private void Update()
    {
        FollowMouseLogic();
    }


    public override void Act()
    {
        if (_acted == false)
        {
            base.Act();

            Debug.Log("check 1");
            AudioController.Instance.PlayAudio(AudioElements[0].Clip, AudioElements[0].Type);
            SpawnObject();

            _activatedFollowMouse = true;
            _acted = true;

            Debug.Log("acted works");

            this.enabled = true;
        }
    }


    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AudioController.Instance.PlayAudio(AudioElements[1].Clip, AudioElements[1].Type);
            LetGoOfMouse();
        }
        else if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    private void LetGoOfMouse()
    {
        //_animation.Play("Spawnable_Pop");

        _activatedFollowMouse = false;
        _acted = false;

        this.enabled = false;    
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        _spawnedObject.transform.position = _mouseWorldPosXY;

        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
        {
            //Debug.DrawRay(ParentTransform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);
            _mouseWorldPositionXYZ = _hit.point;
            _spawnedObject.transform.position = _mouseWorldPositionXYZ;
        }   
    }


    private void SpawnObject()
    {
        // might be better to use object pooling here...
        var spawnedObject = Instantiate(_objectToSpawn);
        _spawnedObject = spawnedObject;

        // list addition
        _spawnedObjects.Add(spawnedObject);

        // remove the object (limited for performance/memory)
        if (_spawnedObjects.Count > _spawnLimit)
        {
            Debug.Log("2");
            AudioController.Instance.PlayAudio(AudioElements[2].Clip, AudioElements[2].Type);

            Instantiate(_prefabParticlePoof, _spawnedObjects[0].transform.position, Quaternion.identity);

            Destroy(_spawnedObjects[0]);
            _spawnedObjects.RemoveAt(0);              
        }

        
    }
}
