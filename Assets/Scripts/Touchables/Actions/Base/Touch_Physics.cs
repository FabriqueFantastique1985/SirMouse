using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class Touch_Physics : Touch_Action
{
    [Header("Spawner/Physics Specifics")]
    [SerializeField]
    protected GameObject _objectToSpawn; // drag in the Sprite_Parent 
    [SerializeField]
    protected SpriteRenderer _spriteUnderParentToSpawn;
    [SerializeField]
    protected List<Sprite> _possibleSpawnedSprites = new List<Sprite>();

    protected GameObject _spawnedObject;

    [SerializeField]
    private GameObject _prefabParticlePoof;

    [HideInInspector]
    public List<GameObject> SpawnedObjects = new List<GameObject>();

    [SerializeField]
    private int _spawnLimit = 5;

    [SerializeField]
    protected SphereCollider _spriteCollider;
    protected SphereCollider _colSpawnedObject;

    protected bool _acted;

    // physics ref
    protected Touch_Physics_Object _physicsScriptOnSpawnedObject;
    protected Rigidbody _rigidSpawnedObject;
    protected Animation _animationSpawnedObject;

    // values for following mouse  
    protected bool _activatedFollowMouse;

    protected Vector3 _mousePosition;
    protected Vector3 _mouseWorldPosXY;
    protected Vector3 _mouseWorldPositionXYZ;

    protected RaycastHit _hit;


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
    protected virtual void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {

            AudioController.Instance.PlayAudio(AudioElements[Random.Range(0, AudioElements.Count)]);

            LetGoOfMouse();            
        }
        else if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
    }
    protected virtual void LetGoOfMouse()
    {
        _animationSpawnedObject.Play(_animPop);

        _activatedFollowMouse = false;
        _acted = false;

        // there's physics present, calculate an offset so the object falls a litte bit
        Vector3 direction = -Camera.main.transform.forward;
        Vector3 calcualtedExtraDistance = direction * 5;

        _spawnedObject.transform.position = _spawnedObject.transform.position + calcualtedExtraDistance;

        _physicsScriptOnSpawnedObject.LetGo = true;
        _physicsScriptOnSpawnedObject.enabled = true;

        _rigidSpawnedObject.useGravity = true;
        _rigidSpawnedObject.isKinematic = false;

        _rigidSpawnedObject.velocity = Vector3.zero;
        _rigidSpawnedObject.AddForce(Camera.main.transform.right * Input.GetAxis("Mouse X") * 10f, ForceMode.Impulse);

        StartCoroutine(StopPhysicsUpdate(4f, _rigidSpawnedObject, _colSpawnedObject));

        GameManager.Instance.BlockInput = false;

        this.enabled = false;
    }

    protected virtual void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = GameManager.Instance.CurrentCamera.ScreenToWorldPoint(_mousePosition);

        // puts the object from screenXY into world
        _spawnedObject.transform.position = _mouseWorldPosXY;

        // if-else is to fix casting under ground bug
        if (_mouseWorldPosXY.y > 0)
        {
            if (Physics.Raycast(_spawnedObject.transform.position, GameManager.Instance.CurrentCamera.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn, QueryTriggerInteraction.Collide))
            {
                Debug.Log(_hit.collider + " collider hit");

                Debug.DrawRay(_spawnedObject.transform.position, GameManager.Instance.CurrentCamera.transform.forward * _hit.distance, Color.yellow);

                _mouseWorldPositionXYZ = _hit.point;
                _spawnedObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            if (Physics.Raycast(_spawnedObject.transform.position, -GameManager.Instance.CurrentCamera.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn, QueryTriggerInteraction.Collide))
            {

                Debug.DrawRay(_spawnedObject.transform.position, GameManager.Instance.CurrentCamera.transform.forward * _hit.distance, Color.yellow);

                _mouseWorldPositionXYZ = _hit.point;
                _spawnedObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
    }

    private void OnDrawGizmos()
    {
        
    }


    protected virtual void SpawnObject()
    {
        GameObject spawnedObject = null;
        // randomize the sprite if possible
        if (_possibleSpawnedSprites.Count > 1)
        {
            var randomIndex = Random.Range(0, _possibleSpawnedSprites.Count - 1);
            _spriteUnderParentToSpawn.sprite = _possibleSpawnedSprites[randomIndex];
        }
        spawnedObject = Instantiate(_objectToSpawn, transform.position, Quaternion.identity);

        // make it visible
        spawnedObject.SetActive(true);

        _rigidSpawnedObject = spawnedObject.AddComponent<Rigidbody>();
        _rigidSpawnedObject.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        // add correct script to spawned object (override perhaps)
        AddPhysicsScript(spawnedObject);
        
        // collider things
        _colSpawnedObject = spawnedObject.AddComponent<SphereCollider>();
        _colSpawnedObject.radius = _spriteCollider.radius;

        // animation
        _animationSpawnedObject = spawnedObject.GetComponent<Animation>();

        _spawnedObject = spawnedObject;

        // list addition
        SpawnedObjects.Add(spawnedObject);

        // remove the object (limited for performance/memory)
        if (SpawnedObjects.Count > _spawnLimit)
        {

            if (AudioElements.Count > 2)
            {
                AudioController.Instance.PlayAudio(AudioElements[2]);
            }
            

            Instantiate(_prefabParticlePoof, SpawnedObjects[0].transform.position, Quaternion.identity);

            RemoveObjectFromList();
        }
    }

    public virtual void RemoveObjectFromList(GameObject obj = null)
    {
        if (obj != null)
        {
            Destroy(obj);
            SpawnedObjects.Remove(obj);
        }
        else
        {
            Destroy(SpawnedObjects[0]);
            SpawnedObjects.RemoveAt(0);
        }

    }

    protected virtual void AddPhysicsScript(GameObject spawnedObj = null)
    {
        _physicsScriptOnSpawnedObject = spawnedObj.AddComponent<Touch_Physics_Object>();
    }



    // called from the override event on pointer_x
    protected virtual IEnumerator StopPhysicsUpdate(float timeActive, Rigidbody rigidSpawnedobject, Collider collSpawnedObject)
    {
        yield return new WaitForSeconds(timeActive);

        if (rigidSpawnedobject != null)
        {
            rigidSpawnedobject.isKinematic = true;
            rigidSpawnedobject.useGravity = false;
            collSpawnedObject.enabled = false;

            if (_physicsScriptOnSpawnedObject != null)
            {
                _physicsScriptOnSpawnedObject.enabled = false;
            }          
        }
    }
}
