﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Audio;
using UnityEngine;

[RequireComponent(typeof(ID))]
public class GatherableObject : MonoBehaviour, IDataPersistence
{
    public delegate void GatherableObjectDelegate(GatherableObject thisGatherable);
    public event GatherableObjectDelegate ObjectGathered;

    public AudioElement _clipPickup;
    
    [SerializeField]
    private ID _id;

    private bool _isGathered = false;
    
    private void Awake()
    {
        if (_id == null)
        {
            _id = GetComponent<ID>();            
        }
    }
    
    private void Start()
    {
        // If the object is already gathered, destroy it.
        if (_isGathered)
        {
            Destroy(gameObject);
            DataPersistenceManager.Instance.RemovePersistentObject(this);
        }
    }

    public virtual void PickedUpGatherable()
    {
        ObjectGathered?.Invoke(this);
        _isGathered = true;
    }

    public void LoadData(GameData data)
    {
        if (data.GatherableData.ContainsKey(_id))
        {
            _isGathered = data.GatherableData[_id];
        }
    }

    public void SaveData(ref GameData data)
    {
        if (string.IsNullOrEmpty(_id))
        {
            Debug.LogWarning("No id yet made! Please generate one!");
            return;
        }

        data.GatherableData[_id] = _isGathered;
    }
}
