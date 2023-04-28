﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNeedyTouchables : InteractableNeedy
{
    [Header("Interactable To Activate")]
    [SerializeField]
    private Interactable _interactableToActivate;

    public bool ResetAble;

    [Header("Touchables")]
    public List<Touchable> WantedTouchables;

    [Header("Spawns")]
    [SerializeField]
    private List<BoxCollider> _spawnAreas = new List<BoxCollider>();
    private List<SizesSpawn> _spawnSizes = new List<SizesSpawn>();

    [Header("Particles Poof")]
    public GameObject ParticlePoofTapped;
    [SerializeField]
    private GameObject _particlePoofRespawn;

    [HideInInspector]
    public List<Touchable> HeldTouchables;


    protected override void Initialize()
    {
        base.Initialize();

        // assign spawns
        AssignSpawnValues();

        // randomize spawns
        RandomlyPositionTouchables();

        // disable theinteractableToActivate
        Invoke("DisableObjectToActivate", 0.2f);
    }
    private void DisableObjectToActivate()
    {
        _interactableToActivate.gameObject.SetActive(false);
    }
    private void AssignSpawnValues()
    {
        for (int i = 0; i < _spawnAreas.Count; i++)
        {
            SizesSpawn newSpawn = new SizesSpawn(0,0);

            newSpawn.SizeX = _spawnAreas[i].bounds.size.x;
            newSpawn.SizeZ = _spawnAreas[i].bounds.size.z;

            _spawnSizes.Add(newSpawn); 
        }
    }
    private void RandomlyPositionTouchables()
    {
        for (int i = 0; i < WantedTouchables.Count; i++)
        {
            Vector3 randomPosition = new Vector3(0, 0, 0);

            float randomX = Random.Range(_spawnAreas[i].transform.position.x - (_spawnSizes[i].SizeX / 2f), _spawnAreas[i].transform.position.x + (_spawnSizes[i].SizeX / 2f));
            float randomZ = Random.Range(_spawnAreas[i].transform.position.z - (_spawnSizes[i].SizeZ / 2f), _spawnAreas[i].transform.position.z + (_spawnSizes[i].SizeZ / 2f));
            randomPosition = new Vector3(randomX, 0, randomZ);

            WantedTouchables[i].transform.position = randomPosition;

            WantedTouchables[i].gameObject.SetActive(true);

            WantedTouchables[i].GetComponent<Touch_Needy>().MySpriteParent.SetActive(true);
            WantedTouchables[i].Collider.enabled = true;

            if (_particlePoofRespawn != null)
            {
                Instantiate(_particlePoofRespawn, randomPosition, Quaternion.identity);
            }      
        }
    }



    // called when a TouchableNeedy is clicked
    public void UpdateMyList(Touchable touchableNeedy)
    {
        if (!HeldTouchables.Contains(touchableNeedy))
        {
            HeldTouchables.Add(touchableNeedy);
        }

        //NeedyBalloon.UpdateOneRequiredTouchable();
        ThinkingBalloon.UpdateOneRequiredTouchable();

        // if I have all of them... (== wantedTouchables)
        if (HeldTouchables.Count >= WantedTouchables.Count)
        {
            // activate the newer interactable
            ActivateInteractable();
        }
    }



    public void ResetMyInteractable()
    {
        // Hide balloon conffetti interactable
        _interactableToActivate.InteractionBalloon.Hide();

        // clear HeldTouchables
        HeldTouchables.Clear();

        // reposition the touchables in WantedTouchables
        RandomlyPositionTouchables();

        // reset the value UsedSuccesfully on the WantedTouchables
        for (int i = 0; i < WantedTouchables.Count; i++)
        {
            WantedTouchables[i].UsedSuccesfully = false;
        }

        // set interactable needing touchables active, set this one to inactive (currently breaks collider of the interactable)
        ReActivateNeedy();
    }
    private void ActivateInteractable()
    {
        _interactableToActivate.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); // this needs to change 
        // disable visuals + collider (+ balloon)
        _spriteParent.SetActive(false);
        _trigger.enabled = false;
        HideNeedyBalloon();
    }
    private void ReActivateNeedy()
    {
        _interactableToActivate.gameObject.SetActive(false);

        //this.gameObject.SetActive(true);
        _spriteParent.SetActive(true);
        _trigger.enabled = true;
        ShowNeedyBalloon();

        ThinkingBalloon.ResetMyNeedyObjects();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        // if I have all the required objects...      
        if (HeldTouchables.Count < WantedTouchables.Count)
        {
            // show interactBalloon 
            ShowNeedyBalloon();
        }

        // ShowInteractionBalloon();
    }
    protected override void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)  
        {
            HideNeedyBalloon();
        }
    }
}


public class SizesSpawn
{
    public float SizeX;
    public float SizeZ;

    public SizesSpawn(float sizeX, float sizeZ)
    {
        SizeX = sizeX;
        SizeZ = sizeZ;
    }
}
