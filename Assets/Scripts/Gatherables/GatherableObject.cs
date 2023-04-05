using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private string _id;

    private bool _isGathered = false;
    
    private void Awake()
    {
        if (_id == string.Empty)
        {
            Debug.LogError("No id yet made! Please generate one!");            
        }
    }

    private void Start()
    {
        // If the object is already gathered, destroy it.
        if (_isGathered)
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        _id = System.Guid.NewGuid().ToString();
    }

    public virtual void PickedUpGatherable()
    {
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
        if (_id == string.Empty)
        {
            Debug.LogWarning("No id yet made! Please generate one!");
            return;
        }

        data.GatherableData[_id] = _isGathered;
    }
}
