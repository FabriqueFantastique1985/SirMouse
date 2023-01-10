using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class Touch_Physics : Touch_Action
{
    [Header("Specifics")]
    [SerializeField]
    private GameObject _objectToSpawn; // drag in the Sprite_Parent 
    private GameObject _spawnedObject;

    [SerializeField]
    private GameObject _prefabParticlePoof;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    [SerializeField]
    private int _spawnLimit = 5;

    [SerializeField]
    private SphereCollider _spriteCollider;
    private SphereCollider _colSpawnedObject;

    private bool _acted;

    // physics ref
    private Touch_Physics_Object _physicsScriptOnSpawnedObject;
    private Rigidbody _rigidSpawnedObject;
    private Animation _animationSpawnedObject;

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
            //base.Act();

            AudioController.Instance.PlayAudio(AudioElements[0]);
            SpawnObject();

            _animationSpawnedObject.Play(_animPop);
            _animationSpawnedObject.PlayQueued(_animIdle);

            _activatedFollowMouse = true;
            _acted = true;
            this.enabled = true;
        }
    }


    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AudioController.Instance.PlayAudio(AudioElements[1]);
            LetGoOfMouse();            
        }
        else if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    private void LetGoOfMouse()
    {
        _animationSpawnedObject.Play(_animPop);

        _activatedFollowMouse = false;
        _acted = false;

        // there's physics present, calculate an offset so the object falls a litte bit
        Vector3 direction = -Camera.main.transform.forward;
        Vector3 calcualtedExtraDistance = direction * 5;

        _spawnedObject.transform.position = _spawnedObject.transform.position + calcualtedExtraDistance;

        _physicsScriptOnSpawnedObject.enabled = true;

        _rigidSpawnedObject.useGravity = true;
        _rigidSpawnedObject.isKinematic = false;

        _rigidSpawnedObject.AddForce(Camera.main.transform.right * Input.GetAxis("Mouse X") * 10f, ForceMode.Impulse);

        StartCoroutine(StopPhysicsUpdate(4f, _rigidSpawnedObject, _colSpawnedObject));

        GameManager.Instance.BlockInput = false;

        this.enabled = false;
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        _spawnedObject.transform.position = _mouseWorldPosXY;

        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
        {
            //Debug.DrawRay(transform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);
            _mouseWorldPositionXYZ = _hit.point;
            _spawnedObject.transform.position = _mouseWorldPositionXYZ;
        }
    }


    private void SpawnObject()
    {
        // might be better to use object pooling here...
        var spawnedObject = Instantiate(_objectToSpawn, transform);

        _rigidSpawnedObject = spawnedObject.AddComponent<Rigidbody>();
        _rigidSpawnedObject.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        _physicsScriptOnSpawnedObject = spawnedObject.AddComponent<Touch_Physics_Object>();
        // collider things
        _colSpawnedObject = spawnedObject.AddComponent<SphereCollider>();
        _colSpawnedObject.radius = _spriteCollider.radius;
        // animation
        _animationSpawnedObject = spawnedObject.GetComponent<Animation>();

        _spawnedObject = spawnedObject;

        // list addition
        _spawnedObjects.Add(spawnedObject);

        // remove the object (limited for performance/memory)
        if (_spawnedObjects.Count > _spawnLimit)
        {
            AudioController.Instance.PlayAudio(AudioElements[2]);

            Instantiate(_prefabParticlePoof, _spawnedObjects[0].transform.position, Quaternion.identity);

            Destroy(_spawnedObjects[0]);
            _spawnedObjects.RemoveAt(0);
        }
    }



    // called from the override event on pointer_x
    private IEnumerator StopPhysicsUpdate(float timeActive, Rigidbody rigidSpawnedobject, Collider collSpawnedObject)
    {
        yield return new WaitForSeconds(timeActive);

        if (rigidSpawnedobject != null)
        {
            rigidSpawnedobject.isKinematic = true;
            rigidSpawnedobject.useGravity = false;
            collSpawnedObject.enabled = false;

            _physicsScriptOnSpawnedObject.enabled = false;
        }
    }
}
