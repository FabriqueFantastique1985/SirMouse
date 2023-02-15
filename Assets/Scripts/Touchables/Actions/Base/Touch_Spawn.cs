using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

public class Touch_Spawn : Touch_Action
{
    [Header("Spawner/Physics Specifics")]
    [SerializeField]
    private GameObject _objectToSpawn; // drag in the Sprite_Parent 
    [SerializeField]
    private SpriteRenderer _spriteUnderParentToSpawn;
    [SerializeField]
    private List<Sprite> _possibleSpawnedSprites = new List<Sprite>();

    private GameObject _spawnedObject;

    [SerializeField]
    private GameObject _prefabParticlePoof;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    [SerializeField]
    private int _spawnLimit = 5;

    private bool _acted;

    // spawned refs
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

        GameManager.Instance.BlockInput = false;

        this.enabled = false;    
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        _spawnedObject.transform.position = _mouseWorldPosXY;

        if (_mouseWorldPosXY.y > 0)
        {
            if (Physics.Raycast(_spawnedObject.transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
            {
                _mouseWorldPositionXYZ = _hit.point;
                _spawnedObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            if (Physics.Raycast(_spawnedObject.transform.position, -Camera.main.transform.forward, out _hit, Mathf.Infinity, _touchableScript.LayersToCastOn))
            {
                _mouseWorldPositionXYZ = _hit.point;
                _spawnedObject.transform.position = _mouseWorldPositionXYZ;
            }
        }
    }


    private void SpawnObject()
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

        _spawnedObject = spawnedObject;
        _animationSpawnedObject = spawnedObject.GetComponent<Animation>();

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
}
